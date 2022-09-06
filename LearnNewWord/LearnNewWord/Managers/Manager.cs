using AutoMapper;
using Common.Dto;
using LearnNewWord.Model;
using Prism.Ioc;

namespace LearnNewWord.Managers
{
    public static class Manager
    {
        private static IContainerProvider _container;

        public static void Configuration(IContainerProvider container)
        {
            _container = container;
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static MapperConfiguration RegisterAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WordDto, WordModel>()
                .ForMember(dest => dest.IsNoun, opt => opt.MapFrom(scr => scr.Noun != 0))
                .ForMember(dest => dest.IsAjective, opt => opt.MapFrom(scr => scr.Ajective != 0))
                .ForMember(dest => dest.IsVerb, opt => opt.MapFrom(scr => scr.Verb != 0))
                .ForMember(dest => dest.IsAdverb, opt => opt.MapFrom(scr => scr.Adverb != 0))
                .ForMember(dest => dest.IsKnown, opt => opt.MapFrom(scr => scr.Known != 0));

                cfg.CreateMap<WordModel, WordDto>()
                .ForMember(dest => dest.Noun, opt => opt.MapFrom(scr => scr.IsNoun ? 1 : 0))
                .ForMember(dest => dest.Ajective, opt => opt.MapFrom(scr => scr.IsAjective ? 1 : 0))
                .ForMember(dest => dest.Verb, opt => opt.MapFrom(scr => scr.IsVerb ? 1 : 0))
                .ForMember(dest => dest.Adverb, opt => opt.MapFrom(scr => scr.IsAdverb ? 1 : 0))
                .ForMember(dest => dest.Known, opt => opt.MapFrom(scr => scr.IsKnown ? 1 : 0));

                cfg.CreateMap<CategoryDto, CategoryModel>().ReverseMap();
            });

            return config;
        }
    }
}