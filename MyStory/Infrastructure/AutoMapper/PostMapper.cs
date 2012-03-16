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
            Mapper.CreateMap<Post, PostDetailViewModel>()
                    .ForMember(vm=>vm.Tags, p=>p.Ignore());

            Mapper.CreateMap<Post, PostListViewModel>();

            Mapper.CreateMap<PostInput, Post>()
                    .ForMember(p=>p.Tags, i=>i.Ignore())
                    .ForMember(p => p.LocationOfWriting, opt => opt.MapFrom(i => new Location { Latitude = i.Latitude, Longitude = i.Longitude }));

            Mapper.CreateMap<Post, PostInput>()
                    .ForMember(i => i.Latitude, opt => opt.MapFrom(p => p.LocationOfWriting.Latitude))
                    .ForMember(i => i.Longitude, opt => opt.MapFrom(p => p.LocationOfWriting.Longitude));

        }
    }
}