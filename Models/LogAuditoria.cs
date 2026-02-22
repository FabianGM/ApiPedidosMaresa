using System;

namespace PedidosAPI.Models
{
    public class LogAuditoria
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Evento { get; set; }
        public string Descripcion { get; set; }
    }
}