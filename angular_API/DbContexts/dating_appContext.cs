﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using angular_API.ModelsFromDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace angular_API.DbContexts
{
    public partial class dating_appContext : DbContext
    {
        public dating_appContext()
        {
        }

        public dating_appContext(DbContextOptions<dating_appContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appuser> Appusers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appuser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}