using System.Data;
using Microsoft.Data.SqlClient;


namespace back.Services
{
    public class GvgService
    {
        public string connectionString;
        private conquerorBladeContext context;

        public GvgService(conquerorBladeContext _context)
        {
            context = _context;
        }

        public bool Existe(DateTime _date)
        {
            int t = (from gvg in context.Gvgs
                     where gvg.DateProgrammer.Equals(_date)
                     select gvg.Id).FirstOrDefault();

            return t != 0;
        }

        public async Task<List<GvgExport>> Lister()
        {
            List<GvgExport>? listeRetour = null;

            await Task.Run(() =>
            {
                listeRetour = (from gvg in context.Gvgs
                              orderby gvg.DateProgrammer
                              select new GvgExport
                              {
                                  Id = gvg.Id,
                                  Date = gvg.DateProgrammer.ToString("dd/MM/yyyy"),
                                  NbParticipant = gvg.IdComptes.Count
                              }).ToList();
            });

            return listeRetour;
        }

        public async Task<int> GetIdGvG(DateTime _date)
        {
            int id = (from gvg in context.Gvgs
                     where gvg.DateProgrammer.Equals(_date)
                     select gvg.Id).FirstOrDefault();

            return id;
        }

        public bool Participe(int _idCompte, int _idGvg)
        {
            int t = (from gvg in context.Gvgs
                    where gvg.Id == _idGvg && gvg.IdComptes.Where(c => c.Id == _idCompte).Select(c => c.Id).First() == _idCompte
                    select gvg.Id).FirstOrDefault();

            
            return t != 0;
        }

        public async Task<List<int>> Ajouter(List<Gvg> _gvg)
        {
            List<int> listeRetour = new();

            foreach (var element in _gvg)
            {
                await context.Gvgs.AddAsync(element);
                await context.SaveChangesAsync();

                int id = context.Gvgs.OrderByDescending(x => x.Id).Select(g => g.Id).First();

                listeRetour.Add(id);
            }
            
            return listeRetour;
        }

        public async Task Participer(int _idGvg, int _idCompte)
        {
            using(SqlConnection sqlCon = new(connectionString))
            {
                await sqlCon.OpenAsync();

                SqlCommand cmd = sqlCon.CreateCommand();
                cmd.CommandText = "INSERT INTO GvgCompte (idGvg, idCompte) VALUES (@idGvg, @idCompte)";

                cmd.Parameters.Add("@idGvg", SqlDbType.Int).Value = _idGvg;
                cmd.Parameters.Add("@idCompte", SqlDbType.Int).Value = _idCompte;

                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();

                await sqlCon.CloseAsync();
            }
        }

        public async Task Absent(int _idGvg, int _idCompte)
        {
            using (SqlConnection sqlCon = new(connectionString))
            {
                await sqlCon.OpenAsync();

                SqlCommand cmd = sqlCon.CreateCommand();
                cmd.Parameters.Add("@idGvg", SqlDbType.Int).Value = _idGvg;
                cmd.Parameters.Add("@idCompte", SqlDbType.Int).Value = _idCompte;

                cmd.CommandText = "DELETE FROM GvgUniteCompte WHERE idGvg = @idGvg AND idCompte = @idCompte";
                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "DELETE FROM GvgCompte WHERE idGvg = @idGvg AND idCompte = @idCompte";
                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();

                await sqlCon.CloseAsync();
            }
        }
    }
}
