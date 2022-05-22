namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GvgController : ControllerBase
    {
        private GvgService gvgService;
        private IConfiguration config;

        public GvgController(conquerorBladeContext _context, IConfiguration _config)
        {
            gvgService = new(_context);
            config = _config;
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await gvgService.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpPost("ajouter")]
        public async Task<string> Ajouter([FromBody] GvgImport[] _gvgImport)
        {
            List<Gvg> listeGvg = new();

            foreach (var element in _gvgImport)
            {
                DateTime dateGvg = DateTime.Parse(element.Date);
                listeGvg.Add(new Gvg { DateProgrammer = dateGvg });
            }

            List<int> listeRetour = await gvgService.Ajouter(listeGvg);

            return JsonConvert.SerializeObject(listeRetour);
        }

        [HttpPost("participer")]
        public async Task<string> Participer([FromBody] GvgCompteImport _gvg)
        {
            gvgService.connectionString = config.GetConnectionString("defaut");
            await gvgService.Participer(_gvg.IdGvg, _gvg.IdCompte);

            return JsonConvert.SerializeObject(true);
        }

        [HttpPost("absent")]
        public async Task<string> Absent([FromBody] GvgCompteImport _gvg)
        {
            gvgService.connectionString = config.GetConnectionString("defaut");
            await gvgService.Absent(_gvg.IdGvg, _gvg.IdCompte);

            return JsonConvert.SerializeObject(true);
        }
    }
}
