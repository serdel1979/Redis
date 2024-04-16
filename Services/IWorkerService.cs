using Redis.DTO;

namespace Redis.Services
{
    public interface IWorkerService
    {
        Task<WorkerDTO> GetById(int Id);
        Task<IQueryable<WorkerDTO>> GetFilter(string find);

        Task Delete(int Id);
        Task Update(WorkerDTO workerDTO, int Id);
        Task New(WorkerDTO workerDTO);
    }
}
