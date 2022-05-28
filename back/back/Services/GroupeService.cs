using System.Data;
using Microsoft.Data.SqlClient;

namespace back.Services
{
    public class GroupeService
    {
        private conquerorBladeContext context;

        public GroupeService(conquerorBladeContext _context)
        {
            context = _context;
        }

        public async Task<IQueryable> Lister()
        {
            IQueryable? liste = null;

            await Task.Run(() =>
            {
                liste = from grp in context.Groupes
                        select new
                        {
                            grp.Id,
                            grp.Nom
                        };
            });

            return liste;
        }

        public async Task ModifierGroupeCompte(CompteGroupeImport _compteGroupe, string connexionString)
        {
            using(SqlConnection sqlCon = new(connexionString))
            {
                await sqlCon.OpenAsync();

                SqlCommand cmd = sqlCon.CreateCommand();

                cmd.CommandText = "UPDATE GvgCompte SET idGroupe = @idGrp WHERE idGvg = @idGvg AND idCompte = @idCompte";

                cmd.Parameters.Add("@idGrp", SqlDbType.Int).Value = _compteGroupe.IdGroupe;
                cmd.Parameters.Add("@idGvg", SqlDbType.Int).Value = _compteGroupe.IdGvg;
                cmd.Parameters.Add("@idCompte", SqlDbType.Int).Value = _compteGroupe.IdCompte;

                await cmd.PrepareAsync();
                await cmd.ExecuteNonQueryAsync();

                await sqlCon.CloseAsync();
            }
        }
    }
}
