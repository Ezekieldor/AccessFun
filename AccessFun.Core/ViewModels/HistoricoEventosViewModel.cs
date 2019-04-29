using AccessFun.Core.Services;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using RecyclerViewer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccessFun.Core.ViewModels
{
    public class HistoricoEventosViewModel : MvxViewModel<Usuario>
    {
        public static List<Evento> eventosParticipando;
        public static PhotoAlbumEvento _photoAlbum;
        public static Usuario _usuario;
        public Usuario usuarioParameter;
        private string _nome;
        private static string _urlFoto;
        public static string _email;
        public static IMvxNavigationService _navigationService;
        public override void Prepare (Usuario parameter)
        {
            usuarioParameter = parameter;
        }

        public HistoricoEventosViewModel (IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async Task Initialize ()
        {
            await base.Initialize ();

            _usuario = usuarioParameter;
            _photoAlbum = App.photoAlbumEvento;
            _nome = _usuario.Nome;
            _urlFoto = _usuario.Email + _usuario.Nome + _usuario.DataNascimento;
            eventosParticipando = App.eventosParticipando;
        }

        public static PhotoAlbumEvento GetPhotoAlbum ()
        {
            List<PhotoEvento> mPhotos = new List<PhotoEvento> ();
            foreach (Evento ev in eventosParticipando)
            {
                string aux = "";
                int cont = 0;
                for (int i = 0; i < ev.Deficiencias.Length; i++)
                {
                    if (ev.Deficiencias[i] == '1')
                    {
                        if (cont++ > 0) aux += " - ";
                        aux += DataService.itensDeficiencias[i];
                    }
                }
                mPhotos.Add (new PhotoEvento
                {
                    mPhotoURI = "http://accessfun.somee.com/Images/" + ev.Criador + ev.Nome + ev.Data + ev.Hora + ".png",
                    mCaption = ev.Nome,
                    mDateHour = ev.Data + " - " + ev.Hora,
                    mDetails = ev.Detalhes,
                    mDeficiency = aux,
                    mCriater = ev.Criador,
                    mevento = ev
                });
            }
            return new PhotoAlbumEvento (mPhotos);
        }

        public static Usuario usuario
        {
            get => _usuario;
        }

        public string NomeUsuario
        {
            get => _nome;
        }

        public static string UrlFoto
        {
            get => _urlFoto;
        }
    }
}
