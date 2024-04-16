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

        public async Task<IQueryable<WorkerDTO>> GetFilter(string find)
        {
            var entities = _repository.GetAllFilter(w => w.Name.ToLower().Contains(find.ToLower()));

            return _mapper.Map<IQueryable<WorkerDTO>>(entities);
         
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
