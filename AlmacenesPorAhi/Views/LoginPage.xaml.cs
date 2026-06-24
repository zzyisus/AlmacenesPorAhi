using AlmacenesPorAhi.Services;

namespace AlmacenesPorAhi.Views
{
    public partial class LoginPage : ContentPage
    {
        readonly AuthService _auth = AuthService.Instance;

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void BtnIngresar_Clicked(object sender, EventArgs e)
        {
            lblError.IsVisible = false;

            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Por favor ingresa usuario y contraseña.";
                lblError.IsVisible = true;
                return;
            }

            btnIngresar.IsEnabled = false;
            btnIngresar.Text = "Ingresando...";

            // Pequeño retraso en la retroalimentación de UX
            await Task.Delay(300);

            bool acceso = _auth.Login(txtUsuario.Text.Trim(), txtPassword.Text);

            if (acceso)
            {
                await Navigation.PushAsync(new DashboardPage());
            }
            else
            {
                lblError.Text = "Usuario o contraseña incorrectos.";
                lblError.IsVisible = true;
                txtPassword.Text = string.Empty;
                btnIngresar.IsEnabled = true;
                btnIngresar.Text = "Ingresar";
            }
        }
    }
}
