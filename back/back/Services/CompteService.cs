namespace back.Services
{
    public class CompteService
    {
        private conquerorBladeContext context { init; get; }

        public CompteService(conquerorBladeContext _context)
        {
            context = _context;
        }

        public bool Existe(string _pseudo)
        {
            int nbCompte = context.Comptes.Count(c => c.Pseudo == _pseudo);

            return nbCompte == 1;
        }

        public async Task<CompteExport?> Info(string _pseudo)
        {
            CompteExport? info = null;

            await Task.Run(() =>
            {
                info = (from c in context?.Comptes
                        where c.Pseudo == _pseudo
                        select new CompteExport
                        {
                            Id = c.Id,
                            Influance = c.Influance,
                            Pseudo = c.Pseudo,
                            IdDiscord = c.IdDiscord,
                            EstPremiereConnexion = c.EstPremiereConnexion,
                            IdClasseHeros = c.IdClasseHeros,
                            NomClasseHeros = c.IdClasseHerosNavigation.Nom,
                            NomImgClasse = c.IdClasseHerosNavigation.NomImg,
                            ListeIdGvgParticipe = c.IdGvgs.Select(g => g.Id).ToList()
                        }).First();
            });

            return info;
        }

        public async Task<int> Ajouter(Compte _compte)
        {
            context.Comptes.Add(_compte);
            await context.SaveChangesAsync();

            int id = context.Comptes.OrderByDescending(c => c.Id).Select(c => c.Id).First();

            return id;
        }

        public async Task Modifier(Compte _compte)
        {
            context.Comptes.Update(_compte);
            await context.SaveChangesAsync();
        }

        public async Task Supprimer(int _id)
        {
            Compte compte = context.Comptes.Where(c => c.Id.Equals(_id)).First();

            context.Comptes.Remove(compte);
            await context.SaveChangesAsync();
        }
    }
}
