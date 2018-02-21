using System;
using System.Collections.Generic;
using System.Text;

namespace InstaBotLibrary.Instagram
{
    public class Post
    {
        public string text { get; set; }
        public string imageUrl { get; set; }
        public List<string> tags { get; set; }
        public Post() { }
        public Post(string text, string imageUrl, List<string> tags)
        {
            this.text = text;
            this.imageUrl = imageUrl;
            this.tags = tags;
        }
    }
}
