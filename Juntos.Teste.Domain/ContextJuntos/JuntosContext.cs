using Juntos.Teste.Domain.Models;
using Juntos.Teste.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Juntos.Teste.Domain.ContextJuntos
{
    public class JuntosContext : DbContext
    {
        public JuntosContext()
        { 
        }

        public JuntosContext(DbContextOptions<JuntosContext> opcoes)
            : base(opcoes)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(etd =>
            {
                etd.ToTable("Usuario");
                etd.HasKey(c => c.Id).HasName("Id");
                etd.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
