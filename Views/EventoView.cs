using System;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using System.Net;
using AccessFun.Core.Services;
using Refractored.Controls;
using Android.Support.Design.Widget;
using Android.Content.Res;
using Android.Support.V4.Content;
using Android.Widget;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "Evento")]
    public class EventoView : MvxAppCompatActivity<EventoViewModel>
    {
        FloatingActionButton button;
        AlertDialog.Builder alert;
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        PhotoAlbumAdapterUser mAdapter;
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.EventoView);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
            SetSupportActionBar (toolbar);

            UsuerViewSet ("");

            AlertSet ();

            ImageView image = FindViewById<ImageView> (Resource.Id.imageViewTolbarLayout);

            Evento ev = EventoViewModel.evento;

            var imageBitmap = GetImageBitmapFromUrl ("http://accessfun.somee.com/Images/" + ev.Criador + ev.Nome + ev.Data + ev.Hora + ".png");
            image.SetImageBitmap (imageBitmap);

            button = FindViewById<FloatingActionButton> (Resource.Id.FloatingActionButtonEventoView);

            if (EventoViewModel.isParticipating ())
            {
                ColorStateList csl = new ColorStateList (new int[][] { new int[0] }, new int[] { Color.Red });
                button.BackgroundTintList = csl;
                button.SetImageResource (Resource.Drawable.ic_delete);
            }
            button.Click += OnItemClickFloating;
        }

        public void OnItemClickFloating (object sender, EventArgs e)
        {
            if (EventoViewModel.isParticipating ())
            {
                ColorStateList csl = new ColorStateList (new int[][] { new int[0] }, new int[] { Color.Red });
                button.BackgroundTintList = csl;
                button.SetImageResource (Resource.Drawable.ic_delete);
            }
            else
            {
                ColorStateList csl = new ColorStateList (new int[][] { new int[0] }, new int[] { Color.Gray });
                button.BackgroundTintList = csl;
                button.SetImageResource (Resource.Drawable.ic_add2);

                Dialog dialog = alert.Create ();
                dialog.Show ();
            } 
        }

        private Bitmap GetImageBitmapFromUrl (string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient ())
            {
                var imageBytes = webClient.DownloadData (url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private void UsuerViewSet (string key)
        {
            mRecyclerView = FindViewById<RecyclerView> (Resource.Id.recyclerView);

            mLayoutManager = new LinearLayoutManager (this);
            mRecyclerView.SetLayoutManager (mLayoutManager);

            mAdapter = new PhotoAlbumAdapterUser (EventoViewModel.GetPhotoAlbum (key));

            mAdapter.ItemClick += OnItemClick;

            mRecyclerView.SetAdapter (mAdapter);
        }

        private void AlertSet ()
        {
            alert = new Android.App.AlertDialog.Builder (this);

            alert.SetTitle ("Participar deste evento");
            alert.SetMessage ("Confirmada a presença neste evento!");
            alert.SetPositiveButton ("Ok", (senderAlert, args) => { });
        }

        public void OnItemClick (object sender, int position)
        {
            //Dialog dialog = alert.Create ();
            //dialog.Show ();
        }
    }
}