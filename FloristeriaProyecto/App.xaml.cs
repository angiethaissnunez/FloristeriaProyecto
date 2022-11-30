
using FloristeriaProyecto.Views;
using Rating;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Plugin.FirebasePushNotification;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace FloristeriaProyecto
{
    public partial class App : Application
    {

        public App()
        {

           /* Device.SetFlags(new string[] {
                "AppTheme_Experimental",
                "MediaElement_Experimental"
                });*/


            InitializeComponent();
         //  MainPage = new Views.PageLogin();



          /*  if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")) && Preferences.Get("Level", "") == "C")
            {
                MainPage = new NavigationPage(new PageInicio());
            }
            else if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")) && Preferences.Get("Level", "") == "D")
            {
                MainPage = new NavigationPage(new DeliveryPage());
            }
            else if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", "")) && Preferences.Get("Level", "") == "A")
            {
                MainPage = new NavigationPage(new AdminPage());
            }*/
         /*   else
            {
                MainPage = new NavigationPage(new PageLogin());
            }*/




            /* if (!Preferences.ContainsKey("usuarios"))
             {
                 //MainPage = new Views.LoginView();
                 MainPage = new Views.PageInicio();
             }
             else
             {
                 MainPage = new Views.PageInicio();
             }*/
        }

        protected override void OnStart()
        {
            Iniciar();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            Debug.WriteLine("OnResume");

        }

        public async void Iniciar()
        {
            if ((Preferences.Get("Remember", true) == true))
            {
                MainPage = new PageLogin();
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(PageInicio)}");
            }
        }
    }
}
