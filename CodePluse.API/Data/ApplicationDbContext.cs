﻿using CodePluse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePluse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
          
        public DbSet<BlogPost>  blogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
    }
}
