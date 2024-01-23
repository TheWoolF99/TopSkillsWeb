using Interfaces.Photo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private DbContextFactory _context;

        public PhotoRepository(DbContextFactory context)
        {
            this._context = context;
        }

        public async Task<byte[]?> GetAvatarUser(string UserId)
        {
            var db = _context.Create(typeof(PhotoRepository));

            return (await db.UserAvatars.Where(x => x.UserId == UserId).FirstOrDefaultAsync())?.Avatar;
        }

        public async Task OnAddUpdateAvatarUser(string UserId, byte[] Avatar)
        {
            var db = _context.Create(typeof(PhotoRepository));
            var avatar = db.UserAvatars.Where(x => x.UserId == UserId).FirstOrDefault();
            if(avatar != null)
                avatar.Avatar = Avatar;
            else
                await db.UserAvatars.AddAsync(new() { UserId = UserId, Avatar = Avatar });

            await db.SaveChangesAsync();

        }


    }
}
