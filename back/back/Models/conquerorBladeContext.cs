using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace back.Models
{
    public partial class conquerorBladeContext : DbContext
    {
        public conquerorBladeContext()
        {
        }

        public conquerorBladeContext(DbContextOptions<conquerorBladeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClasseHero> ClasseHeros { get; set; } = null!;
        public virtual DbSet<Compte> Comptes { get; set; } = null!;
        public virtual DbSet<CouleurUnite> CouleurUnites { get; set; } = null!;
        public virtual DbSet<Groupe> Groupes { get; set; } = null!;
        public virtual DbSet<Gvg> Gvgs { get; set; } = null!;
        public virtual DbSet<GvgCompte> GvgComptes { get; set; } = null!;
        public virtual DbSet<GvgUniteCompte> GvgUniteComptes { get; set; } = null!;
        public virtual DbSet<TypeUnite> TypeUnites { get; set; } = null!;
        public virtual DbSet<Unite> Unites { get; set; } = null!;
        public virtual DbSet<UniteCompte> UniteComptes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClasseHero>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IconClasse)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("iconClasse");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.Property(e => e.NomImg)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nomImg");
            });

            modelBuilder.Entity<Compte>(entity =>
            {
                entity.ToTable("Compte");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EstAdmin).HasColumnName("estAdmin");

                entity.Property(e => e.EstPremiereConnexion)
                    .HasColumnName("estPremiereConnexion")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdClasseHeros).HasColumnName("idClasseHeros");

                entity.Property(e => e.IdDiscord)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("idDiscord");

                entity.Property(e => e.Influance).HasColumnName("influance");

                entity.Property(e => e.Pseudo)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("pseudo");

                entity.HasOne(d => d.IdClasseHerosNavigation)
                    .WithMany(p => p.Comptes)
                    .HasForeignKey(d => d.IdClasseHeros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Compte__idClasse__01142BA1");
            });

            modelBuilder.Entity<CouleurUnite>(entity =>
            {
                entity.ToTable("CouleurUnite");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Groupe>(entity =>
            {
                entity.ToTable("Groupe");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Gvg>(entity =>
            {
                entity.ToTable("Gvg");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateProgrammer)
                    .HasColumnType("date")
                    .HasColumnName("dateProgrammer");
            });

            modelBuilder.Entity<GvgCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdGvg, e.IdCompte })
                    .HasName("PK__GvgCompt__CA012407E8F1AFF5");

                entity.ToTable("GvgCompte");

                entity.Property(e => e.IdGvg).HasColumnName("idGvg");

                entity.Property(e => e.IdCompte).HasColumnName("idCompte");

                entity.Property(e => e.IdGroupe).HasColumnName("idGroupe");

                entity.HasOne(d => d.IdCompteNavigation)
                    .WithMany(p => p.GvgComptes)
                    .HasForeignKey(d => d.IdCompte)
                    .HasConstraintName("FK__GvgCompte__idCom__05D8E0BE");

                entity.HasOne(d => d.IdGroupeNavigation)
                    .WithMany(p => p.GvgComptes)
                    .HasForeignKey(d => d.IdGroupe)
                    .HasConstraintName("FK__GvgCompte__idGro__04E4BC85");

                entity.HasOne(d => d.IdGvgNavigation)
                    .WithMany(p => p.GvgComptes)
                    .HasForeignKey(d => d.IdGvg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GvgCompte__idGvg__03F0984C");
            });

            modelBuilder.Entity<GvgUniteCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdGvg, e.IdCompte, e.IdUnite })
                    .HasName("PK__GvgUnite__C790EC66E4FD1FE5");

                entity.ToTable("GvgUniteCompte");

                entity.Property(e => e.IdGvg).HasColumnName("idGvg");

                entity.Property(e => e.IdCompte).HasColumnName("idCompte");

                entity.Property(e => e.IdUnite).HasColumnName("idUnite");

                entity.HasOne(d => d.IdCompteNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdCompte)
                    .HasConstraintName("FK__GvgUniteC__idCom__09A971A2");

                entity.HasOne(d => d.IdGvgNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdGvg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GvgUniteC__idGvg__08B54D69");

                entity.HasOne(d => d.IdUniteNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GvgUniteC__idUni__0A9D95DB");
            });

            modelBuilder.Entity<TypeUnite>(entity =>
            {
                entity.ToTable("TypeUnite");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Unite>(entity =>
            {
                entity.ToTable("Unite");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EstMeta).HasColumnName("estMeta");

                entity.Property(e => e.EstVisible)
                    .HasColumnName("estVisible")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdCouleur).HasColumnName("idCouleur");

                entity.Property(e => e.IdTypeUnite).HasColumnName("idTypeUnite");

                entity.Property(e => e.Influance).HasColumnName("influance");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.Property(e => e.NomImg)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nomImg");

                entity.HasOne(d => d.IdCouleurNavigation)
                    .WithMany(p => p.Unites)
                    .HasForeignKey(d => d.IdCouleur)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Unite__idCouleur__7B5B524B");

                entity.HasOne(d => d.IdTypeUniteNavigation)
                    .WithMany(p => p.Unites)
                    .HasForeignKey(d => d.IdTypeUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Unite__idTypeUni__7C4F7684");
            });

            modelBuilder.Entity<UniteCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdCompte, e.IdUnite })
                    .HasName("PK__UniteCom__91A50BEE596DAFE5");

                entity.ToTable("UniteCompte");

                entity.Property(e => e.IdCompte).HasColumnName("idCompte");

                entity.Property(e => e.IdUnite).HasColumnName("idUnite");

                entity.Property(e => e.EstTemporaire).HasColumnName("estTemporaire");

                entity.Property(e => e.NiveauMaitrise)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("niveauMaitrise");

                entity.HasOne(d => d.IdCompteNavigation)
                    .WithMany(p => p.UniteComptes)
                    .HasForeignKey(d => d.IdCompte)
                    .HasConstraintName("FK__UniteComp__idCom__0E6E26BF");

                entity.HasOne(d => d.IdUniteNavigation)
                    .WithMany(p => p.UniteComptes)
                    .HasForeignKey(d => d.IdUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UniteComp__idUni__0F624AF8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
