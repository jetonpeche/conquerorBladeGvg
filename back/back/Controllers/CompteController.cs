namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompteController : ControllerBase
    {
        private readonly CompteService compte;
        private conquerorBladeContext context;
        public CompteController(conquerorBladeContext _context)
        {
            context = _context;

            compte = new(_context);
        }

        [HttpGet("lister")]
        public async Task<string> Lister()
        {
            var liste = await compte.Lister();

            return JsonConvert.SerializeObject(liste);
        }

        [HttpGet("connexion/{pseudo}")]
        public async Task<string> Connexion([FromRoute] string pseudo)
        {
            if (compte.Existe(pseudo))
            {
                var info = await compte.Info(pseudo);

                return JsonConvert.SerializeObject(info);
            }
            else
            {
                return JsonConvert.SerializeObject($"Le compte: '{pseudo}' n'existe pas");
            }
        }

        [HttpGet("estAdmin/{idDiscord}")]
        public async Task<string> EstAdmin(string idDiscord)
        {
            bool retour = await compte.EstAdmin(idDiscord);

            return JsonConvert.SerializeObject(retour);
        }

        [HttpPost("ajouter")]
        public async Task<string> Ajouter([FromBody] CompteImport _compte)
        {
            try
            {
                if (compte.Existe(_compte.Pseudo))
                    return JsonConvert.SerializeObject("Le pseudo existe déjà");

                if(compte.IdDiscordExiste(_compte.IdDiscord))
                    return JsonConvert.SerializeObject("L'ID discord existe déjà");

                Compte nouveauCompte = new()
                {
                    Pseudo = _compte.Pseudo,
                    Influance = _compte.Influance,
                    IdClasseHeros = _compte.IdClasseHeros,
                    IdDiscord = _compte.IdDiscord,
                    EstAdmin = _compte.EstAdmin,
                    EstPremiereConnexion = 1
                };

                await compte.Ajouter(nouveauCompte);

                return JsonConvert.SerializeObject(true);
            }
            catch (Exception e)
            {
                return JsonConvert.SerializeObject(e.Message);
            }
        }

        [HttpPut("modifier")]
        public async Task<string> Modifier([FromBody] CompteImportModif _compte)
        {
            List<UniteCompte> liste = new();

            foreach (var unite in _compte.ListeUniteNiv)
            {
                liste.Add(new() { IdCompte = _compte.IdCompte, IdUnite = unite.Id, NiveauMaitrise = unite.Niveau });
            }

            Compte compteModif = new()
            {
                Id = _compte.IdCompte,
                Pseudo = _compte.Pseudo,
                Influance = _compte.Influance,
                IdClasseHeros = _compte.IdClasseHeros,
                IdDiscord = _compte.IdDiscord,
                EstPremiereConnexion = 0
            };

            await compte.Modifier(compteModif);

            UniteService uniteService = new(context);
            await uniteService.AjouterUniteCompte(liste);

            return JsonConvert.SerializeObject(true);
        }

        [HttpGet("supprimer/{idCompte}")]
        public async Task<string> Supprimer([FromRoute] int idCompte)
        {
            await compte.Supprimer(idCompte);

            return JsonConvert.SerializeObject(true);
        }

        [HttpGet("supprimerViaDiscord/{idDiscord}")]
        public async Task<string> Supprimer([FromRoute] string idDiscord)
        {
            if (!compte.IdDiscordExiste(idDiscord))
                return JsonConvert.SerializeObject(false);

            await compte.Supprimer(idDiscord);

            return JsonConvert.SerializeObject(true);
        }
    }
}
