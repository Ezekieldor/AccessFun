using System.Collections.Generic;
using AccessFun.Core.Services;
using System.Threading.Tasks;

namespace RecyclerViewer
{
    // Photo: contains image resource ID and caption:
    public class PhotoEvento
    {
        public string mPhotoURI;
        public string mCaption;
        public string mDateHour;
        public string mDetails;
        public string mDeficiency;
        public string mCriater;
        public Evento mevento;

        public string PhotoURI
        {
            get { return mPhotoURI; }
        }

        public string Caption
        {
            get { return mCaption; }
        }

        public string DateHour
        {
            get { return mDateHour; }
        }

        public string Deficiency
        {
            get { return mDeficiency; }
        }

        public string Details
        {
            get { return mDetails; }
        }

        public string Criater
        {
            get { return mCriater; }
        }

        public Evento evento
        {
            get { return mevento; }
        }
    }

    public class PhotoAlbumEvento
    {
        private DataService dataService;
        private List<Evento> eventos;
        public List<PhotoEvento> mPhotos;

        public PhotoAlbumEvento (List<PhotoEvento> list)
        {
            mPhotos = list;
        }

        public PhotoAlbumEvento ()
        {
            dataService = new DataService ();
            eventos = new List<Evento> ();
            mPhotos = new List<PhotoEvento> ();
            GetEventosAsync ();
        }

        public async void GetEventosAsync ()
        {
            eventos = await DataService.GetEventosAsync ();

            foreach (Evento ev in eventos)
            {
                string aux = "";
                int cont = 0;
                for (int i = 0; i < ev.Deficiencias.Length; i++)
                {
                    if (ev.Deficiencias[i] == '1')
                    {
                        if (cont++ > 0) aux += " - ";
                        aux += DataService.itensDeficiencias[i];
                    }
                }
                mPhotos.Add (new PhotoEvento
                {
                    mPhotoURI = "http://accessfun.somee.com/Images/" + ev.Criador + ev.Nome + ev.Data + ev.Hora + ".png",
                    mCaption = ev.Nome,
                    mDateHour = ev.Data + " - " + ev.Hora,
                    mDetails = ev.Detalhes,
                    mDeficiency = aux,
                    mCriater = ev.Criador,
                    mevento = ev
                });
            }
        }

        public int NumPhotos
        {
            get { return mPhotos.Count; }
        }

        public PhotoEvento this[int i]
        {
            get { return mPhotos[i]; }
        }
    }
}