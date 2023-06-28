using API.Model.DTOs;
using API.Model.DTOs.Requests;
using API.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("user")]
        public async Task<List<PostDTO>> GetUserPost()
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _postService.GetUserPosts(userId);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<PostDTO>> GetShowedPosts()
        {
            return await _postService.GetAllPosts();
        }

        [HttpGet("{id}")]
        public async Task<PostDTO> GetPost(string id)
        {
            return await _postService.GetPostById(id);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeletePost(string id)
        {
            return await _postService.DeletePost(id);
        }

        [HttpPost]
        public async Task<bool> CreatePost([FromForm] PostRequest post) 
        {
           var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
           return await _postService.CreatePost(post, userId);
        }

        [HttpPut("{id}")]
        public async Task<Post> UpdatePost(string id, [FromForm] PostRequest post)
        {
            return await _postService.UpdatePost(post, id);
        }
    }
}
