using AccessFun.Core.Services;
using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.PictureChooser;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessFun.Core.ViewModels
{
    public class CadastrarUsuarioViewModel : MvxViewModel
    {
        private Stream imageStream;
        private readonly IMvxPictureChooserTask _pictureChooserTask;
        private readonly IMvxNavigationService _navigationService;
        private string nomeUsuario;
        private string emailUsuario;
        private string senhaUsuario;
        private string dataNascimentoUsuario;
        private string enderecoUsuario;
        public static string deficienciasUsuario;

        public CadastrarUsuarioViewModel (IMvxPictureChooserTask pictureChooserTask, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _pictureChooserTask = pictureChooserTask;
        }

        public override async Task Initialize ()
        {
            await base.Initialize ();
        }

        public static string DeficienciasUsuario
        {
            get => deficienciasUsuario;
            set
            {
                deficienciasUsuario = value;
            }
        }

        public string NomeUsuario
        {
            get => nomeUsuario;
            set
            {
                nomeUsuario = value;
                RaisePropertyChanged (() => NomeUsuario);
            }
        }

        public string EmailUsuario
        {
            get => emailUsuario;
            set
            {
                emailUsuario = value;
                RaisePropertyChanged (() => EmailUsuario);
            }
        }

        public string DataNascimentoUsuario
        {
            get => dataNascimentoUsuario;
            set
            {
                dataNascimentoUsuario = value;
                RaisePropertyChanged (() => DataNascimentoUsuario);
            }
        }

        public string EnderecoUsuario
        {
            get => enderecoUsuario;
            set
            {
                enderecoUsuario = value;
                RaisePropertyChanged (() => EnderecoUsuario);
            }
        }

        public string SenhaUsuario
        {
            get => senhaUsuario;
            set
            {
                senhaUsuario = value;
                RaisePropertyChanged (() => SenhaUsuario);
            }
        }

        private MvxCommand _cadastrarUsuarioCommand;
        public ICommand CadastrarUsuarioCommand
        {
            get
            {
                _cadastrarUsuarioCommand = _cadastrarUsuarioCommand ?? new MvxCommand (DoCadastrarUsuario);
                return _cadastrarUsuarioCommand;
            }
        }

        private MvxCommand _sairCadastrarUsuarioCommand;
        public ICommand SairCadastrarUsuarioCommand
        {
            get
            {
                _sairCadastrarUsuarioCommand = _sairCadastrarUsuarioCommand ?? new MvxCommand (DoSairCadastrarUsuario);
                return _sairCadastrarUsuarioCommand;
            }
        }

        private MvxCommand _takePictureCommand;
        public ICommand TakePictureCommand
        {
            get
            {
                _takePictureCommand = _takePictureCommand ?? new MvxCommand (DoTakePicture);
                return _takePictureCommand;
            }
        }

        private async void DoTakePicture ()
        {
            Permission[] permissions = new Permission[] { Permission.Camera, Permission.Storage };

            try
            {
                await CrossPermissions.Current.RequestPermissionsAsync (permissions);

                var Status = await CrossPermissions.Current.CheckPermissionStatusAsync (Permission.Camera);

                if (Status == PermissionStatus.Granted)
                {
                    _pictureChooserTask.TakePicture (400, 95, OnPicture, () => { });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnPicture (Stream image)
        {
            imageStream = image;
        }

        public void DoSairCadastrarUsuario ()
        {
            _navigationService.Close (this);
        }

        public async void DoCadastrarUsuario ()
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create ("ftp://accessfun.somee.com/www.AccessFun.somee.com/Images/" + emailUsuario + nomeUsuario + dataNascimentoUsuario + ".png");
            req.UseBinary = true;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential ("Ezekieldor", "Eze92251442");

            byte[] fileData = ConverteStreamToByteArray (imageStream);

            req.ContentLength = fileData.Length;
            Stream reqStream = req.GetRequestStream ();
            reqStream.Write (fileData, 0, fileData.Length);
            reqStream.Close ();

            await DataService.AddUsuarioAsync (new Usuario (nomeUsuario, emailUsuario, senhaUsuario, dataNascimentoUsuario, enderecoUsuario, deficienciasUsuario));
            await _navigationService.Close (this);
        }

        public static byte[] ConverteStreamToByteArray (Stream stream)
        {
            byte[] byteArray = new byte[16 * 1024];
            using (MemoryStream mStream = new MemoryStream ())
            {
                int bit;
                while ((bit = stream.Read (byteArray, 0, byteArray.Length)) > 0)
                {
                    mStream.Write (byteArray, 0, bit);
                }
                return mStream.ToArray ();
            }
        }
    }
}
