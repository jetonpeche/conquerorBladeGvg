using Microsoft.AspNetCore.Http;
using back;

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

        [HttpPost("ajouter")]
        public async Task<string> Ajouter([FromBody] CompteImport _compte)
        {
            Compte nouveauCompte = new()
            {
                Pseudo = _compte.Pseudo,
                Influance = _compte.Influance,
                IdClasseHeros = _compte.IdClasseHeros
            };

            int id = await compte.Ajouter(nouveauCompte);

            return JsonConvert.SerializeObject(id);
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
    }
}
