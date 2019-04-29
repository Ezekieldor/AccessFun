using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "")]
    public class Header : MvxAppCompatActivity <UserViewModel>
    {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.header);

            SetImageAsync ("http://accessfun.somee.com/Images/ezefoto35445625.png");
        }

        public async Task SetImageAsync (string url)
        {
            ImageView Image = FindViewById<ImageView> (Resource.Id.imageViewHeader);

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
    }
}