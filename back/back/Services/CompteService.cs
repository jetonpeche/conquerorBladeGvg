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

        public async Task<bool> EstAdmin(string _idDiscord)
        {
            int estAdmin = 0;

            await Task.Run(() =>
            {
                estAdmin = context.Comptes.First(c => c.IdDiscord == _idDiscord).EstAdmin;
            });

            return estAdmin == 1;
        }

        public bool IdDiscordExiste(string _idDiscord)
        {
            int nbCompte = context.Comptes.Count(c => c.IdDiscord == _idDiscord);

            return nbCompte == 1;
        }

        public async Task<List<CompteExport>> Lister()
        {
            List<CompteExport> liste = Array.Empty<CompteExport>().ToList();

            GvgService gvgService = new(context);
            int idGvg = await gvgService.GetIdProchaineGvG();

            liste = (from c in context.Comptes
                    orderby c.Pseudo
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
                        EstAdmin = c.EstAdmin,
                        ParticipeProchaineGvg = idGvg == 0 ? false : c.GvgComptes.Where(g => g.IdGvg == idGvg && g.IdCompte == c.Id).Count() == 1,
                        ListeIdGvgParticipe = c.GvgComptes.Select(g => g.IdGvg).ToList()
                    }).ToList();

            return liste;
        }

        public async Task<int> GetIdCompte(string _idDiscord)
        {
            int id = 0;

            await Task.Run(() =>
            {
                id = context.Comptes.Where(c => c.IdDiscord == _idDiscord).Select(c => c.Id).FirstOrDefault();
            });

            return id;
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
                            EstAdmin = c.EstAdmin,
                            ListeIdGvgParticipe = c.GvgComptes.Select(g => g.IdGvg).ToList()
                        }).First();
            });

            return info;
        }

        public async Task Ajouter(Compte _compte)
        {
            context.Comptes.Add(_compte);
            await context.SaveChangesAsync();
        }

        public async Task Modifier(Compte _compte)
        {
            int estAdmin = context.Comptes.Where(c => c.Id == _compte.Id).Select(c => c.EstAdmin).First();
            _compte.EstAdmin = estAdmin;

            context.Comptes.Update(_compte);
            await context.SaveChangesAsync();
        }

        public async Task Supprimer(int _id)
        {
            Compte compte = context.Comptes.Where(c => c.Id.Equals(_id)).First();

            context.Comptes.Remove(compte);
            await context.SaveChangesAsync();
        }

        public async Task Supprimer(string _idDiscord)
        {
            Compte compte = context.Comptes.Where(c => c.IdDiscord.Equals(_idDiscord)).First();

            context.Comptes.Remove(compte);
            await context.SaveChangesAsync();
        }
    }
}
