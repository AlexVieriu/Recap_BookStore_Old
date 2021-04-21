using BookStore_API.Data;
using BookStore_API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<bool> Create(Book entity)
        {
            // create query
            await _db.Books.AddAsync(entity);

            // save query
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            _db.Books.Remove(entity);

            return await Save();
        }

        public async Task<bool> Exists(int id)
        {
            var isExist = await _db.Books.AnyAsync(q => q.Id == id);

            return isExist;
        }

        public async Task<IList<Book>> FindAll()
        {
            var books = await _db.Books.Include(q => q.Author)
                                       .ToListAsync();

            return books;
        }

        public async Task<Book> FindById(int id)
        {
            var book = await _db.Books.Include(q => q.Author)
                                      .FirstOrDefaultAsync(q => q.Id == id);

            return book;
        }

        public async Task<bool> Save()
        {
            var records = await _db.SaveChangesAsync();
            return records > 0;
        }

        public async Task<bool> Update(Book entity)
        {
             _db.Books.Update(entity);

            return await Save();
        }
    }
}
