using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentore.Models.DTOs.Responses.Base;

namespace Mentore.Models.DTOs.Responses
{
    public class IntervalResponse : GeneralResponse
    {
        public string Color {get; set;}
        public string Unit {get; set;}
        public string Title { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
    }
}