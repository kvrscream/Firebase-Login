using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperLogin.Model;
using SuperLogin.ViewModels;
using SuperLogin.Views;
using Xamarin.Forms;

namespace SuperLogin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public LoginViewModel LoginViewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            this.LoginViewModel = new LoginViewModel();
            this.BindingContext = this.LoginViewModel;
        }

        void Button_Pressed(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Cadastro());
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<LoginViewModel>(this, "entrar", async (msg) =>
            {
                Usuario usuario = new Usuario();
                usuario.Email = this.LoginViewModel.Email;
                usuario.Senha = this.LoginViewModel.Senha;
                bool entrar = await this.LoginViewModel.Logar(usuario: usuario);

                if (entrar)
                {
                    await Navigation.PushAsync(new Views.Home());
                } else
                {
                    await DisplayAlert("OPS", "Não foi possível entrar", "Ok");
                }
            });

        }
    }
}
