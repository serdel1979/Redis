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
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO<bool> { Success = false, Message = "Ocurrió un error interno" });
            }
        }


        [HttpGet("Id:int")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                ResponseDTO<WorkerDTO> response = new ResponseDTO<WorkerDTO>()
                {
                    Success = true,
                    Data = await _workerService.GetById(Id),
                    Message = "Datos del trabajador"
                };
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {

                return StatusCode(500, new ResponseDTO<bool> { Success = false, Message = $"No existe el id {Id}" });
            }
            catch (Exception)
            {

                return StatusCode(500, new ResponseDTO<bool> { Success = false, Message = "Ocurrió un error interno" });
            }
        }



        [HttpPost]
        public async Task<IActionResult> New(WorkerDTO worker)
        {
            try
            {
                ResponseDTO<WorkerDTO> response = new ResponseDTO<WorkerDTO>();

                await _workerService.New(worker);

                response.Success = true;
                response.Data = worker;
                response.Message = "Creado";

                return Ok(response);

            }
            catch (Exception)
            {

                return StatusCode(500, new ResponseDTO<bool> { Success = false, Message = "Ocurrió un error interno" });
            }
        }


        [HttpPut("Id:int")]
        public async Task<IActionResult> Update(WorkerDTO worker, int Id)
        {
            try
            {
                ResponseDTO<bool> response = new ResponseDTO<bool>();
                await _workerService.Update(worker, Id);
                response.Success = true;
                response.Data = true;
                response.Message = "Actualizado";
                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404, new ResponseDTO<bool> { Success = false, Message = $"No existe el id {Id}" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ResponseDTO<bool> { Success = false, Message = "Error interno" });
            }
        }


        [HttpDelete("Id:int")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _workerService.Delete(Id);
                ResponseDTO<bool> response = new ResponseDTO<bool>();
                response.Success = true;
                response.Data = true;
                response.Message = $"Eliminado id {Id}";

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return StatusCode(404, new ResponseDTO<bool> { Success = false, Message = $"No existe el id {Id}" });
            }
            catch (Exception)
            {

                return StatusCode(500, new ResponseDTO<bool> { Success = false, Message = "Error interno" });
            }
        }

    }
}
