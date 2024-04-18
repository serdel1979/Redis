using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.DTO;
using Redis.Model;
using Redis.Repository;
using System.Buffers.Text;
using System.Text;

namespace Redis.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IRepository<Worker> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public WorkerService(IRepository<Worker> repository, IMapper mapper, IDistributedCache distributedCache)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCache = distributedCache;
        }
        public async Task Delete(int Id)
        {
            var entity = await _repository.GetAllFilter(w => w.Id == Id).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new KeyNotFoundException($"No existe usuario con id {Id}");
            }
            await _repository.Delete(entity);
            await _repository.SaveAll();
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
            var cacheKey = "listWorkers";
            string serializedWorkers;
            var listWorkers = new List<Worker>();

            var redisWorkers = await _distributedCache.GetAsync(cacheKey);

            if (redisWorkers is not null)
            {
                serializedWorkers = Encoding.UTF8.GetString(redisWorkers);
                listWorkers = JsonConvert.DeserializeObject<List<Worker>>(serializedWorkers);
            }
            else
            {
                listWorkers = await _repository.GetAllFilter().ToListAsync();
                serializedWorkers = JsonConvert.SerializeObject(listWorkers);
                redisWorkers = Encoding.UTF8.GetBytes(serializedWorkers);

                var options = new DistributedCacheEntryOptions()
                                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                                    .SetSlidingExpiration(TimeSpan.FromSeconds(2));

                await _distributedCache.SetAsync(cacheKey,redisWorkers, options);
            }


            return _mapper.Map<IEnumerable<WorkerDTO>>(listWorkers);
        }

        public async Task New(WorkerDTO workerDTO)
        {
            await _repository.Create(_mapper.Map<Worker>(workerDTO));
            await _repository.SaveAll();
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
            await _repository.SaveAll();
        }
    }
}
