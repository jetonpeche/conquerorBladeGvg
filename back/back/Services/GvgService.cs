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

        public async Task<IQueryable> Lister()
        {
            IQueryable? listeRetour = null;

            await Task.Run(() =>
            {
                listeRetour = from gvg in context.Gvgs
                              orderby gvg.DateProgrammer
                              select new
                              {
                                  gvg.Id,
                                  Date = gvg.DateProgrammer.ToString("dd/MM/yyyy"),
                                  NbParticipant = gvg.IdComptes.Count
                              };
            });

            return listeRetour;
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
