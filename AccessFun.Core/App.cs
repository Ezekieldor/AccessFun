using AccessFun.Core.Services;
using AccessFun.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.ViewModels;
using RecyclerViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccessFun.Core
{
    public class App : MvxApplication
    {
        public static PhotoAlbumEvento photoAlbumEvento;
        public static PhotoAlbumUser photoAlbumUser;
        public static List<Evento> eventosParticipando;
        public override void Initialize ()
        {
            CreatableTypes ()
                .EndingWith ("Service")
                .AsInterfaces ()
                .RegisterAsLazySingleton ();

            RegisterAppStart<LoginViewModel> ();

            photoAlbumEvento = new PhotoAlbumEvento ();
            photoAlbumUser = new PhotoAlbumUser ();
        }
    }
}
