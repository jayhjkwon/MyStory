using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyStory.Models;
using MyStory.ViewModels;

namespace MyStory.Infrastructure.AutoMapper
{
    public class PostMapper : IMapper
    {
        public void Execute()
        {
            Mapper.CreateMap<Post, PostListViewModel>()
                    .ForMember(vm => vm.Content, opt => opt.MapFrom(p => p.ContentWithHtml));
        }
    }
}