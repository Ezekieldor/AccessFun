using AccessFun.Core.Services;
using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.PictureChooser;
using MvvmCross.ViewModels;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessFun.Core.ViewModels
{
    public class CriarEventoViewModel : MvxViewModel<Usuario>
    {
        private Stream imageStream;
        private readonly IMvxPictureChooserTask _pictureChooserTask;
        private readonly IMvxNavigationService _navigationService;
        private Usuario usuario;
        private string nomeEvento;
        private string dataEvento;
        private string localEvento;
        private string horaEvento;
        private string detalhesEvento;
        public static string deficienciasEvento;

        public override void Prepare (Usuario parameter)
        {
            usuario = parameter;
        }

        public CriarEventoViewModel (IMvxPictureChooserTask pictureChooserTask, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _pictureChooserTask = pictureChooserTask;
        }

        public override async Task Initialize ()
        {
            await base.Initialize ();
        }

        public static string DeficienciasEvento
        {
            get => deficienciasEvento;
            set
            {
                deficienciasEvento = value;
            }
        }

        public string NomeEvento
        {
            get => nomeEvento;
            set
            {
                nomeEvento = value;
                RaisePropertyChanged (() => NomeEvento);
            }
        }

        public string DataEvento
        {
            get => dataEvento;
            set
            {
                dataEvento = value;
                RaisePropertyChanged (() => DataEvento);
            }
        }

        public string LocalEvento
        {
            get => localEvento;
            set
            {
                localEvento = value;
                RaisePropertyChanged (() => LocalEvento);
            }
        }

        public string HoraEvento
        {
            get => horaEvento;
            set
            {
                horaEvento = value;
                RaisePropertyChanged (() => HoraEvento);
            }
        }

        public string DetalhesEvento
        {
            get => detalhesEvento;
            set
            {
                detalhesEvento = value;
                RaisePropertyChanged (() => DetalhesEvento);
            }
        }

        private MvxCommand _sairCriarEventoViewCommand;
        public ICommand SairCriarEventoViewCommand
        {
            get
            {
                _sairCriarEventoViewCommand = _sairCriarEventoViewCommand ?? new MvxCommand (DoSairCriarEventoView);
                return _sairCriarEventoViewCommand;
            }
        }

        public void DoSairCriarEventoView ()
        {
            _navigationService.Close (this);
        }

        private MvxCommand _criarEventoCommand;

        public ICommand CriarEventoCommand
        {
            get
            {
                _criarEventoCommand = _criarEventoCommand ?? new MvxCommand (DoCriarEvento);
                return _criarEventoCommand;
            }
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

        public async void DoCriarEvento ()
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create ("ftp://accessfun.somee.com/www.AccessFun.somee.com/Images/" + usuario.Email + nomeEvento + dataEvento + horaEvento + ".png");
            req.UseBinary = true;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential ("Ezekieldor", "Eze92251442");

            byte[] fileData = ConverteStreamToByteArray (imageStream);

            req.ContentLength = fileData.Length;
            Stream reqStream = req.GetRequestStream ();
            reqStream.Write (fileData, 0, fileData.Length);
            reqStream.Close ();

            await DataService.AddEventoAsync (new Evento (usuario.Email, nomeEvento, dataEvento, horaEvento, localEvento, detalhesEvento, deficienciasEvento));

            await _navigationService.Close (this);
        }

        private MvxCommand _choosePictureCommand;

        public ICommand ChoosePictureCommand
        {
            get
            {
                _choosePictureCommand = _choosePictureCommand ?? new MvxCommand (DoChoosePicture);
                return _choosePictureCommand;
            }
        }

        private void DoChoosePicture ()
        {
            _pictureChooserTask.ChoosePictureFromLibrary (400, 95, OnPicture, () => { });
        }

        private void OnPicture (Stream image)
        {
            imageStream = image;
        }
    }
}
