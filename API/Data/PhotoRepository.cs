using System.Collections.Immutable;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context.Photos.FindAsync(id);
            // return await _context.Photos
            //     .IgnoreQueryFilters()
            //     .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
        {
            return await _context.Photos.Where(p => p.isApproved == false).Select(u => new PhotoForApprovalDto
            {
                Id = u.Id,
                Url = u.Url,
                Username = u.AppUser.UserName,
                isApproved = u.isApproved
            }).ToListAsync();
        }

        public async Task<AppUser> GetUserByPhotoId(int photoId)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .IgnoreQueryFilters()
                .Where(u => u.Photos.Any(p => p.Id == photoId))
                .FirstOrDefaultAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }
    }
}