using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Gvg> Gvgs { get; set; } = null!;
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

                entity.Property(e => e.EstPremiereConnexion)
                    .HasColumnName("estPremiereConnexion")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdClasseHeros).HasColumnName("idClasseHeros");

                entity.Property(e => e.IdDiscord)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("idDiscord");

                entity.Property(e => e.Influance).HasColumnName("influance");

                entity.Property(e => e.NomDiscord)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("nomDiscord");

                entity.Property(e => e.Pseudo)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("pseudo");

                entity.HasOne(d => d.IdClasseHerosNavigation)
                    .WithMany(p => p.Comptes)
                    .HasForeignKey(d => d.IdClasseHeros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Compte__idClasse__3E1D39E1");
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

            modelBuilder.Entity<Gvg>(entity =>
            {
                entity.ToTable("Gvg");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateProgrammer)
                    .HasColumnType("date")
                    .HasColumnName("dateProgrammer");

                entity.HasMany(d => d.IdComptes)
                    .WithMany(p => p.IdGvgs)
                    .UsingEntity<Dictionary<string, object>>(
                        "GvgCompte",
                        l => l.HasOne<Compte>().WithMany().HasForeignKey("IdCompte").HasConstraintName("FK__GvgCompte__idCom__41EDCAC5"),
                        r => r.HasOne<Gvg>().WithMany().HasForeignKey("IdGvg").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GvgCompte__idGvg__40F9A68C"),
                        j =>
                        {
                            j.HasKey("IdGvg", "IdCompte").HasName("PK__GvgCompt__CA012407BFD3F4A3");

                            j.ToTable("GvgCompte");

                            j.IndexerProperty<int>("IdGvg").HasColumnName("idGvg");

                            j.IndexerProperty<int>("IdCompte").HasColumnName("idCompte");
                        });
            });

            modelBuilder.Entity<GvgUniteCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdGvg, e.IdCompte })
                    .HasName("PK__GvgUnite__CA01240751E83C8C");

                entity.ToTable("GvgUniteCompte");

                entity.Property(e => e.IdGvg).HasColumnName("idGvg");

                entity.Property(e => e.IdCompte).HasColumnName("idCompte");

                entity.Property(e => e.IdUnite).HasColumnName("idUnite");

                entity.HasOne(d => d.IdCompteNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdCompte)
                    .HasConstraintName("FK__GvgUniteC__idCom__45BE5BA9");

                entity.HasOne(d => d.IdGvgNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdGvg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GvgUniteC__idGvg__44CA3770");

                entity.HasOne(d => d.IdUniteNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdUnite)
                    .HasConstraintName("FK__GvgUniteC__idUni__46B27FE2");
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
                    .HasConstraintName("FK__Unite__idCouleur__395884C4");

                entity.HasOne(d => d.IdTypeUniteNavigation)
                    .WithMany(p => p.Unites)
                    .HasForeignKey(d => d.IdTypeUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Unite__idTypeUni__3A4CA8FD");
            });

            modelBuilder.Entity<UniteCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdCompte, e.IdUnite })
                    .HasName("PK__UniteCom__91A50BEEB2820340");

                entity.ToTable("UniteCompte");

                entity.Property(e => e.IdCompte).HasColumnName("idCompte");

                entity.Property(e => e.IdUnite).HasColumnName("idUnite");

                entity.Property(e => e.NiveauMaitrise)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("niveauMaitrise");

                entity.HasOne(d => d.IdCompteNavigation)
                    .WithMany(p => p.UniteComptes)
                    .HasForeignKey(d => d.IdCompte)
                    .HasConstraintName("FK__UniteComp__idCom__498EEC8D");

                entity.HasOne(d => d.IdUniteNavigation)
                    .WithMany(p => p.UniteComptes)
                    .HasForeignKey(d => d.IdUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UniteComp__idUni__4A8310C6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
