using System.Collections.Generic;
using AccessFun.Core.Services;
using System.Threading.Tasks;

namespace RecyclerViewer
{
    // Photo: contains image resource ID and caption:
    public class PhotoUser
    {
        public string mPhotoURI;
        public string mCaption;

        public string PhotoURI
        {
            get { return mPhotoURI; }
        }

        public string Caption
        {
            get { return mCaption; }
        }
    }

    public class PhotoAlbumUser
    {
        private DataService dataService;
        private List<Usuario> usuarios;
        public List<PhotoUser> mPhotos;

        public PhotoAlbumUser (List<PhotoUser> list)
        {
            mPhotos = list;
        }

        public PhotoAlbumUser ()
        {
            dataService = new DataService ();
            usuarios = new List<Usuario> ();
            mPhotos = new List<PhotoUser> ();
            GetEventosAsync ();
        }

        public async void GetEventosAsync ()
        {
            usuarios = await DataService.GetUsuariosAsync ();

            foreach (Usuario us in usuarios)
            {
                mPhotos.Add (new PhotoUser
                {
                    mPhotoURI = "http://accessfun.somee.com/Images/" + us.Email + us.Nome + us.DataNascimento + ".png",
                    mCaption = us.Nome
                });
            }
        }

        public int NumPhotos
        {
            get { return mPhotos.Count; }
        }

        public PhotoUser this[int i]
        {
            get { return mPhotos[i]; }
        }
    }
}