using API.Model.DTOs;
using API.Model.DTOs.Requests;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IPostService
    {
        public Task<bool> CreatePost(PostRequest post, string userId);
        public Task<Post> UpdatePost(PostRequest post);
        public Task<bool> DeletePost(string postId);
        public Task<List<PostDTO>> GetUserPosts(string userId);
        public Task<List<PostDTO>> GetAllPosts();
        public Task<PostDTO> GetPostById(string postId);
    }
}
