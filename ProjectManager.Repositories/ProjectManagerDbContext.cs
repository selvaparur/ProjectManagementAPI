﻿using ProjectManager.Entities;
using System.Data.Entity;

namespace ProjectManager.Repositories
{
    public class ProjectManagerDbContext : DbContext
    {
        public ProjectManagerDbContext():base("name=ProjectManagerDb")
        {
        }

        public DbSet<ParentTask> ParentTasks { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTask> Tasks { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
