using Newtonsoft.Json;
using RecyclerViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AccessFun.Core.Services
{
    public class DataService
    {
        public static HttpClient client = new HttpClient ();
        public static string[] itensDeficiencias = { "Visual", "Auditiva", "Mental", "Física" };

        public static async Task<List<Usuario>> GetUsuariosAsync ()
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/Usuarios";
                var response = await client.GetStringAsync (url);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>> (response);

                return usuarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<Evento>> GetEventosParticipandoAsync (string email)
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/Eventos?email=" + email;
                var response = await client.GetStringAsync (url);
                var eventos = JsonConvert.DeserializeObject<List<Evento>> (response);

                return eventos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<Evento>> GetEventosAsync ()
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/Eventos";
                var response = await client.GetStringAsync (url);
                var eventos = JsonConvert.DeserializeObject<List<Evento>> (response);

                return eventos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<List<RUsuarioEvento>> GetRUsuarioEventoAsync (RUsuarioEvento rUsuarioEvento)
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/RUsuarioEventos?id=" + rUsuarioEvento.Id + "&email=" + rUsuarioEvento.Email;
                var response = await client.GetStringAsync (url);
                var eventos = JsonConvert.DeserializeObject<List<RUsuarioEvento>> (response);

                return eventos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task DeixarParticipacaoEventoAsync (RUsuarioEvento rUsuarioEvento)
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/RUsuarioEventos?id=" + rUsuarioEvento.Id + "&email=" + rUsuarioEvento.Email;
                var uri = new Uri (string.Format (url, rUsuarioEvento.Email));

                HttpResponseMessage response = null;

                response = await client.DeleteAsync (uri);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception ("Erro ao deletar evento");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task ParticiparEventoAsync (RUsuarioEvento r)
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/RUsuarioEventos/{0}";
                var uri = new Uri (string.Format (url, r.Email));

                var data = JsonConvert.SerializeObject (r);
                var content = new StringContent (data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync (uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception ("Erro ao incluir usuario");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task AddUsuarioAsync (Usuario usuario)
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/Usuarios/{0}";
                var uri = new Uri (string.Format (url, usuario.Email));

                var data = JsonConvert.SerializeObject (usuario);
                var content = new StringContent (data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync (uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception ("Erro ao incluir usuario");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task AddEventoAsync (Evento evento)
        {
            try
            {
                string url = "http://www.accessfun.somee.com/api/Eventos/{0}";
                var uri = new Uri (string.Format (url, evento.Nome));

                var data = JsonConvert.SerializeObject (evento);
                var content = new StringContent (data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync (uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception ("Erro ao incluir evento");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
