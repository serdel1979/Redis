using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.DTO;
using Redis.Model;
using Redis.Repository;
using StackExchange.Redis;
using System.Buffers.Text;
using System.Text;

namespace Redis.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IRepository<Worker> _repository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly ConnectionMultiplexer _redisConnection;
        public WorkerService(IRepository<Worker> repository, IMapper mapper, 
            IDistributedCache distributedCache,
            ConnectionMultiplexer redisConnection)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._distributedCache = distributedCache;
            this._redisConnection = redisConnection;
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
            var redisDatabase = _redisConnection.GetDatabase();

            var redisWorkers = await redisDatabase.StringGetAsync(cacheKey);
            //var redisWorkers = await _distributedCache.GetAsync(cacheKey);

            if (!redisWorkers.IsNull)
            {
                serializedWorkers = Encoding.UTF8.GetString(redisWorkers);
                listWorkers = JsonConvert.DeserializeObject<List<Worker>>(serializedWorkers);
            }
            else
            {
                listWorkers = await _repository.GetAllFilter().ToListAsync();
                var workersDTO = _mapper.Map<IEnumerable<WorkerDTO>>(listWorkers);


                serializedWorkers = JsonConvert.SerializeObject(workersDTO);
              //  redisWorkers = Encoding.UTF8.GetBytes(serializedWorkers);
                await redisDatabase.StringSetAsync(cacheKey, serializedWorkers);


                //var options = new DistributedCacheEntryOptions()
                //                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                //                    .SetSlidingExpiration(TimeSpan.FromSeconds(2));
                //redisDatabase.StringSet(cacheKey, redisWorkers);
               // await _distributedCache.SetAsync(cacheKey,redisWorkers, options);
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
