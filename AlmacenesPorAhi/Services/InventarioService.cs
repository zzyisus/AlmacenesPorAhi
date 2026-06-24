using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlmacenesPorAhi.Models;

namespace AlmacenesPorAhi.Services
{
    public class InventarioService
    {
        private static InventarioService _instance;
        public static InventarioService Instance => _instance ??= new InventarioService();

        private readonly List<Producto> _productos;
        private int _nextId = 3;

        private InventarioService()
        {
            _productos = new List<Producto>
            {
                new Producto
                {
                    Id = 1, Nombre = "Polera", Talla = "M", Color = "Negro",
                    Precio = 15000, Stock = 10, Categoria = "Ropa",
                    ImageUrl = "polera.png", Description = "Polera cómoda de algodón", StockMinimo = 3
                },
                new Producto
                {
                    Id = 2, Nombre = "Pantalón", Talla = "L", Color = "Azul",
                    Precio = 25000, Stock = 2, Categoria = "Ropa",
                    ImageUrl = "pantalon.png", Description = "Pantalón casual resistente", StockMinimo = 3
                },
                new Producto
                {
                    Id = 3, Nombre = "Zapatilla", Talla = "42", Color = "Blanco",
                    Precio = 45000, Stock = 2, Categoria = "Calzado",
                    ImageUrl = "zapatilla.png", Description = "Zapatilla deportiva", StockMinimo = 2
                }
            };
        }

        public List<Producto> ObtenerProductos(string filtro = null, string categoria = null)
        {
            var query = _productos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.ToLower();
                query = query.Where(p =>
                    p.Nombre.ToLower().Contains(filtro) ||
                    p.Color.ToLower().Contains(filtro) ||
                    p.Talla.ToLower().Contains(filtro));
            }

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p => p.Categoria == categoria);

            return query.ToList();
        }

        public Producto ObtenerPorId(int id)
            => _productos.FirstOrDefault(p => p.Id == id);

        public void AgregarProducto(Producto producto)
        {
            producto.Id = _nextId++;
            _productos.Add(producto);
        }

        public void ActualizarProducto(Producto producto)
        {
            var idx = _productos.FindIndex(p => p.Id == producto.Id);
            if (idx >= 0) _productos[idx] = producto;
        }

        public void EliminarProducto(int id)
            => _productos.RemoveAll(p => p.Id == id);

        public void AjustarStock(int id, int delta)
        {
            var p = ObtenerPorId(id);
            if (p != null)
            {
                p.Stock = Math.Max(0, p.Stock + delta);
            }
        }

        public List<string> ObtenerCategorias()
            => _productos.Select(p => p.Categoria).Distinct().OrderBy(c => c).ToList();

        public int TotalProductos => _productos.Count;
        public int ProductosSinStock => _productos.Count(p => p.Stock == 0);
        public int ProductosStockBajo => _productos.Count(p => p.StockBajo && p.Stock > 0);
    }
}
