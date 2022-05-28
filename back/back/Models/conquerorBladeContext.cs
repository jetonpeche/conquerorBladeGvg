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
                    .HasConstraintName("FK__Compte__idClasse__0C50D423");
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
                        l => l.HasOne<Compte>().WithMany().HasForeignKey("IdCompte").HasConstraintName("FK__GvgCompte__idCom__10216507"),
                        r => r.HasOne<Gvg>().WithMany().HasForeignKey("IdGvg").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__GvgCompte__idGvg__0F2D40CE"),
                        j =>
                        {
                            j.HasKey("IdGvg", "IdCompte").HasName("PK__GvgCompt__CA0124079E5DF419");

                            j.ToTable("GvgCompte");

                            j.IndexerProperty<int>("IdGvg").HasColumnName("idGvg");

                            j.IndexerProperty<int>("IdCompte").HasColumnName("idCompte");
                        });
            });

            modelBuilder.Entity<GvgUniteCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdGvg, e.IdCompte, e.IdUnite })
                    .HasName("PK__GvgUnite__C790EC664775E363");

                entity.ToTable("GvgUniteCompte");

                entity.Property(e => e.IdGvg).HasColumnName("idGvg");

                entity.Property(e => e.IdCompte).HasColumnName("idCompte");

                entity.Property(e => e.IdUnite).HasColumnName("idUnite");

                entity.HasOne(d => d.IdCompteNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdCompte)
                    .HasConstraintName("FK__GvgUniteC__idCom__13F1F5EB");

                entity.HasOne(d => d.IdGvgNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdGvg)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GvgUniteC__idGvg__12FDD1B2");

                entity.HasOne(d => d.IdUniteNavigation)
                    .WithMany(p => p.GvgUniteComptes)
                    .HasForeignKey(d => d.IdUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GvgUniteC__idUni__14E61A24");
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
                    .HasConstraintName("FK__Unite__idCouleur__0697FACD");

                entity.HasOne(d => d.IdTypeUniteNavigation)
                    .WithMany(p => p.Unites)
                    .HasForeignKey(d => d.IdTypeUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Unite__idTypeUni__078C1F06");
            });

            modelBuilder.Entity<UniteCompte>(entity =>
            {
                entity.HasKey(e => new { e.IdCompte, e.IdUnite })
                    .HasName("PK__UniteCom__91A50BEED9D75110");

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
                    .HasConstraintName("FK__UniteComp__idCom__17C286CF");

                entity.HasOne(d => d.IdUniteNavigation)
                    .WithMany(p => p.UniteComptes)
                    .HasForeignKey(d => d.IdUnite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UniteComp__idUni__18B6AB08");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
