using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.DTO;
using Redis.Services;

namespace Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            this._workerService = workerService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ResponseDTO<IEnumerable<WorkerDTO>> resp = new ResponseDTO<IEnumerable<WorkerDTO>>()
                {
                    Success = true,
                    Data = await _workerService.GetAll(),
                    Message = "Listado"
                };
                return Ok(resp);
            }
            catch (KeyNotFoundException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
