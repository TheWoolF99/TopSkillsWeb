using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Account
{
    public class FeedBackModel
    {
        public string EMail { get; set; }

        public string User { get; set; }

        public string mailTo { get; set; }

        public string Topic { get; set; }

        public string Message { get; set; }

        public List<IFormFile> Photo { get; set; }

        public FeedBackModel()
        {
            this.EMail = "";
            this.Message = "";
            this.Topic = "";
            this.Photo = new();
        }
    }
}
