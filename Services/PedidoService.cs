using Microsoft.EntityFrameworkCore;
using PedidosAPI.DTOs;
using PedidosAPI.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PedidosAPI.Data;

namespace PedidosAPI.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _http;
        private readonly ILogger<PedidoService> _logger;

        public PedidoService(
            AppDbContext context,
            IHttpClientFactory http,
            ILogger<PedidoService> logger)
        {
            _context = context;
            _http = http;
            _logger = logger;
        }

        public async Task<int> CrearPedido(PedidoRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Inicio registro pedido");

                // Validación externa
                var client = _http.CreateClient();
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/users/1");

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error validando cliente");

                var total = request.Items.Sum(x => x.Cantidad * x.Precio);

                var cabecera = new PedidoCabecera
                {
                    ClienteId = request.ClienteId,
                    Fecha = DateTime.Now,
                    Total = total,
                    Usuario = request.Usuario
                };

                _context.PedidoCabeceras.Add(cabecera);
                await _context.SaveChangesAsync();

                foreach (var item in request.Items)
                {
                    var detalle = new PedidoDetalle
                    {
                        PedidoId = cabecera.Id,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        Precio = item.Precio
                    };

                    _context.PedidoDetalles.Add(detalle);
                }

                await _context.SaveChangesAsync();

                var log = new LogAuditoria
                {
                    Fecha = DateTime.Now,
                    Evento = "CREAR_PEDIDO",
                    Descripcion = $"Pedido {cabecera.Id} creado"
                };

                _context.LogAuditorias.Add(log);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Pedido creado correctamente");

                return cabecera.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex, "Error creando pedido");

                throw;
            }
        }
    }
}