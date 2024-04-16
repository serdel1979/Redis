using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Redis.DTO;
using Redis.Model;
using Redis.Repository;

namespace Redis.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IRepository<Worker> _repository;
        private readonly IMapper _mapper;

        public WorkerService(IRepository<Worker> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public async Task Delete(int Id)
        {
            var entity = await _repository.GetAllFilter(w => w.Id == Id).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new KeyNotFoundException($"No existe usuario con id {Id}");
            }
            await _repository.Delete(entity);
        }

        public async Task<WorkerDTO> GetById(int Id)
        {
            var entity = await _repository.GetAllFilter(w => w.Id == Id).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new KeyNotFoundException($"No existe usuario con id {Id}");
            }
            return _mapper.Map<WorkerDTO>(entity);
        }

        public async Task<IEnumerable<WorkerDTO>> FindFilter(string find)
        {
            var entities = await _repository.GetAllFilter(w => w.Name.ToLower().Contains(find.ToLower())).ToListAsync();

            return entities.Select(w=> _mapper.Map<WorkerDTO>(w));
        }


        public async Task<IEnumerable<WorkerDTO>> GetAll()
        {
            var entities = await _repository.GetAllFilter().ToListAsync();

            return entities.Select(w => _mapper.Map<WorkerDTO>(w));
        }

        public async Task New(WorkerDTO workerDTO)
        {
            await _repository.Create(_mapper.Map<Worker>(workerDTO));
        }

        public async Task Update(WorkerDTO workerDTO, int Id)
        {
            var entity = await _repository.GetAllFilter(w => w.Id == Id).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new KeyNotFoundException($"No existe usuario con id {Id}");
            }
            
            entity.Name = workerDTO.Name;
            entity.Description = workerDTO.Description;
            await _repository.Update(entity);
        }
    }
}
