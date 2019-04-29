using System;
using System.Collections.Generic;
using System.Text;

namespace AccessFun.Core.Services
{
    public class RUsuarioEvento
    {
        public string Email { get; set; }
        public int Id { get; set; }

        public RUsuarioEvento (string email, int id)
        {
            Email = email;
            Id = id;
        }

        public RUsuarioEvento ()
        {
                
        }
    }
}
