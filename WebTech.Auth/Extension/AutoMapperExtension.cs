using AutoMapper;

namespace WebTech.Auth.Extension;

public static class AutoMapperExtension
{
    public static void AddAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new UserMappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}