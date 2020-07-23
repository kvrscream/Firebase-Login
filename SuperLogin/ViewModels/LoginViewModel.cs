using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using SuperLogin.Model;
using Xamarin.Forms;

namespace SuperLogin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        string _senha;
        public string Senha
        {
            get
            {
                return _senha;
            }
            set
            {
                _senha = value;
            }
        }

        public ICommand EntrarCommand { get; set; }

        public LoginViewModel()
        {
            EntrarCommand = new Command(() =>
            {
                MessagingCenter.Send(this, "entrar");
            });
        }


        bool _aguarde;
        public bool Aguarde
        {
            get
            {
                return _aguarde;
            }
            set
            {
                _aguarde = value;
                OnPropertChanged();
                OnPropertChanged(nameof(Aguarde));
            }
        }

        public async Task<bool> Logar(Usuario usuario)
        {
            bool complete = true;
            Aguarde = true;
            try
            {
                string usuarioJson = JsonConvert.SerializeObject(new
                {
                    email = usuario.Email,
                    senha = usuario.Senha
                });

                StringContent content = new StringContent(usuarioJson, encoding: Encoding.UTF8, "application/json");

                using(HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.PostAsync("https://xamarin-api-dev.herokuapp.com/api/login", content);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string message = await responseMessage.Content.ReadAsStringAsync();
                        Response response = JsonConvert.DeserializeObject<Response>(message);
                        if(response.Status == 200)
                        {
                            complete = true;
                            Aguarde = false;
                        }
                    }
                }

            } catch(Exception ex)
            {
                Aguarde = false;
            }

            return complete;
        }
    }





}
