using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TurnKeyBrokerSignUp.WebUI.Models
{

    public class Blogs
    {
        public int? BlogId { get; set; }
        public int? AuthorID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Blogger { get; set; }
        public string Article { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string PostedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public long? Views { get; set; }

        public int? TotalComments { get; set; }

        
        public string GenerateSlug()
        {
            string phrase = string.Format("{0}", Title);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = str.Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}