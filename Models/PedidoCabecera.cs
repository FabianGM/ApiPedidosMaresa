namespace PedidosAPI.Models
{
    public class PedidoCabecera
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Usuario { get; set; }
    }
}