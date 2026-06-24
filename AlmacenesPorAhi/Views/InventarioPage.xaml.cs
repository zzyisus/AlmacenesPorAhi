using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class InventarioPage : ContentPage
    {
        readonly InventarioService _inventario = InventarioService.Instance;
        string _categoriaActual = null;
        string _busquedaActual = null;

        public InventarioPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarCategorias();
            RefrescarLista();
        }

        void CargarCategorias()
        {
            stackCategorias.Children.Clear();

            var btnTodas = CrearBotonCategoria("Todas", null);
            stackCategorias.Children.Add(btnTodas);

            foreach (var cat in _inventario.ObtenerCategorias())
            {
                stackCategorias.Children.Add(CrearBotonCategoria(cat, cat));
            }
        }

        Button CrearBotonCategoria(string texto, string valor)
        {
            bool seleccionada = _categoriaActual == valor;
            var btn = new Button
            {
                Text = texto,
                CornerRadius = 20,
                HeightRequest = 34,
                FontSize = 13,
                Padding = new Thickness(14, 0),
                BackgroundColor = seleccionada ? Color.FromArgb("#1E3A5F") : Color.FromArgb("#F1F5F9"),
                TextColor = seleccionada ? Colors.White : Color.FromArgb("#475569"),
            };
            btn.Clicked += (s, e) =>
            {
                _categoriaActual = valor;
                CargarCategorias();
                RefrescarLista();
            };
            return btn;
        }

        void RefrescarLista()
        {
            listaProductos.ItemsSource = _inventario.ObtenerProductos(_busquedaActual, _categoriaActual);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            _busquedaActual = e.NewTextValue;
            RefrescarLista();
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarProductoPage());
        }

        private async void ListaProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection?.Count > 0)
            {
                var producto = e.CurrentSelection[0] as Producto;
                ((CollectionView)sender).SelectedItem = null;
                if (producto != null)
                    await Navigation.PushAsync(new DetalleProductoPage(producto));
            }
        }
    }
}
