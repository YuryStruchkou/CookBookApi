using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.CoreProject.Interfaces;
using CookBook.DAL.Data;
using CookBook.Domain.Models;
using CookBook.Domain.ViewModels.CommentViewModels;

namespace CookBook.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CommentService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Comment> AddAsync(CreateCommentViewModel model, int? userId)
        {
            var recipe = _context.Recipes.Find(model?.RecipeId);
            if (recipe == null) return null;
            var comment = _mapper.Map<CreateCommentViewModel, Comment>(model);
            comment.UserId = userId;
            comment.RecipeId = model.RecipeId.Value;
            comment.CreationDate = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> GetAsync(int id)
        {
            var result = await _context.Comments.FindAsync(id);
            return result;
        }

        public async Task<Comment> UpdateAsync(UpdateCommentViewModel model, int commentId)
        {
            var comment = _context.Comments.SingleOrDefault(r => r.Id == commentId);
            if (comment == null)
            {
                return null;
            }
            comment.Content = model.Content;
            comment.EditDate = DateTime.Now;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null) return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
