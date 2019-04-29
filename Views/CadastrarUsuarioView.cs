using System;
using AccessFun.Core.Services;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "Usuário")]
    public class CadastrarUsuarioView : MvxActivity<CadastrarUsuarioViewModel>
    {
        Android.App.AlertDialog.Builder alert;
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.CadastrarUsuarioView);

            alert = new AlertDialog.Builder (this);

            bool[] checkedItems = new bool[100];

            alert.SetMultiChoiceItems (DataService.itensDeficiencias, checkedItems, (sender, e) =>
            {
                int index = e.Which;

                checkedItems[index] = e.IsChecked;
            });

            alert.SetPositiveButton ("Ok", (sender, e) =>
            {
                string aux = "";
                for (int i = 0; i < 4; i++)
                {
                    if (checkedItems[i] == true) aux += "1";
                    else aux += "0";
                }
                CadastrarUsuarioViewModel.DeficienciasUsuario = aux;
            });

            Button button = FindViewById<Button> (Resource.Id.ButtonDefi);

            //Assign The Event To Button
            button.Click += delegate
            {

                Dialog dialog = alert.Create ();
                dialog.Show ();
            };
        }
    }
}