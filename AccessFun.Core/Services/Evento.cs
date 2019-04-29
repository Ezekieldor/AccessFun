using System;
using System.Collections.Generic;
using System.Text;

namespace AccessFun.Core.Services
{
    public class Evento
    {
        public int Id { get; set; }
        public string Criador { get; set; }
        public string Nome { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Local { get; set; }
        public string Detalhes { get; set; }
        public string Deficiencias { get; set; }

        public Evento (string criador, string nome, string data, string hora, string local, string det, string def)
        {
            Criador = criador;
            Nome = nome;
            Data = data;
            Hora = hora;
            Local = local;
            Detalhes = det;
            Deficiencias = def;
        }

        public Evento ()
        {

        }
    }
}
