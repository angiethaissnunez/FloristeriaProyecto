using AppNotas.Modelo;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FloristeriaProyecto.Service
{
    public class ApiServiceAuthentication
    {
        public static async Task<bool> Login(UserAuthentication oUsuario)
        {
            try
            {

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(AppSettings.ApiAuthentication("LOGIN"), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    AppSettings.oAuthentication = oResponse;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }



      /*  private async void RegisterMethod()
        {
            if (string.IsNullOrEmpty(this.email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password.",
                    "Accept");
                return;
            }

            string WebAPIkey = "AIzaSyC-O1SLFIfJsBOUi6kAGnXsX8FGhpnWDMA";

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(EmailTxt.ToString(), PasswordTxt.ToString());
                string gettoken = auth.FirebaseToken;
                var content = await auth.GetFreshAuthAsync();

                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                if (content.User.IsEmailVerified == false)
                {
                    var action = await App.Current.MainPage.DisplayAlert("Alerta", "Su correo electrónico no está activado, ¿quiere enviar enlace de Verificación?!", "Yes", "No");

                    if (action)
                    {
                        await authProvider.SendEmailVerificationAsync(gettoken);
                        Application.Current.MainPage = new LoginPage();

                    }

                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }


        }*/

        public static async Task<bool> RegistrarUsuario(Usuario oUsuario)
        {
            bool respuesta = true;
            try
            {
                UserAuthentication oUser = new UserAuthentication()
                {
                    email = oUsuario.Email,
                    password = oUsuario.Clave,
                    returnSecureToken = true
                };

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUser);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(AppSettings.ApiAuthentication("SIGNIN"), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    if(oResponse != null)
                    {
                        respuesta = await ApiServiceFirebase.RegistrarUsuario(oUsuario, oResponse);
                        // string WebAPIkey = "AIzaSyC-O1SLFIfJsBOUi6kAGnXsX8FGhpnWDMA";

                      /*  try
                        {
                            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(AppSettings.KeyAplication)).CreateUserWithEmailAndPasswordAsync(oResponse.Email.ToString(), oUsuario.Clave);
                          //  var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(oResponse.Email.ToString(), oUsuario.Clave);
                            string gettoken = authProvider.Result.FirebaseToken;
                            var contento = await authProvider.Result.GetFreshAuthAsync();

                            var serializedcontnet = JsonConvert.SerializeObject(contento);
                            Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);

                            if (contento.User.IsEmailVerified == false)
                            {
                                var action = await App.Current.MainPage.DisplayAlert("Alerta", "Su correo electrónico no está activado, ¿quiere enviar enlace de Verificación?!", "Yes", "No");

                                if (action)
                                {
                                    await new FirebaseAuthProvider(new FirebaseConfig(AppSettings.KeyAplication)).SendEmailVerificationAsync(gettoken);
                                    Application.Current.MainPage = new PageLogin();

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
                        }*/




                    }
                    else
                    {
                        respuesta = false;
                    }
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                respuesta = false;
            }

            return respuesta;

        }
    }
}
