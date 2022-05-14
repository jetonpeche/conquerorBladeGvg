using System.Data;
using Microsoft.Data.SqlClient;

namespace back.Services
{
    public class UniteService
    {
        private conquerorBladeContext context;

        public UniteService(conquerorBladeContext _context)
        {
            context = _context;
        }

        public async Task<IQueryable> Lister()
        {
            IQueryable? liste = null;
            string chemin = Path.Combine(Directory.GetCurrentDirectory(), "imgUnite");

            await Task.Run(() =>
            {
                liste = from u in context.Unites
                        orderby u.IdCouleur descending
                        select new
                        {
                            u.Id,
                            u.Influance,
                            // recuperer l'image et pouvoir l'afficher dans HTML
                            NomImg = string.Format($"http://localhost:5019/imgUnite/{u.NomImg}"),
                            u.Nom,
                            u.IdCouleur,
                            Couleur = u.IdCouleurNavigation.Nom,
                            u.IdTypeUnite,
                            NomTypeUnite = u.IdTypeUniteNavigation.Nom
                        };
            });


            return liste;
        }

        public async Task<List<RUnite>> ListerIdMesUnite(int _idCompte)
        {
            MesUnite? liste = null;

            await Task.Run(() =>
            {
                liste = (from u in context.Comptes
                        select new MesUnite()
                        {
                            ListeUnite = u.UniteComptes.Where(u => u.IdCompte == _idCompte).Select(u => new RUnite(u.IdUnite, u.NiveauMaitrise)).ToList()
                        }).First();
            });

            return liste.ListeUnite;
        }

        public async Task AjouterUniteCompte(List<UniteCompte> _listeUniteCompte)
        {
            await context.UniteComptes.AddRangeAsync(_listeUniteCompte);
            await context.SaveChangesAsync();
        }

        public async Task ModifierLvl(UniteCompte _uniteCompte)
        {
            context.UniteComptes.Update(_uniteCompte);
            await context.SaveChangesAsync();
        }

        public record RUnite (int Id, string Niveau);

        public class MesUnite
        {
            public List<RUnite> ListeUnite { get; set; } = null!;
        }
    }
}
