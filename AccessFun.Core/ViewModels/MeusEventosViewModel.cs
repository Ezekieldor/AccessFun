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
    public class MeusEventosViewModel : MvxViewModel<Usuario>
    {
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

        public MeusEventosViewModel (IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override async Task Initialize ()
        {
            await base.Initialize ();

            _usuario = usuarioParameter;
            _photoAlbum = App.photoAlbumEvento;
            _nome = usuario.Nome;
            _urlFoto = usuario.Email + usuario.Nome + usuario.DataNascimento;
        }

        public static PhotoAlbumEvento GetPhotoAlbum (string key)
        {
            List<PhotoEvento> list = new List<PhotoEvento> ();

            foreach (PhotoEvento p in _photoAlbum.mPhotos)
            {
                if (p.Criater == key)
                {
                    list.Add (p);
                }
            }

            return new PhotoAlbumEvento (list);
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
