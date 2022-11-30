using FloristeriaProyecto.Modelo;
using FloristeriaProyecto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppNotas.Modelo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using System.IO;

namespace FloristeriaProyecto.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRegistro : ContentPage
    {
        byte[] Image;
        MediaFile FileFoto = null;
        
        public PageRegistro()
        {
            InitializeComponent();
        }
        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            Usuario oUsuario = new Usuario()
            {
                Nombres = txtNombre.Text,
                Apellidos = txtApellido.Text,
                Documento = txtDocumento.Text,
                Email = txtEmail.Text,
                Image = Image,
                Clave = txtContrasena.Text
            };

            bool respuesta = await ApiServiceAuthentication.RegistrarUsuario(oUsuario);

            if (respuesta)
            {
                await DisplayAlert("Correcto", "Usuario registrado", "Aceptar");
                await Navigation.PopModalAsync();
               


            }
            else
            {
                await DisplayAlert("Oops", "No se pudo registrar", "Aceptar");
            }



        }

        private void TapBackArrow_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void TapLabelTerminosCondiciones_Tapped(object sender, EventArgs e)
        {
            popupTerminosCondiciones.IsVisible = true;
           
        }

        private void BtnCerrarModal_Clicked(object sender, EventArgs e)
        {
            popupTerminosCondiciones.IsVisible = false;
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {

            bool response = await Application.Current.MainPage.DisplayAlert("Advertencia", "Realizar la opción mendiante: ", "Camara", "Galeria");

            if (response)
                GetImageFromCamera();
            else
                GetImageFromGallery();


        }

        private async void GetImageFromCamera()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    FileFoto = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                        SaveToAlbum = true
                    });

                    if (FileFoto == null)
                        return;

                    Imagen.Source = ImageSource.FromStream(() => { return FileFoto.GetStream(); });
                    Image = File.ReadAllBytes(FileFoto.Path);
                }
                catch (Exception)
                {
                    Message("Advertencia", "Se produjo un error al tomar la fotografia.");
                }
            }
            else
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }


        }

        private async void GetImageFromGallery()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    var FileFoto = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                    });
                    if (FileFoto == null)
                        return;

                    Imagen.Source = ImageSource.FromStream(() => { return FileFoto.GetStream(); });
                    Image = File.ReadAllBytes(FileFoto.Path);
                }
                else
                {
                    Message("Advertencia", "Se produjo un error al seleccionar la imagen");
                }
            }
            catch (Exception)
            {
                Message("Advertencia", "Se produjo un error al seleccionar la imagen");
            }

        }



        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();

                    stream.CopyTo(memory);

                    return memory.ToArray();
                }
            }

            return null;
        }
        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }

    }
}