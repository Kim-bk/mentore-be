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
        private readonly UploadImageService _uploadImageService;

        public PostService(IPostRepository postRepo,
            IMapperCustom map,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            UploadImageService uploadImageService) : base(unitOfWork, map)
        {
            _postRepo = postRepo;
            _mapper = mapper;
            _uploadImageService = uploadImageService;
        }

        public async Task<bool> CreatePost(PostRequest post, string userId)
        {
            try
            {
                var isValid = post.ValidateInput();
                if (!isValid)  return false;

                var newPost = new Post()
                {
                    Title = post.Title.ToUpper(),
                    Content = post.Content,
                    AccountId = userId,
                    IsAccepted = false,
                };

                if (post.File != null)
                {
                    var fileUrl = _uploadImageService.UploadFile(post.File);
                    if (fileUrl == string.Empty)
                        return false;

                    newPost.FileUrl = fileUrl;
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

        public async Task<Post> UpdatePost(PostRequest post, string postId)
        {
            try
            {
                var findPost = await _postRepo.FindAsync(p => p.Id == postId && !p.IsDeleted);
                if (findPost == null)
                    return null;

                if (post.File != null)
                {
                    var fileUrl = _uploadImageService.UploadFile(post.File);
                    if (fileUrl.IsNullOrEmpty()) return null;

                    findPost.FileUrl = fileUrl;
                }

                findPost.Title = post.Title.ToUpper() ?? findPost.Title.ToUpper();
                findPost.Content = post.Content ?? findPost.Content;
                findPost.UpdatedAt = DateTime.Now;

                _postRepo.Update(findPost);
                await _unitOfWork.CommitTransaction();
                return findPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostDTO> GetPostById(string postId)
        {
            var post = await _postRepo.FindAsync(p => p.Id == postId);
            return _mapper.Map<PostDTO>(post);
        }
    }
}
