using API.Model.DTOs;
using API.Model.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IPostService
    {
        public Task<bool> CreatePost(PostRequest post, string userId);
        public Task<bool> UpdatePost(PostRequest post, string postId);
        public Task<bool> DeletePost(string postId);
        public Task<List<PostDTO>> GetUserPosts(string userId);
        public Task<List<PostDTO>> GetAllPosts();
        public Task<PostDTO> GetPostById(string postId);
    }
}
