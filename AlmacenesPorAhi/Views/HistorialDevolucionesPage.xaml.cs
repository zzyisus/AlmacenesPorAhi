using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class HistorialDevolucionesPage : ContentPage
    {
        readonly DevolucionService _devoluciones = DevolucionService.Instance;

        public HistorialDevolucionesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var historial = _devoluciones.ObtenerDevoluciones()
                .OrderByDescending(d => d.Fecha)
                .ToList();

            listaHistorial.ItemsSource = historial;
            lblTotalDevoluciones.Text = _devoluciones.TotalDevoluciones.ToString();
            lblTotalReembolsado.Text = $"${_devoluciones.TotalReembolsado:N0}";
        }
    }
}
