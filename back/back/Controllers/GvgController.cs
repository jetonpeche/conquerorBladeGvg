namespace back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GvgController : ControllerBase
    {
        private GvgService gvgService;

        public GvgController(conquerorBladeContext _context)
        {
            gvgService = new(_context);  
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
    }
}
