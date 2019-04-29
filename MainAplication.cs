using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccessFun.Core;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using RecyclerViewer;
using Android.Net.Http;
using Android.Graphics;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Refractored.Controls;

namespace AccessFun.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<MvxAndroidSetup<App>, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }

    public class PhotoViewHolderEvento : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }
        public TextView DateHour { get; private set; }
        public TextView Deficiency { get; private set; }
        public TextView Details { get; private set; }

        public PhotoViewHolderEvento (View itemView, Action<int> listener) : base (itemView)
        {
            Image = itemView.FindViewById<ImageView> (Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView> (Resource.Id.textView1);
            DateHour = itemView.FindViewById<TextView> (Resource.Id.textView2);
            Deficiency = itemView.FindViewById<TextView> (Resource.Id.textView3);
            Details = itemView.FindViewById<TextView> (Resource.Id.textView4);

            itemView.Click += (sender, e) => listener (base.LayoutPosition);
        }
    }

    public class PhotoViewHolderUser : RecyclerView.ViewHolder
    {
        public CircleImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        public PhotoViewHolderUser (View itemView, Action<int> listener) : base (itemView)
        {
            Image = itemView.FindViewById<CircleImageView> (Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView> (Resource.Id.textView1);

            itemView.Click += (sender, e) => listener (base.LayoutPosition);
        }
    }

    public class PhotoAlbumAdapterEvento : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public PhotoAlbumEvento mPhotoAlbum;
        public PhotoAlbumAdapterEvento (PhotoAlbumEvento photoAlbum)
        {
            mPhotoAlbum = photoAlbum;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.EventoCardView, parent, false);

            PhotoViewHolderEvento vh = new PhotoViewHolderEvento (itemView, OnClick);
            return vh;
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            PhotoViewHolderEvento vh = holder as PhotoViewHolderEvento;

            SetImageAsync (vh.Image, mPhotoAlbum[position].mPhotoURI);

            vh.Caption.Text = mPhotoAlbum[position].Caption;
            vh.DateHour.Text = mPhotoAlbum[position].DateHour;
            vh.Details.Text = mPhotoAlbum[position].Details;
            vh.Deficiency.Text = mPhotoAlbum[position].Deficiency;
        }

        public async void SetImageAsync (ImageView Image, string url)
        {
            using (var bm = await GetImageFromUrl (url))
            {
                Image.SetImageBitmap (bm);
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

        public override int ItemCount
        {
            get
            {
                if (mPhotoAlbum != null) return mPhotoAlbum.NumPhotos;
                else return 0;
            }
        }

        void OnClick (int position)
        {
            if (ItemClick != null) ItemClick (this, position);
        }
    }

    public class PhotoAlbumAdapterUser : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public PhotoAlbumUser mPhotoAlbum;
        public PhotoAlbumAdapterUser (PhotoAlbumUser photoAlbum)
        {
            mPhotoAlbum = photoAlbum;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From (parent.Context).Inflate (Resource.Layout.UserCardView, parent, false);

            PhotoViewHolderUser vh = new PhotoViewHolderUser (itemView, OnClick);
            return vh;
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            PhotoViewHolderUser vh = holder as PhotoViewHolderUser;

            SetImageAsync (vh.Image, mPhotoAlbum[position].mPhotoURI);

            vh.Caption.Text = mPhotoAlbum[position].Caption;
        }

        public async void SetImageAsync (CircleImageView Image, string url)
        {
            using (var bm = await GetImageFromUrl (url))
            {
                Image.SetImageBitmap (bm);
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

        public override int ItemCount
        {
            get
            {
                if (mPhotoAlbum != null) return mPhotoAlbum.NumPhotos;
                else return 0;
            }
        }

        void OnClick (int position)
        {
            if (ItemClick != null) ItemClick (this, position);
        }
    }
}