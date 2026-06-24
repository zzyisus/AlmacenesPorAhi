using System;
using System.Collections.Generic;
using System.Text;

namespace AlmacenesPorAhi.Models
{
    public enum TipoReembolso
    {
        Credito,
        Reembolso,
        CambioProducto
    }

    public class Devolucion
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
        public TipoReembolso TipoReembolso { get; set; }
        public decimal MontoReembolso { get; set; }
        public string Empleado { get; set; }
        public string Estado { get; set; } = "Procesada";
    }
}
