using AccessFun.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using RecyclerViewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessFun.Core.ViewModels
{
    public class UserViewModel : MvxViewModel<Usuario>
    {
        public static PhotoAlbumEvento _photoAlbum;
        private static Usuario _usuario;
        private Usuario usuarioParameter;
        private string _nome;
        private static string _urlFoto;
        public static IMvxNavigationService _navigationService;
        public override void Prepare (Usuario parameter)
        {
            usuarioParameter = parameter;
        }

        public UserViewModel (IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public static Usuario usuario
        {
            get => _usuario;
        }

        public override async Task Initialize ()
        {
            await base.Initialize ();

            _photoAlbum = App.photoAlbumEvento;
            _usuario = usuarioParameter;
            _nome = usuario.Nome;
            _urlFoto = _usuario.Email + _usuario.Nome + _usuario.DataNascimento;
        }

        public static PhotoAlbumEvento GetPhotoAlbum (string key)
        {
            List<PhotoEvento> list = new List<PhotoEvento> ();

            foreach (PhotoEvento p in _photoAlbum.mPhotos)
            {
                if (key != "")
                {
                    if (p.Caption.ToUpper () == key)
                    {
                        list.Add (p);
                        continue;
                    }
                    else if (p.Criater.ToUpper () == key)
                    {
                        list.Add (p);
                        continue;
                    }
                    for (int i = 0; i < p.Deficiency.Length; i += 3)
                    {
                        string str = "";
                        while (i < p.Deficiency.Length && p.Deficiency[i] != ' ')
                        {
                            str += p.Deficiency[i++];
                        }
                        if (str.ToUpper () == key)
                        {
                            list.Add (p);
                            break;
                        }
                    }
                }
                else list.Add (p);
            }

            return new PhotoAlbumEvento (list);
        }

        private MvxCommand _viewCriarEventoCommand;
        public ICommand ViewCriarEventoCommand
        {
            get
            {
                _viewCriarEventoCommand = _viewCriarEventoCommand ?? new MvxCommand (DoLogin);
                return _viewCriarEventoCommand;
            }
        }

        private void DoLogin ()
        {
            _navigationService.Navigate<CriarEventoViewModel, Usuario> (usuario);
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
