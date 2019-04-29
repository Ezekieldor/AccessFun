using AccessFun.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using RecyclerViewer;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessFun.Core.ViewModels
{
    public class DetailArgsNavigationEvento
    {
        public Evento evento;
        public Usuario usuario;

        public DetailArgsNavigationEvento (Evento _evento, Usuario _usuario)
        {
            evento = _evento;
            usuario = _usuario;
        }
    }
    public class EventoViewModel : MvxViewModel<DetailArgsNavigationEvento>
    {
        private static Usuario _usuario;
        private static Evento _evento;
        public static PhotoAlbumUser _photoAlbum;
        private Usuario usuarioParameter;
        private Evento eventoParameter;
        public static IMvxNavigationService _navigationService;
        public override void Prepare (DetailArgsNavigationEvento parameter)
        {
            usuarioParameter = parameter.usuario;
            eventoParameter = parameter.evento;
        }

        public EventoViewModel (IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async Task Initialize ()
        {
            await base.Initialize ();

            _usuario = usuarioParameter;
            _evento = eventoParameter;
            _photoAlbum = App.photoAlbumUser;
        }

        public string Nome
        {
            get => _evento.Nome;
        }

        public static Evento evento
        {
            get => _evento;
        }

        public static Usuario usuario
        {
            get => _usuario;
        }

        public static bool isParticipating ()
        {
            if (App.eventosParticipando.Find (x => x.Id == _evento.Id) != null)
            {
                return true;
            }
            return false;
        }

        public static PhotoAlbumUser GetPhotoAlbum (string key)
        {
            List<PhotoUser> list = new List<PhotoUser> ();

            foreach (PhotoUser p in _photoAlbum.mPhotos)
            {
                list.Add (p);
            }

            return new PhotoAlbumUser (list);
        }

        private MvxCommand _participarEventoCommand;
        public ICommand ParticiparEventoCommand
        {
            get
            {
                _participarEventoCommand = _participarEventoCommand ?? new MvxCommand (DoParticiparEvento);
                return _participarEventoCommand;
            }
        }

        private async void DoParticiparEvento ()
        {
            List<RUsuarioEvento> rUsuarioEvento = await DataService.GetRUsuarioEventoAsync (new RUsuarioEvento (_usuario.Email, _evento.Id));
            if (rUsuarioEvento.Count == 0) await DataService.ParticiparEventoAsync (new RUsuarioEvento (_usuario.Email, _evento.Id));
            else await DataService.DeixarParticipacaoEventoAsync (new RUsuarioEvento (_usuario.Email, _evento.Id));
            App.eventosParticipando = await DataService.GetEventosParticipandoAsync (_usuario.Email);
        }
    }
}
