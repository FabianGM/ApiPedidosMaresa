using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosAPI.DTOs;
using PedidosAPI.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PedidosAPI.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidosController(PedidoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPedido([FromBody] PedidoRequest request)
        {
            // Validación de datos
            if (request == null || request.Items == null || request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    error = "El pedido no contiene items válidos"
                });
            }

            try
            {
                var id = await _service.CrearPedido(request);

                return Ok(new
                {
                    mensaje = "Pedido registrado correctamente",
                    pedidoId = id
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new
                {
                    error = "Servicio externo no disponible"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}