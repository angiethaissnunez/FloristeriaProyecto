using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using System.Collections.ObjectModel;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMapa : ContentPage
    {
        Ubicacion ubicacion = null;

        public ObservableCollection<Bolsa> oListaGlobalBolsa = new ObservableCollection<Bolsa>();

        public PageMapa(Ubicacion ubicacion)
        {
            InitializeComponent();
            getLatitudeAndLongitude();
            this.ubicacion = ubicacion;
        }
        /*  protected override void OnAppearing()
          {
              base.OnAppearing();
              getLatitudeAndLongitude();
          }*/

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status == PermissionStatus.Granted)
                {
                    var localizacion = await Geolocation.GetLocationAsync();

                    if (localizacion != null)
                    {
                        var pin = new Pin()
                        {
                            Type = PinType.SearchResult,
                            Position = new Position(ubicacion.latitud, ubicacion.longitud),
                          //  Label = "Descripcion",
                           // Address = Sitio.Description
                        };

                        mapa.Pins.Add(pin);
                        //mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(localizacion.Latitude, localizacion.Longitude), Distance.FromMeters(100)));
                        mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(ubicacion.latitud, ubicacion.longitud), Distance.FromMeters(100)));
                    }
                }
                else
                {
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("Location services are not enabled on device."))
                {
                    Message("Error", "Servicio de localizacion no encendido");
                }
                else
                {
                    Message("Error", e.Message);
                }
            }
        }

      
        private async void getLatitudeAndLongitude()
        {
            try
            {

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var localizacion = await Geolocation.GetLocationAsync();
                    txtLatitude.Text = Math.Round(localizacion.Latitude, 5) + "";
                    txtLongitude.Text = Math.Round(localizacion.Longitude, 5) + "";
                }
                else
                {
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception e)
            {

                if (e.Message.Equals("Location services are not enabled on device."))
                {

                    Message("Error", "Servicio de localizacion no encendido");
                }
                else
                {
                    Message("Error", e.Message);
                }

            }
        }

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                Message("Advertencia", "Actualmente no cuenta con acceso a internet");
                return;
            }

            if (string.IsNullOrEmpty(txtLatitude.Text) || string.IsNullOrEmpty(txtLongitude.Text))
            {
                Message("Aviso", "Aun no se obtiene la ubicacion");
                getLatitudeAndLongitude();
                return;
            }


            Ubicacion oUbicacion = new Ubicacion()
            {
                latitud = double.Parse(txtLatitude.Text),
                longitud = double.Parse(txtLongitude.Text)
            };

            Compra oCompra = new Compra()
            {
                fechaCompra = DateTime.Now.ToString("dd/MM/yyyy"),
                tipoEntrega = "ubicacion",
                oListaBolsa = oListaGlobalBolsa,
                oUbicacion = oUbicacion,
                oTienda = null,
                oDepacho = null
            };

            await Navigation.PushAsync(new PagePago(oCompra));

           /* try
            {


                var sitio = new Ubicacion()
                {
                    latitud = double.Parse(txtLatitude.Text),
                    longitud = double.Parse(txtLongitude.Text)
                };

                var result = await ApiServiceFirebase.RegistrarCompra(sitio);

              

                if (result)
                {
                    Message("Aviso", "Sitio agregado correctamente");
                  //  clearComp();
                }
                else
                {
                    Message("Aviso", "No se pudo agregar el sitio");
                }

            }
            catch (Exception ex)
            {
              //  UserDialogs.Instance.HideLoading();

                await Task.Delay(500);

                Message("Error: ", ex.Message);
            }*/

        }
    }
}