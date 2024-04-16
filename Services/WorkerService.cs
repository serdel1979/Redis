using Redis.DTO;
using Redis.Model;
using Redis.Repository;

namespace Redis.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IRepository<Worker> _repository;

        public WorkerService(IRepository<Worker> repository)
        {
            this._repository = repository;
        }
        public Task Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<WorkerDTO> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<WorkerDTO>> GetFilter(string find)
        {
            throw new NotImplementedException();
        }

        public Task Update(WorkerDTO workerDTO, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
