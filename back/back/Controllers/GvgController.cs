﻿namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GvgController : ControllerBase
    {
        private GvgService gvgService;
        private IConfiguration config;
        private conquerorBladeContext context;

        public GvgController(conquerorBladeContext _context, IConfiguration _config)
        {
            context = _context;
            gvgService = new(_context);
            config = _config;
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await gvgService.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpGet("listerViaDiscord/{idDiscord}")]
        public async Task<string> ListerViaGvG(string idDiscord)
        {
            CompteService compteService = new(context);
            int idCompte = await compteService.GetIdCompte(idDiscord);

            if(idCompte == 0)
                return JsonConvert.SerializeObject("Veuillez remplir l'id discord sur le site avant, \n Ou taper la commande: !InitMonIdDiscord <pseudo>");

            var listeRetour = gvgService.Lister();

            foreach (var gvg in listeRetour.Result)
            {
                gvg.estInscrit = gvgService.Participe(idCompte, gvg.Id) ? "Oui" : "Non";
            }

            return JsonConvert.SerializeObject(listeRetour.Result);
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

        [HttpPost("participerViaDiscord")]
        public async Task<string> ParticiperViaDiscord([FromBody] GvgCompteImport _gvg)
        {
            CompteService compteService = new(context);

            int idCompte = await compteService.GetIdCompte(_gvg.IdDiscord);

            if(idCompte == 0)
                return JsonConvert.SerializeObject("Veuillez remplir l'id discord sur le site avant, \n Ou taper la commande: !InitMonIdDiscord <pseudo>");

            DateTime date = DateTime.Parse(_gvg.Date);

            if (!gvgService.Existe(date))
                return JsonConvert.SerializeObject($"Aucune GvG pour la date du: {_gvg.Date}");

            int idGvG = await gvgService.GetIdGvG(date);

            if (gvgService.Participe(idCompte, idGvG))
                return JsonConvert.SerializeObject($"Tu participe déjà a la GvG du: {_gvg.Date}");

            gvgService.connectionString = config.GetConnectionString("defaut");
            await gvgService.Participer(idGvG, idCompte);    

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
