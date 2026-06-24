using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class DevolucionPage : ContentPage
    {
        readonly InventarioService _inventario = InventarioService.Instance;
        readonly DevolucionService _devoluciones = DevolucionService.Instance;
        readonly AuthService _auth = AuthService.Instance;

        Producto _productoSeleccionado;

        public DevolucionPage()
        {
            InitializeComponent();
            lblFecha.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy — HH:mm");
        }

        private void TxtProductoId_TextChanged(object sender, TextChangedEventArgs e)
        {
            frameProductoInfo.IsVisible = false;
            frameProductoError.IsVisible = false;
            _productoSeleccionado = null;
            ActualizarMontoEstimado();

            if (int.TryParse(e.NewTextValue, out int id))
            {
                var producto = _inventario.ObtenerPorId(id);
                if (producto != null)
                {
                    _productoSeleccionado = producto;
                    lblProductoNombre.Text = $"📦 {producto.Nombre}  ({producto.Color} / Talla {producto.Talla})";
                    lblProductoStock.Text = $"Stock actual: {producto.Stock} unidades";
                    lblProductoPrecio.Text = $"Precio unitario: ${producto.Precio:N0}";
                    frameProductoInfo.IsVisible = true;
                    ActualizarMontoEstimado();
                }
                else if (!string.IsNullOrEmpty(e.NewTextValue))
                {
                    frameProductoError.IsVisible = true;
                }
            }
        }

        private void ActualizarMontoEstimado()
        {
            if (_productoSeleccionado == null ||
                !int.TryParse(txtCantidad?.Text, out int cant) || cant <= 0)
            {
                lblMontoEstimado.Text = "Monto estimado: $0";
                return;
            }
            lblMontoEstimado.Text = $"Monto estimado: ${_productoSeleccionado.Precio * cant:N0}";
        }

        private async void Registrar_Clicked(object sender, EventArgs e)
        {
            // Validaciones
            if (_productoSeleccionado == null)
            {
                await DisplayAlert("Error", "Ingresa un ID de producto válido.", "OK");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                await DisplayAlert("Error", "Ingresa una cantidad válida mayor a 0.", "OK");
                return;
            }

            if (cantidad > _productoSeleccionado.Stock)
            {
                await DisplayAlert("Error",
                    $"No puedes devolver más unidades de las que hay en stock ({_productoSeleccionado.Stock}).", "OK");
                return;
            }

            if (pickerTipoReembolso.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Selecciona un tipo de resolución.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                await DisplayAlert("Error", "Ingresa el motivo de la devolución.", "OK");
                return;
            }

            var tipoReembolso = (TipoReembolso)pickerTipoReembolso.SelectedIndex;

            // Registrar devolución y ajustar inventario
            _devoluciones.RegistrarDevolucion(
                _productoSeleccionado,
                cantidad,
                txtMotivo.Text,
                tipoReembolso,
                _auth.UsuarioActual?.NombreCompleto ?? "Sistema");

            _inventario.AjustarStock(_productoSeleccionado.Id, cantidad);

            string tipoTexto = pickerTipoReembolso.Items[pickerTipoReembolso.SelectedIndex];
            decimal monto = _productoSeleccionado.Precio * cantidad;

            await DisplayAlert("✅ Devolución registrada",
                $"Producto: {_productoSeleccionado.Nombre}\n" +
                $"Cantidad: {cantidad} uds\n" +
                $"Resolución: {tipoTexto}\n" +
                $"Monto: ${monto:N0}\n\n" +
                $"El stock ha sido actualizado.", "Aceptar");

            LimpiarFormulario();
        }

        void LimpiarFormulario()
        {
            txtProductoId.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtMotivo.Text = string.Empty;
            pickerTipoReembolso.SelectedIndex = -1;
            frameProductoInfo.IsVisible = false;
            frameProductoError.IsVisible = false;
            _productoSeleccionado = null;
            lblMontoEstimado.Text = "Monto estimado: $0";
        }

        private async void BtnHistorial_Clicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new HistorialDevolucionesPage());
    }
}
