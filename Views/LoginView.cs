using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "AccessFun")]
    public class LoginView : MvxActivity<LoginViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.LoginView);
        }
    }
}