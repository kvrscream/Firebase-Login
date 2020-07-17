using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using SuperLogin.Model;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Text;

namespace SuperLogin.ViewModels
{
    public class CadastroViewModel : BaseViewModel
    {
        public Usuario UsuarioModel { get; set; }
        public string Email
        {
            get
            {
                return UsuarioModel.Email;
            }
            set
            {
                UsuarioModel.Email = value;
            }
        }


        public string Senha
        {
            get
            {
                return UsuarioModel.Senha;
            }
            set
            {
                UsuarioModel.Senha = value;
            }
        }

        string _confima;
        public string ConfirmaSenha
        {
            get
            {
                return _confima;
            }
            set
            {
                _confima = value;
            }
        }

        public ICommand CadastrarCommand { get; set; }


        public CadastroViewModel()
        {
            UsuarioModel = new Usuario();
            CadastrarCommand = new Command(() =>
            {
                MessagingCenter.Send(this, "Cadastrar");
            });
            //    () =>
            //    {
            //        return !string.IsNullOrEmpty(_confima) && !_confima.Equals(Senha);
            //    }
            //);
        }


        public async Task<bool> CreateUser(Usuario usuario)
        {
            bool complete = false;

            try
            {
                using(HttpClient client = new HttpClient())
                {
                    string usuarioJson = JsonConvert.SerializeObject(new {
                        email = usuario.Email,
                        senha = usuario.Senha,
                        confirmaSenha = usuario.ConfirmarSenha
                    });

                    StringContent content = new StringContent(usuarioJson, encoding: Encoding.UTF8, "application/json");

                    HttpResponseMessage clientRequest = await client.PostAsync("http://10.0.2.2:4004/api/createUser",content);
                    if (clientRequest.IsSuccessStatusCode)
                    {
                        string response = await clientRequest.Content.ReadAsStringAsync();
                        Response responseModel = JsonConvert.DeserializeObject<Response>(response);

                        if(responseModel.Status == 200)
                        {
                            complete = true;
                        }
                    }
                }
            } catch(Exception ex) { }


            return complete;
        }
    }
}
