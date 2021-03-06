using RedmondBlog.Data;
using RedmondBlog.Enums;
using RedmondBlog.Models;
using System.Linq;

namespace RedmondBlog.Services
{
    public class BlogSearchService
    {
        private readonly ApplicationDbContext _context;

        public BlogSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Search(string searchTerm)
        {
            var posts = _context.Posts.Where(p => p.ReadyStatus == ReadyStatus.ProductionReady).AsQueryable();
            if (searchTerm != null)
            {
                searchTerm = searchTerm.ToLower();

                posts = posts.Where(
                    p => p.Title.ToLower().Contains(searchTerm) ||
                    p.Abstract.ToLower().Contains(searchTerm) ||
                    p.Content.ToLower().Contains(searchTerm) ||
                    p.Comments.Any(c => c.Body.ToLower().Contains(searchTerm) ||
                                        c.ModeratedBody.ToLower().Contains(searchTerm) ||
                                        c.Author.FirstName.ToLower().Contains(searchTerm) ||
                                        c.Author.LastName.ToLower().Contains(searchTerm) ||
                                        c.Author.Email.ToLower().Contains(searchTerm)));
            }

            return posts.OrderByDescending(p => p.Created);

        }

        public IQueryable<Post> SearchTag(string searchTag)
        {
            var posts = _context.Posts.Where(p => p.ReadyStatus == ReadyStatus.ProductionReady).AsQueryable();

            if (searchTag != null)
            {
                searchTag = searchTag.ToLower();

                posts = posts.Where(p => p.Tags.Any(t => t.Text.ToLower().Contains(searchTag)))
                             .Where(p => p.ReadyStatus == ReadyStatus.ProductionReady);
            }

            return posts.OrderByDescending(p => p.Created);
        }

    }
}
