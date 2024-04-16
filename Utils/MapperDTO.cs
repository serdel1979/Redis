using AutoMapper;
using Redis.DTO;
using Redis.Model;

namespace Redis.Utils
{
    public class MapperDTO : Profile
    {
        public MapperDTO()
        {
            CreateMap<Worker, WorkerDTO>().ReverseMap();
        }
    }
}
