using Data.Repository;
using Interfaces.Photo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class PhotoService
    {
        private readonly IPhotoRepository _photo;

        public PhotoService(IPhotoRepository photo) => this._photo = photo;

        public async Task<byte[]?> GetAvatarUser(string UserId)
        {
            return await _photo.GetAvatarUser(UserId);
        }

        public async Task OnAddUpdateAvatarUser(string UserId, byte[] Avatar)
        {
            await _photo.OnAddUpdateAvatarUser(UserId, Avatar);
        }
    }
}
