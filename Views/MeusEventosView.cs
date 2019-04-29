using AccessFun.Core;
using AccessFun.Core.Services;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using RecyclerViewer;
using Refractored.Controls;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "Meus Eventos")]
    public class MeusEventosView : MvxAppCompatActivity<MeusEventosViewModel>
    {
        CircleImageView image;
        private SwipeRefreshLayout refreshLayout;
        Android.App.AlertDialog.Builder alert;
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        PhotoAlbumAdapterEvento mAdapter;
        DrawerLayout drawerLayout;
        PhotoAlbumEvento photoAlbum;
        protected async override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.MeusEventosView);

            ToolBarSet ();
            //Toast.MakeText (this, MeusEventosViewModel.usuario.Email, ToastLength.Short).Show ();

            AlertSet ();

            EventsViewSet (MeusEventosViewModel.usuario.Email);

            refreshLayout = FindViewById<SwipeRefreshLayout> (Resource.Id.swipeRefreshLayout);
            refreshLayout.SetColorSchemeColors (Color.Red, Color.Green, Color.Blue, Color.Yellow);
            refreshLayout.Refresh += RefreshLayout_Refresh;

            await SetImageAsync ("http://accessfun.somee.com/Images/" + MeusEventosViewModel.UrlFoto + ".png");
        }

        private void RefreshLayout_Refresh (object sender, EventArgs e)
        {
            App.photoAlbumEvento = new PhotoAlbumEvento ();
            EventsViewSet (MeusEventosViewModel.usuario.Email);
            BackgroundWorker work = new BackgroundWorker ();
            work.DoWork += Work_DoWork;
            work.RunWorkerCompleted += Work_RunWorkerCompleted;
            work.RunWorkerAsync ();
        }
        private void Work_RunWorkerCompleted (object sender, RunWorkerCompletedEventArgs e)
        {
            refreshLayout.Refreshing = false;
        }
        private void Work_DoWork (object sender, DoWorkEventArgs e)
        {
            Thread.Sleep (1000);
        }

        public async Task SetImageAsync (string url)
        {
            using (var bm = await GetImageFromUrl (url))
            {
                image = FindViewById<CircleImageView> (Resource.Id.imageHeaderView123123);
                image.SetImageBitmap (bm);
            }
        }
        private async Task<Bitmap> GetImageFromUrl (string url)
        {
            using (var client = new HttpClient ())
            {
                var msg = await client.GetAsync (url);
                if (msg.IsSuccessStatusCode)
                {
                    using (var stream = await msg.Content.ReadAsStreamAsync ())
                    {
                        var bitmap = await BitmapFactory.DecodeStreamAsync (stream);
                        return bitmap;
                    }
                }
            }
            return null;
        }

        private void ToolBarSet ()
        {
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
            SetSupportActionBar (toolbar);

            drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);

            var drawerToggle = new ActionBarDrawerToggle (this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.AddDrawerListener (drawerToggle);
            drawerToggle.SyncState ();

            var navigationView = FindViewById<NavigationView> (Resource.Id.nav_view);
            navigationView.SetCheckedItem (Resource.Id.meus_eventos);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
        }

        private void EventsViewSet (string key)
        {
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            mLayoutManager = new LinearLayoutManager (this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            photoAlbum = MeusEventosViewModel.GetPhotoAlbum (key);

            mAdapter = new PhotoAlbumAdapterEvento (photoAlbum);

            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter(mAdapter);
        }

        void OnItemClick (object sender, int position)
        {
            UserViewModel._navigationService.Navigate<EventoViewModel, DetailArgsNavigationEvento> (new DetailArgsNavigationEvento (photoAlbum[position].evento, UserViewModel.usuario));
        }

        private void AlertSet ()
        {
            alert = new Android.App.AlertDialog.Builder (this);

            alert.SetTitle ("Participar deste evento");
            alert.SetMessage ("Confirma a presença neste evento?");
            alert.SetPositiveButton ("Sim", (senderAlert, args) => {
                Toast.MakeText (this, "Confirmado!", ToastLength.Short).Show ();
            });

            alert.SetNegativeButton ("Não", (senderAlert, args) => {
                Toast.MakeText (this, "Cancelado!", ToastLength.Short).Show ();
            });
        }

        void NavigationView_NavigationItemSelected (object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.home):
                    MeusEventosViewModel._navigationService.Navigate<UserViewModel, Usuario> (MeusEventosViewModel.usuario);
                    break;
                case (Resource.Id.meus_eventos):
                    break;
                case (Resource.Id.historico_eventos):
                    MeusEventosViewModel._navigationService.Navigate<HistoricoEventosViewModel, Usuario> (MeusEventosViewModel.usuario);
                    break;
                case (Resource.Id.suporte):
                    Toast.MakeText (this, "selecionando menu Xamarin", ToastLength.Short).Show ();
                    break;
                case (Resource.Id.sair):
                    MeusEventosViewModel._navigationService.Navigate<LoginViewModel> ();
                    break;
            }
        }
    }
}