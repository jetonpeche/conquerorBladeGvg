namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClasseHerosController : ControllerBase
    {
        private readonly ClasseHerosService classeHeros;

        public ClasseHerosController(conquerorBladeContext _context)
        {
            classeHeros = new(_context);
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await classeHeros.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpPost("ajouter")]
        public async Task<string> Ajouter([FromBody] ClasseHerosImport _classeHeros)
        {
            ClasseHero classeHero = new()
            {
                Nom = _classeHeros.Nom
            };

            int id = await classeHeros.Ajouter(classeHero);

            return JsonConvert.SerializeObject(id);
        }
    }
}
