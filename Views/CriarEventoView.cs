using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccessFun.Core.Services;
using AccessFun.Core.ViewModels;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;

namespace AccessFun.Droid.Views
{
    [Activity (Label = "Evento")]
    public class CriarEventoView : MvxActivity<CriarEventoViewModel>
    {
        AlertDialog.Builder alert;
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            SetContentView (Resource.Layout.CriarEventoView);

            alert = new Android.App.AlertDialog.Builder (this);

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
                CriarEventoViewModel.DeficienciasEvento = aux;
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