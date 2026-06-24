using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class AgregarProductoPage : ContentPage
    {
        readonly InventarioService _inventario = InventarioService.Instance;

        public AgregarProductoPage()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Error", "El nombre del producto es obligatorio.", "OK");
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
            {
                await DisplayAlert("Error", "Ingresa un precio válido.", "OK");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                await DisplayAlert("Error", "Ingresa un stock válido.", "OK");
                return;
            }

            var producto = new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Description = txtDescripcion.Text?.Trim(),
                Talla = txtTalla.Text?.Trim() ?? "-",
                Color = txtColor.Text?.Trim() ?? "-",
                Precio = precio,
                Stock = stock,
                Categoria = txtCategoria.Text?.Trim() ?? "General",
                ImageUrl = "default_product.png"
            };

            _inventario.AgregarProducto(producto);

            await DisplayAlert("✅ Producto guardado",
                $"{producto.Nombre} fue agregado al inventario.", "Aceptar");

            await Navigation.PopAsync();
        }
    }
}
