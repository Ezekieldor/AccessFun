using Android.App;
using MvvmCross.Platforms.Android.Views;

namespace AccessFun.Droid
{
    [Activity (Label = "AccessFun", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity ()
            : base (Resource.Layout.SplashScreen)
        {
        }
    }
}