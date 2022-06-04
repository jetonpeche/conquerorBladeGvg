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

        [HttpGet("initIdDiscord/{pseudoCb}/{idDiscord}")]
        public async Task<string> InitIdDisocrd(string pseudoCb, string idDiscord)
        {
            if (!compte.Existe(pseudoCb))
                return JsonConvert.SerializeObject($"Je ne te connais pas {pseudoCb}");

            if(compte.IdDiscordExiste(idDiscord))
                return JsonConvert.SerializeObject($"Votre id discord est déjà parametré {pseudoCb}");

            await compte.InitIdDiscord(pseudoCb, idDiscord);

            return JsonConvert.SerializeObject($"je te connais {pseudoCb}");
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

        [HttpDelete("supprimer/{idCompte}")]
        public async Task Supprimer([FromRoute] int idCompte)
        {
            await compte.Supprimer(idCompte);
        }

        [HttpDelete("supprimerViaDiscord/{idDiscord}")]
        public async Task<string> Supprimer([FromRoute] string idDiscord)
        {
            if (!compte.IdDiscordExiste(idDiscord))
                return JsonConvert.SerializeObject(false);

            await compte.Supprimer(idDiscord);

            return JsonConvert.SerializeObject(true);
        }
    }
}
