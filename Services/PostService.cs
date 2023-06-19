using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Model.DTOs.Requests;
using API.Services.Interfaces;
using AutoMapper;
using Castle.Core.Internal;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAL.Entities;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Repositories;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly IMapper _mapper;

        #region Image vs Video extensions
        private readonly List<string> ImageExtensions = new() { ".png", ".jpg", ".jpeg" };
        private readonly List<string> VideoExtensions = new() { ".mp4", "m4p", ".m4v", ".mpg", ".mpeg", ".m2v", ".mov" };
        #endregion

        #region Cloudinary Informations

        private const string CLOUD_NAME = "dor7ghk95";
        private const string API_KEY = "588273259994552";
        private const string API_SECRET = "YImi-iuUxclgZJFC2-R0cN3tcEA";

        #endregion

        private Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public PostService(IPostRepository postRepo,
            IMapperCustom map,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment) : base(unitOfWork, map)
        {
            _postRepo = postRepo;
            _mapper = mapper;
            _webHostEnviroment = webHostEnvironment;
        }

        public async Task<bool> CreatePost(PostRequest post, string userId)
        {
            try
            {
                var isValid = post.ValidateInput();
                if (!isValid)  return false;

                var newPost = new Post()
                {
                    Title = post.Title,
                    Content = post.Content,
                    AccountId = userId,
                    IsAccepted = false,
                };

                if (post.File != null)
                {
                    Account account = new(CLOUD_NAME, API_KEY, API_SECRET);
                    _cloudinary = new Cloudinary(account);
                    var fileUrl = UploadFile(post.File);

                    if (fileUrl == string.Empty)
                        return false;

                    newPost.FileUrl = fileUrl;
                    newPost.IsContainVideo = false;
                }

                if (!post.VideoUrl.IsNullOrEmpty() && IsValidYouTubeUrl(post.VideoUrl))
                {
                    newPost.VideoUrl = post.VideoUrl;
                    newPost.IsContainVideo = true;
                }    

                await _unitOfWork.BeginTransaction();
                await _postRepo.AddAsync(newPost);
                await _unitOfWork.CommitTransaction();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeletePost(string postId)
        {
            await _unitOfWork.BeginTransaction();
            var postToDelete = await _postRepo.FindAsync(_ => _.Id == postId);
            if (postToDelete != null)
            {
                _postRepo.Delete(postToDelete);
                await _unitOfWork.CommitTransaction();
                return true;
            }
           
            return false;
        }

        public async Task<List<PostDTO>> GetAllPosts()
        {
            var listPosts = new List<PostDTO>();
            var showedPosts = await _postRepo.GetShowedPost();
            foreach (var post in showedPosts)
            {
                var postDTO = _mapper.Map<PostDTO>(post);
                postDTO.Time = GetDurationPosted(postDTO.CreatedAt);
                listPosts.Add(postDTO);
            }

            return listPosts;
        }

        public async Task<List<PostDTO>> GetUserPosts(string userId)
        {
            var listPosts = new List<PostDTO>();
            var userPosts = await _postRepo.GetQuery(_ => _.AccountId == userId && !_.IsDeleted).ToListAsync();
            foreach (var post in userPosts.OrderByDescending(_ => _.CreatedAt))
            {
                var postDTO = _mapper.Map<PostDTO>(post);
                postDTO.Time = GetDurationPosted(postDTO.CreatedAt);
                listPosts.Add(postDTO);
            }

            return listPosts;
        }

        private static string GetDurationPosted(DateTime createdAt)
        {
            var duration = Convert.ToInt32((DateTime.Now - createdAt).TotalMinutes);
            if (duration >= 60)
            {
                duration /= 60;
                if (duration >= 24)
                {
                    duration /= 24;
                    if (duration >= 168)
                        return duration + " tuần trước.";
                 
                    return duration + " ngày trước.";
                }

                return duration + " giờ trước.";
            }

            return duration + " phút trước.";
        }

        public async Task<bool> UpdatePost(PostRequest post, string postId)
        {
            try
            {
                var findPost = await _postRepo.FindAsync(p => p.Id == postId && p.IsAccepted);
                if (findPost == null)
                    return false;

                if (post.File != null)
                {
                    Account account = new(CLOUD_NAME, API_KEY, API_SECRET);
                    _cloudinary = new Cloudinary(account);
                    var fileUrl = UploadFile(post.File);

                    if (fileUrl.IsNullOrEmpty()) return false;

                    findPost.FileUrl = fileUrl;
                    findPost.IsContainVideo = false;
                }

                if (!post.VideoUrl.IsNullOrEmpty() && IsValidYouTubeUrl(post.VideoUrl))
                {
                    findPost.VideoUrl = post.VideoUrl;
                    findPost.IsContainVideo = true;
                }

                findPost.Title = post.Title ?? findPost.Title;
                findPost.Content = post.Content ?? findPost.Content;
                findPost.UpdatedAt = DateTime.Now;

                _postRepo.Update(findPost);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string UploadFile(IFormFile file)
        {
            try
            {
                var fileUrl = "";
                if (!Directory.Exists(_webHostEnviroment.WebRootPath + "\\Images\\"))
                {
                    Directory.CreateDirectory(_webHostEnviroment.WebRootPath + "\\Images\\");
                }

                using (FileStream fileStream = System.IO.File.Create(_webHostEnviroment.WebRootPath + "\\Images\\" + file.FileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    fileUrl = _webHostEnviroment.WebRootPath + "\\Images\\" + file.FileName;
                }

                if (fileUrl == "")
                    return string.Empty;

                string extension = Path.GetExtension(file.FileName).ToLower();
                if (ImageExtensions.Contains(extension))
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileUrl),
                    };

                    var uploadResult = _cloudinary.Upload(uploadParams);
                    return uploadResult.Url.ToString();
                }

                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                throw;
            }
        }

        private static bool IsValidYouTubeUrl(string url)
        {
            // Regular expression pattern to match YouTube video URLs
            string pattern = @"^(https?\:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+";

            // Create a Regex object with the pattern
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Check if the given URL matches the pattern
            Match match = regex.Match(url);

            // Return true if there is a match, indicating a valid YouTube video URL
            return match.Success;
        }

        public async Task<PostDTO> GetPostById(string postId)
        {
            var post = await _postRepo.FindAsync(p => p.Id == postId);
            return _mapper.Map<PostDTO>(post);
        }
    }
}
