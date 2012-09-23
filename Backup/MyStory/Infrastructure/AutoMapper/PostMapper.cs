using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MyStory.Models;
using MyStory.ViewModels;
using MarkdownDeep;

namespace MyStory.Infrastructure.AutoMapper
{
    public class PostMapper : IMapper
    {
        public void Execute()
        {
            Mapper.CreateMap<Post, PostDetailViewModel>()
                    .ForMember(vm=>vm.Tags, p=>p.Ignore());

            var md = new Markdown();
            md.SafeMode = true;
            md.ExtraMode = true;

            Mapper.CreateMap<Post, PostListViewModel>()
                .ForMember(vm=>vm.Content, opt=>opt.MapFrom(p=> md.Transform( p.Content.Length>500 ? p.Content.Substring(0,500) : p.Content)));

            Mapper.CreateMap<PostInput, Post>()
                    .ForMember(p=>p.Tags, i=>i.Ignore())
                    .ForMember(p => p.LocationOfWriting, opt => opt.MapFrom(i => new Location { Latitude = i.Latitude, Longitude = i.Longitude }));

            Mapper.CreateMap<Post, PostInput>()
                    .ForMember(i => i.Latitude, opt => opt.MapFrom(p => p.LocationOfWriting.Latitude))
                    .ForMember(i => i.Longitude, opt => opt.MapFrom(p => p.LocationOfWriting.Longitude));

        }
    }
}