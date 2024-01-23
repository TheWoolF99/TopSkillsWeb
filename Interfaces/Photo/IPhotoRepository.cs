using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Photo
{
    public interface IPhotoRepository
    {
        public Task<byte[]?> GetAvatarUser(string UserId);
        public Task OnAddUpdateAvatarUser(string UserId, byte[] Avatar);

    }
}
