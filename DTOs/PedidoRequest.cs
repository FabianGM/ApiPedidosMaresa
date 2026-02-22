using System.Collections.Generic;

namespace PedidosAPI.DTOs
{
    public class PedidoRequest
    {
        public int ClienteId { get; set; }
        public string Usuario { get; set; }
        public List<ItemRequest> Items { get; set; }
    }

    public class ItemRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}