namespace back.Services
{
    public class ClasseHerosService
    {
        private readonly conquerorBladeContext context;
        public ClasseHerosService(conquerorBladeContext _context)
        {
            context = _context;
        }

        public async Task<IQueryable?> Lister()
        {
            IQueryable? liste = null;

            await Task.Run(() =>
            {
                liste = from classeHeros in context.ClasseHeros
                        select new
                        {
                            classeHeros.Id,
                            classeHeros.Nom,
                            classeHeros.NomImg,
                            classeHeros.IconClasse
                        };
            });

            return liste;
        }

        public async Task<int> Ajouter(ClasseHero classeHero)
        {
            await context.ClasseHeros.AddAsync(classeHero);
            await context.SaveChangesAsync();

            int id = context.ClasseHeros.OrderByDescending(c => c.Id).Select(c => c.Id).First();

            return id;
        }
    }
}
