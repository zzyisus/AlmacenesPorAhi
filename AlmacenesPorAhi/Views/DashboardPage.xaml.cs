using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class DashboardPage : ContentPage
    {
        readonly AuthService _auth = AuthService.Instance;
        readonly InventarioService _inventario = InventarioService.Instance;

        public DashboardPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ActualizarDatos();
        }

        void ActualizarDatos()
        {
            var usuario = _auth.UsuarioActual;
            lblBienvenida.Text = $"¡Hola, {usuario?.NombreCompleto ?? usuario?.UsuarioNombre}!";
            lblRol.Text = $"Rol: {usuario?.Rol}";
            lblFecha.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");

            lblTotalProductos.Text = _inventario.TotalProductos.ToString();
            lblStockBajo.Text = _inventario.ProductosStockBajo.ToString();
            lblSinStock.Text = _inventario.ProductosSinStock.ToString();
        }

        private async void BtnInventario_Clicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new InventarioPage());

        private async void BtnDevoluciones_Clicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new DevolucionPage());

        private async void BtnHistorial_Clicked(object sender, EventArgs e)
            => await Navigation.PushAsync(new HistorialDevolucionesPage());

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert("Cerrar sesión",
                "¿Deseas cerrar la sesión?", "Sí", "Cancelar");
            if (confirmar)
            {
                _auth.Logout();
                Navigation.InsertPageBefore(new LoginPage(), this);
                await Navigation.PopAsync();
            }
        }
    }
}
