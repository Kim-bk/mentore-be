using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;

namespace API.Model.DTOs.Requests
{
    public class PostRequest
    {
        public IFormFile File { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }

        public bool ValidateInput()
        {
            if (File == null && Content.IsNullOrEmpty() && Title.IsNullOrEmpty())
                return false;

            return true;
        }
    }
}
