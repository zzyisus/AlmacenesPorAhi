using System;
using System.Collections.Generic;
using System.Text;
using AlmacenesPorAhi.Models;

namespace AlmacenesPorAhi.Services
{
    public class DevolucionService
    {
        private static DevolucionService _instance;
        public static DevolucionService Instance => _instance ??= new DevolucionService();

        private readonly List<Devolucion> _devoluciones = new();
        private int _nextId = 1;

        private DevolucionService() { }

        public void RegistrarDevolucion(
            Producto producto,
            int cantidad,
            string motivo,
            TipoReembolso tipoReembolso,
            string empleado)
        {
            var devolucion = new Devolucion
            {
                Id = _nextId++,
                IdProducto = producto.Id,
                NombreProducto = producto.Nombre,
                Cantidad = cantidad,
                Motivo = motivo ?? string.Empty,
                Fecha = DateTime.Now,
                TipoReembolso = tipoReembolso,
                MontoReembolso = producto.Precio * cantidad,
                Empleado = empleado,
                Estado = "Procesada"
            };

            _devoluciones.Add(devolucion);
        }

        public IReadOnlyList<Devolucion> ObtenerDevoluciones()
            => _devoluciones.AsReadOnly();

        public IReadOnlyList<Devolucion> ObtenerDevolucionesPorProducto(int idProducto)
            => _devoluciones.Where(d => d.IdProducto == idProducto).ToList();

        public decimal TotalReembolsado => _devoluciones.Sum(d => d.MontoReembolso);
        public int TotalDevoluciones => _devoluciones.Count;
    }
}
