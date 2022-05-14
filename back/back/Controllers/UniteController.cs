namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UniteController : ControllerBase
    {
        private UniteService uniteService;
        private IConfiguration configuration;

        public UniteController(conquerorBladeContext _context, IConfiguration _configuration)
        {
            configuration = _configuration;
            uniteService = new(_context);
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await uniteService.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpGet("listerIdMesUnite/{idCompte}")]
        public async Task<string> Lister(int idCompte)
        {
            var liste = await uniteService.ListerIdMesUnite(idCompte);

            return JsonConvert.SerializeObject(liste);
        }

        [HttpPut("modifierLvl")]
        public async Task<string> Modifier([FromBody] UniteCompteImport _uniteCompte)
        {
            UniteCompte uniteCompte = new()
            {
                IdCompte = _uniteCompte.IdCompte,
                IdUnite = _uniteCompte.IdUnite,
                NiveauMaitrise = _uniteCompte.NiveauMaitrise
            };

            await uniteService.ModifierLvl(uniteCompte);

            return JsonConvert.SerializeObject(true);
        }
    }
}
