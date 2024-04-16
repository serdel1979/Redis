using Redis.DTO;

namespace Redis.Services
{
    public interface IWorkerService
    {
        Task<WorkerDTO> GetById(int Id);
        Task<IEnumerable<WorkerDTO>> FindFilter(string? find);
        Task<IEnumerable<WorkerDTO>> GetAll();
        Task Delete(int Id);
        Task Update(WorkerDTO workerDTO, int Id);
        Task New(WorkerDTO workerDTO);
    }
}
