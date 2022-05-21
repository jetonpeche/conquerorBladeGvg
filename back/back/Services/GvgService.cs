namespace back.Services
{
    public class GvgService
    {
        private conquerorBladeContext context;

        public GvgService(conquerorBladeContext _context)
        {
            context = _context;
        }

        public async Task<IQueryable> Lister()
        {
            IQueryable? listeRetour = null;

            await Task.Run(() =>
            {
                listeRetour = from gvg in context.Gvgs
                              select new
                              {
                                  gvg.Id,
                                  gvg.DateProgrammer,
                                  gvg.IdComptes.Count
                              };
            });

            return listeRetour;
        }
        
        public async Task<List<int>> Ajouter(List<Gvg> _gvg)
        {
            List<int> listeRetour = new();

            foreach (var element in _gvg)
            {
                await context.Gvgs.AddAsync(element);
                await context.SaveChangesAsync();

                int id = context.Gvgs.OrderByDescending(x => x.Id).Select(g => g.Id).First();

                listeRetour.Add(id);
            }
            
            return listeRetour;
        }
    }
}
