using AccessFun.Core.Services;
using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AccessFun.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private string _loginContent;
        private string _senhaContent;
        private DataService dataService;
        private List<Usuario> usuarios;


        public LoginViewModel (IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            NaoPossuiCadastroCommand = new MvxAsyncCommand (() => _navigationService.Navigate <CadastrarUsuarioViewModel> ());
        }

        public IMvxAsyncCommand NaoPossuiCadastroCommand { get; private set; }

        public override async Task Initialize ()
        {
            await base.Initialize ();

            dataService = new DataService ();
            usuarios = new List<Usuario> ();
        }

        public string LoginContent
        {
            get => _loginContent;
            set
            {
                _loginContent = value;
                RaisePropertyChanged (() => LoginContent);
            }
        }

        public string SenhaContent
        {
            get => _senhaContent;
            set
            {
                _senhaContent = value;
                RaisePropertyChanged (() => SenhaContent);
            }
        }

        private MvxCommand _entrarCommand;

        public ICommand EntrarCommand
        {
            get
            {
                _entrarCommand = _entrarCommand ?? new MvxCommand (DoLogin);
                return _entrarCommand;
            }
        }

        private void DoLogin ()
        {
            DoLoginAsync ();
        }

        public async void DoLoginAsync ()
        {
            usuarios = await DataService.GetUsuariosAsync ();

            foreach (Usuario user in usuarios)
            {
                if (user.Email == LoginContent && user.Senha == SenhaContent)
                {
                    await _navigationService.Close (this);
                    await _navigationService.Navigate<UserViewModel, Usuario> (user);
                    break;
                }
            }
        }
    }
}
