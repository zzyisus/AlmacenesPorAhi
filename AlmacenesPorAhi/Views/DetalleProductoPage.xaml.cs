using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class DetalleProductoPage : ContentPage
    {
        readonly InventarioService _inventario = InventarioService.Instance;
        Producto _producto;

        public DetalleProductoPage(Producto producto)
        {
            InitializeComponent();
            _producto = producto;
            CargarDatos();
        }

        void CargarDatos()
        {
            lblNombre.Text = _producto.Nombre;
            lblDescripcion.Text = _producto.Description;
            lblPrecio.Text = $"${_producto.Precio:N0}";
            lblStock.Text = $"{_producto.Stock} uds";
            lblStock.TextColor = Color.FromArgb(_producto.StockColor);
            lblTalla.Text = _producto.Talla;
            lblColor.Text = _producto.Color;
            lblCategoria.Text = _producto.Categoria ?? "—";
            lblEstado.Text = _producto.StockEstado;
            lblEstado.TextColor = Color.FromArgb(_producto.StockColor);
        }

        private async void BtnRegistrarDevolucion_Clicked(object sender, EventArgs e)
        {
            var page = new DevolucionPage();
            await Navigation.PushAsync(page);
        }

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Eliminar producto",
                $"¿Eliminar {_producto.Nombre} del inventario? Esta acción no se puede deshacer.",
                "Eliminar", "Cancelar");

            if (confirmar)
            {
                _inventario.EliminarProducto(_producto.Id);
                await Navigation.PopAsync();
            }
        }
    }
}
