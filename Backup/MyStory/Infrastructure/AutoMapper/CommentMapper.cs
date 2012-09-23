using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyStory.Models;
using MyStory.ViewModels;

namespace MyStory.Infrastructure.AutoMapper
{
    public class CommentMapper : IMapper
    {
        public void Execute()
        {
            Mapper.CreateMap<Comment, CommentSidebarViewModel>()
                    .ForMember(vm => vm.PostId, opt => opt.MapFrom(c => c.PostId))
                    .ForMember(vm => vm.CommentId, opt => opt.MapFrom(c => c.Id))
                    .ForMember(vm => vm.Content, opt => opt.MapFrom(c => c.Content))
                    .ForMember(vm => vm.CommenterName, opt => opt.MapFrom(c => c.Commenter.Name))
                    .ForMember(vm => vm.DateCreated, opt => opt.MapFrom(c => c.DateCreated));
        }
    }
}