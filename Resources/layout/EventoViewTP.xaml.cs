using AccessFun.Droid.Views;
using MvvmCross;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccessFun.Droid.Resources.layout
{
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class EventoViewTP : TabbedPage
    {
        public EventoViewTP ()
        {
            InitializeComponent ();
            //MainPage = new TabbedPage ();
        }
    }
}