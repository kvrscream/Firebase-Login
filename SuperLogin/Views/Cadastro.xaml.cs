using System;
using System.Collections.Generic;
using SuperLogin.Model;
using SuperLogin.ViewModels;
using Xamarin.Forms;

namespace SuperLogin.Views
{
    public partial class Cadastro : ContentPage
    {
        public CadastroViewModel CadastroViewModel { get; set; }
        public Cadastro()
        {
            InitializeComponent();
            this.Title = "Cadastro";
            CadastroViewModel = new CadastroViewModel();
            this.BindingContext = this.CadastroViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<CadastroViewModel>(this, "Cadastrar", async (msg) =>
            {
                Usuario usuario = new Usuario();
                usuario.Email = CadastroViewModel.Email;
                usuario.Senha = CadastroViewModel.Senha;
                usuario.ConfirmarSenha = CadastroViewModel.ConfirmaSenha;

                bool response = await this.CadastroViewModel.CreateUser(usuario: usuario);

                if(response == false)
                {
                    await DisplayAlert("Ops", "Não foi possível cadastrar o usuário!", "OK");
                } else
                {
                    await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");

                    await Navigation.PopAsync();

                }

            });
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<Usuario>(this, "Cadastrar");
        }

    }
}
