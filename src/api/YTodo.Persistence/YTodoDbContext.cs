﻿using Microsoft.EntityFrameworkCore;
using YTodo.Persistence.Entities;

namespace YTodo.Persistence;

public class YTodoDbContext(DbContextOptions<YTodoDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(YTodoDbContext).Assembly);
    }
}