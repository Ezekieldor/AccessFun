using System.Net.Http;
using System.Threading.Tasks;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using RecyclerViewer;
using SearchView = Android.Support.V7.Widget.SearchView;
using Refractored.Controls;
using AccessFun.Core.Services;
using AccessFun.Core;
using System.ComponentModel;
using System.Threading;
using System;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "Home")]
    public class UserView : MvxAppCompatActivity<UserViewModel>
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
            SetContentView (Resource.Layout.UserView);

            ToolBarSet ();

            AlertSet ();

            EventsViewSet ("");

            App.eventosParticipando = await DataService.GetEventosParticipandoAsync (UserViewModel.usuario.Email);

            refreshLayout = FindViewById <SwipeRefreshLayout> (Resource.Id.swipeRefreshLayout);
            refreshLayout.SetColorSchemeColors (Color.Red, Color.Green, Color.Blue, Color.Yellow);
            refreshLayout.Refresh += RefreshLayout_Refresh;

            await SetImageAsync ("http://accessfun.somee.com/Images/" + UserViewModel.UrlFoto + ".png");
        }

        private void RefreshLayout_Refresh (object sender, EventArgs e)
        {
            App.photoAlbumEvento = new PhotoAlbumEvento ();
            EventsViewSet ("");
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

            drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);

            var drawerToggle = new ActionBarDrawerToggle (this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.AddDrawerListener (drawerToggle);
            drawerToggle.SyncState ();

            var navigationView = FindViewById<NavigationView> (Resource.Id.nav_view);
            navigationView.SetCheckedItem (Resource.Id.home);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            toolbar.InflateMenu (Resource.Menu.itemSearch);

            var search = toolbar.Menu.FindItem (Resource.Id.search);
            var searchView = search.ActionView.JavaCast<SearchView> ();

            searchView.QueryTextChange += searchView_QueryTextChange;
        }

        private void EventsViewSet (string key)
        {
            mRecyclerView = FindViewById<RecyclerView> (Resource.Id.recyclerView);

            mLayoutManager = new LinearLayoutManager (this);
            mRecyclerView.SetLayoutManager (mLayoutManager);

            photoAlbum = UserViewModel.GetPhotoAlbum (key);

            mAdapter = new PhotoAlbumAdapterEvento (photoAlbum);

            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter (mAdapter);
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

        private void searchView_QueryTextChange (object sender, SearchView.QueryTextChangeEventArgs e)
        {
            //Toast.MakeText (this, e.NewText.ToUpper (), ToastLength.Short).Show ();
            EventsViewSet (e.NewText.ToUpper ());
        }

        void OnItemClick (object sender, int position)
        {
            UserViewModel._navigationService.Navigate<EventoViewModel, DetailArgsNavigationEvento> (new DetailArgsNavigationEvento (photoAlbum[position].evento, UserViewModel.usuario));
            //Dialog dialog = alert.Create ();
            //dialog.Show ();
        }

        void NavigationView_NavigationItemSelected (object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.home):
                    break;
                case (Resource.Id.meus_eventos):
                    UserViewModel._navigationService.Navigate<MeusEventosViewModel, Usuario> (UserViewModel.usuario);
                    break;
                case (Resource.Id.historico_eventos):
                    UserViewModel._navigationService.Navigate<HistoricoEventosViewModel, Usuario> (UserViewModel.usuario);
                    break;
                case (Resource.Id.suporte):
                    Toast.MakeText (this, "selecionando menu Xamarin", ToastLength.Short).Show ();
                    break;
                case (Resource.Id.sair):
                    UserViewModel._navigationService.Navigate<LoginViewModel> ();
                    break;
            }
        }
    }
}