using AutoMapper;

namespace Testing.Mocking
{
    abstract class BaseMocking<T, TProfile> where TProfile : Profile, new()
    {
        public abstract T Setup();

        protected IMapper SetupMapper()
        {
            var config = new MapperConfiguration(c => c.AddProfile<TProfile>());
            return config.CreateMapper();
        }
    }
}
