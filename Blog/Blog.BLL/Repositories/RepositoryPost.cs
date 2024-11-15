﻿using Blog.BLL.Interfaces;
using Blog.DAL.Data;
using Blog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Repositories
{
    public class RepositoryPost:IRepositoryPost
    {
        private readonly ApplicationDbContext _context;

        public RepositoryPost(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Add(Post post)
        {
           _context.Posts.Add(post);
            return _context.SaveChanges();
        }

        public int Delete(Post post)
        {
            // Retrieve the post from the database, including related entities like Comments and Likes
            var postWithRelatedEntities = _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .FirstOrDefault(p => p.Id == post.Id);

            if (postWithRelatedEntities == null)
            {
                // If the post is not found, return 0 (or handle this as needed)
                return 0;
            }

            // Remove related comments
            _context.Comments.RemoveRange(postWithRelatedEntities.Comments);

            // Remove related likes
            _context.Likes.RemoveRange(postWithRelatedEntities.PostLikes);

            // Remove the post itself
            _context.Posts.Remove(postWithRelatedEntities);

            return _context.SaveChanges();
        }

        public Post Get(int id)
        {
            var post = _context.Posts.Local.Where(p => p.Id == id).FirstOrDefault();
            if (post == null)
            {
                post = _context.Posts.AsNoTracking().Where(p => p.Id == id).FirstOrDefault();
            }
            return post;
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.AsNoTracking()
                .Include(p => p.PostLikes).Include(p => p.Comments).ToList(); 
        }

        public IEnumerable<Post> GetUserPostsFilterByCategory(string UserId, int? CategoryId)
        {
            var query = _context.Posts.Include(p => p.Category).Include(p => p.User).Where(p => p.UserId == UserId).OrderByDescending(p => p.CreatedAt).ToList();
            if (CategoryId.HasValue)
            {
                return query.Where(p => p.CategoryId == CategoryId.Value );

            }
            return query;
        }
        public int Update(Post post)
        {
            _context.Posts.Update(post);
            return _context.SaveChanges();
        }
       public IEnumerable<Post> GetFollowingPosts(IEnumerable<string> FollowingIds, int? categoryId)
        {
            var query = _context.Posts.AsQueryable()
             .Include(p => p.User)
             .Include(p => p.Category) 
             .Where(p => FollowingIds.Contains(p.UserId));
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            var posts = query.OrderByDescending(p => p.CreatedAt).ToList();
            return posts;
        }
    }
}
