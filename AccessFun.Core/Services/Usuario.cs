using System;
using System.Collections.Generic;
using System.Text;

namespace AccessFun.Core.Services
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Deficiencias { get; set; }

        public Usuario (string nome, string email, string senha, string data, string end, string def)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            DataNascimento = data;
            Endereco = end;
            Deficiencias = def;
        }

        public Usuario ()
        {

        }
    }
}
