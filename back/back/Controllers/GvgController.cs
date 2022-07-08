namespace back.Controllers
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
            gvgService.connectionString = _config.GetConnectionString("defaut");
            config = _config;
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await gvgService.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpGet("listerParticipant/{idGvg}")]
        public async Task<string> ListerParticipant(int idGvg)
        {
            var liste = await gvgService.listerParticipant(idGvg);

            return JsonConvert.SerializeObject(liste);
        }

        [HttpGet("listerMesUnites/{idGvg}/{idCompte}")]
        public async Task<string> ListerMesUnites(int idGvg, int idCompte)
        {
            var liste = await gvgService.ListerMesUnites(idGvg, idCompte);

            return JsonConvert.SerializeObject(liste);
        }

        // A TESTER
        [HttpGet("listerMesUnitesViaDiscord/{dateGvg}/{idDiscord}")]
        public async Task<string> ListerMesUniteViaDiscord(string dateGvg, string idDiscord)
        {
            CompteService compteServ = new(context);
            int idCompte = await compteServ.GetIdCompte(idDiscord);

            if (idCompte is 0)
                return JsonConvert.SerializeObject("Je ne te connais pas");

            if (DateTime.TryParse(dateGvg, out DateTime date))
            {
                if (!gvgService.Existe(date))
                    return JsonConvert.SerializeObject($"Aucune GvG pour le {dateGvg}");

                int idGvg = await gvgService.GetIdGvG(date);

                if (!gvgService.Participe(idCompte, idGvg))
                    return JsonConvert.SerializeObject("Tu ne participes pas a cette GvG");

                var listeUnite = await gvgService.ListerMesUnites(idGvg, idCompte);

                return JsonConvert.SerializeObject(listeUnite);
            }
            else
            {
                int idGvg = await gvgService.GetIdProchaineGvG();

                if (idGvg is 0)
                    return JsonConvert.SerializeObject("Aucune GvG prochainement");

                var listeUnite = await gvgService.ListerMesUnites(idGvg, idCompte);

                return JsonConvert.SerializeObject(listeUnite);
            } 
        }

        [HttpGet("listerViaDiscord/{idDiscord}")]
        public async Task<string> ListerViaGvG(string idDiscord)
        {
            CompteService compteService = new(context);
            int idCompte = await compteService.GetIdCompte(idDiscord);

            if(idCompte == 0)
                return JsonConvert.SerializeObject("Veuillez remplir l'id discord sur le site avant, ou taper la commande: !InitMonIdDiscord");

            var listeRetour = gvgService.Lister();

            foreach (var gvg in listeRetour.Result)
            {
                gvg.estInscrit = gvgService.Participe(idCompte, gvg.Id) ? "Oui" : "Non";
            }

            return JsonConvert.SerializeObject(listeRetour.Result);
        }

        [HttpGet("recupererInfoGvgDuJour")]
        public async Task<string> RecupererInfoGvg()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var liste = await gvgService.ListerParametrer(date);

            return JsonConvert.SerializeObject(liste);
        }

        [HttpGet("pingerNonIncritProchaineGvg")]
        public async Task<string> ListerLesInscritsProchaineGvg()
        {
            string retour = await gvgService.ListerIdDiscordNonInscritProchaineGvG();

            return JsonConvert.SerializeObject(retour);
        }

        /// <summary>
        /// Utiliser automatiquement par le serveur pour les soirs
        /// NE PAS UTILISER
        /// </summary>
        /// <returns></returns>
        [HttpGet("CoroutineSuppGvGPasser")]
        public async Task SupprimerGvGPasser()
        {
            DateTime date = DateTime.Parse(DateTime.Now.ToString("d"));

            await gvgService.SupprimerGvgPasser(date);
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
                return JsonConvert.SerializeObject("Je ne te connais pas");

            int idGvG;
            bool estProchaineGvG;

            if (DateTime.TryParse(_gvg.Date, out DateTime date))
            {
                estProchaineGvG = false;

                if (!gvgService.Existe(date))
                    return JsonConvert.SerializeObject($"Aucune GvG pour la date du: {_gvg.Date}");

                idGvG = await gvgService.GetIdGvG(date);
            }
            else
            {
                estProchaineGvG = true;
                idGvG = await gvgService.GetIdProchaineGvG();

                if (idGvG is 0)
                    return JsonConvert.SerializeObject("Aucune GvG programmée prochainement");
            }

            if (gvgService.Participe(idCompte, idGvG))
                return JsonConvert.SerializeObject(estProchaineGvG ? "Tu participes déjà à la prochaine GvG" : $"Tu participes déjà à la GvG du: {_gvg.Date}");

            gvgService.connectionString = config.GetConnectionString("defaut");
            await gvgService.Participer(idGvG, idCompte);    

            return JsonConvert.SerializeObject(estProchaineGvG ? "Tu as été incrit à la prochaine GvG" : $"Tu as été incrit à la GvG du {_gvg.Date}");
        }

        /// <summary>
        ///     Ajout des unites du compte de la GvG
        /// </summary>
        /// <returns></returns>
        [HttpPost("parametrerGvG")]
        public async Task<string> ParametrerGvG([FromBody] GvgCompteUniteImport _gvgUniteCompte)
        {
            gvgService.connectionString = config.GetConnectionString("defaut");
            await gvgService.ParametrerGvG(_gvgUniteCompte);

            return JsonConvert.SerializeObject("OK");
        }

        [HttpPost("absent")]
        public async Task<string> Absent([FromBody] GvgCompteImport _gvg)
        {
            gvgService.connectionString = config.GetConnectionString("defaut");
            await gvgService.Absent(_gvg.IdGvg, _gvg.IdCompte);

            return JsonConvert.SerializeObject(true);
        }

        [HttpPost("absentViaDiscord")]
        public async Task<string> AbsentViaDiscord(GvgCompteImport _gvg)
        {
            CompteService compteService = new(context);

            if(!compteService.IdDiscordExiste(_gvg.IdDiscord))
                return JsonConvert.SerializeObject("Je ne te connais pas");

            int idCompte = await compteService.GetIdCompte(_gvg.IdDiscord);

            if(DateTime.TryParse(_gvg.Date, out DateTime date))
            {
                if (!gvgService.Existe(date))
                    return JsonConvert.SerializeObject($"Aucune GvG pour le: {_gvg.Date}");

                int idGvg = await gvgService.GetIdGvG(date);

                await gvgService.Absent(idGvg, idCompte);

                return JsonConvert.SerializeObject("Tu ne participe plus à la GvG");
            }
            else
            {
                int idGvg = await gvgService.GetIdProchaineGvG();

                if (idGvg is 0)
                    return JsonConvert.SerializeObject("Aucune GvG prochainement");

                await gvgService.Absent(idGvg, idCompte);

                return JsonConvert.SerializeObject("Tu ne participe plus à la prochaine GvG");
            }
        }

        [HttpPost("existe")]
        public async Task<string> Existe(GvgImport _gvgImport)
        {
            DateTime dateTime = DateTime.Parse(_gvgImport.Date);

            if(gvgService.Existe(dateTime))
                return JsonConvert.SerializeObject(true);

            return JsonConvert.SerializeObject(false);
        }

        [HttpPost("supprimer/{idGvG}")]
        public async Task<string> Supprimer([FromRoute] int idGvG)
        {
            await gvgService.Supprimer(idGvG);

            return JsonConvert.SerializeObject(true);
        }

        [HttpPost("supprimerViaDiscord")]
        public async Task<string> Supprimer(GvgImport _gvgImport)
        {
            if(DateTime.TryParse(_gvgImport.Date, out DateTime date))
            {
                int idGvg = await gvgService.GetIdGvG(date);

                await gvgService.Supprimer(idGvg);

                return JsonConvert.SerializeObject(true);
            }
            else
            {
                int idGvg = await gvgService.GetIdProchaineGvG();

                if (idGvg is 0)
                    return JsonConvert.SerializeObject("Aucune GvG programmée prochainement");

                await gvgService.Supprimer(idGvg);

                return JsonConvert.SerializeObject("La prochaine GvG à été supprimée");
            }
        }
    }
}
