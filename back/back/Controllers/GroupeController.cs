namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupeController : ControllerBase
    {
        private GroupeService grpService;
        private string connexionString;

        public GroupeController(conquerorBladeContext _context, IConfiguration _config)
        {
            grpService = new(_context);
            connexionString = _config.GetConnectionString("defaut");
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await grpService.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpPut("modifierCompteGroupe")]
        public async Task<string> ModifierCompteGroupe(CompteGroupeImport _compteGrp)
        {
            await grpService.ModifierGroupeCompte(_compteGrp, connexionString);

            return JsonConvert.SerializeObject(true);
        }
    }
}
