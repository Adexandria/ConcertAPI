using Concert.Application.DTO;
using Concert.Domain.Entities;
using Mapster;


namespace Concert.Infrastructure.Service
{
    public class MappingConfig
    {
        public TypeAdapterConfig UserModelConfig()
        {
            var config = TypeAdapterConfig<SignUp, UserModel>.NewConfig()
                .Map(dest => dest.PasswordHash, src => src.Password);
            return config.Config;
        }
        
    }
}
