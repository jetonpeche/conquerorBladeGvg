using System.Data;

namespace back.Services
{
    public class UniteService
    {
        private const string urlImg = "https://cb-gvg-api.jetonpeche.fr/imgUnite"; //"http://localhost:5019/imgUnite";

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
                            NomImg = string.Format($"{urlImg}/{u.NomImg}"),
                            u.Nom,
                            u.IdCouleur,
                            Couleur = u.IdCouleurNavigation.Nom,
                            u.IdTypeUnite,
                            NomTypeUnite = u.IdTypeUniteNavigation.Nom
                        };
            });


            return liste;
        }

        public async Task<IQueryable> ListerIdMesUnite(int _idCompte)
        {
            IQueryable? liste = null;

            await Task.Run(() =>
            {
                liste = from u in context.UniteComptes
                        where u.IdCompte == _idCompte
                        select new
                        {
                            Id = u.IdUnite,
                            Niveau = u.NiveauMaitrise
                        };
            });

            return liste;
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
    }
}
