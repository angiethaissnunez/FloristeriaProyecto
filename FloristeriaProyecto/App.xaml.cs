
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
           MainPage = new Views.PageLogin();

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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
