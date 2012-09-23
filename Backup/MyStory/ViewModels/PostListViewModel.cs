using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.ViewModels
{
    public class PostListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TitleShort
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    return Title.Length > 20 ? Title.Substring(0, 20) : Title;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}