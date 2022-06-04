using Microsoft.Data.SqlClient;
using System.Data;


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
                                  NbParticipant = gvg.GvgComptes.Count
                              }).ToList();
            });

            return listeRetour;
        }

        public async Task<IEnumerable<dynamic>> ListerParametrer(DateTime _dateTime)
        {
            IEnumerable<dynamic> liste = null!;

            await Task.Run(() =>
            {
                liste = (from gvg in context.GvgComptes
                         where gvg.IdGvgNavigation.DateProgrammer.Equals(_dateTime)
                         orderby gvg.IdGroupe
                         select new
                         {
                             Nom = gvg.IdGroupeNavigation.Nom,
                             ListeJoueur = gvg.IdGroupeNavigation.GvgComptes.Select(g => new
                             {
                                 g.IdCompteNavigation.Pseudo,
                                 g.IdCompteNavigation.Influance,
                                 ListeUnite = gvg.IdCompteNavigation.GvgUniteComptes.Select(c => new { c.IdUniteNavigation.Nom, c.IdUniteNavigation.Influance })
                             })
                         }).AsEnumerable().GroupBy(c => c.Nom);
            });
   
            return liste;
        }

        public async Task<IQueryable> listerParticipant(int _idGvg)
        {
            IQueryable? liste = null;

            await Task.Run(() =>
            {
                liste = (from gvg in context.Gvgs
                         where gvg.Id == _idGvg
                         select new
                         {
                             gvg.Id,
                             Date = gvg.DateProgrammer.ToString("d"),
                             ListeCompte = gvg.GvgComptes.Select(compte =>
                             new
                             {
                                 Id = compte.IdCompte,
                                 compte.IdCompteNavigation.Pseudo,
                                 compte.IdCompteNavigation.Influance,
                                 compte.IdGroupe,
                                 ListeUnite = compte.IdCompteNavigation.UniteComptes.Select(unite =>
                                 new
                                 {
                                     Id = unite.IdUniteNavigation.Id,
                                     unite.IdUniteNavigation.Influance,
                                     unite.IdUniteNavigation.Nom,
                                     unite.NiveauMaitrise,
                                     EstDejaChoisi = gvg.GvgUniteComptes.Where(gvgUc => gvgUc.IdUnite == unite.IdUnite && gvgUc.IdCompte == compte.IdCompte && gvg.Id == _idGvg)
                                                                         .Select(gvg => gvg.IdUnite)
                                                                         .FirstOrDefault() != 0
                                 }),
                             })
                         });
            });

            return liste;
        }

        public async Task<IQueryable> ListerMesUnites(int _idGvg, int _idCompte)
        {
            IQueryable? liste = null;

            await Task.Run(() =>
            {
                liste = from gvg in context.GvgUniteComptes
                        where gvg.IdGvg == _idGvg && gvg.IdCompte == _idCompte
                        select new
                        {
                            gvg.IdUniteNavigation.Nom,
                            gvg.IdUniteNavigation.Influance
                        };
            });

            return liste;
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
                    where gvg.Id == _idGvg && gvg.GvgComptes.Where(c => c.IdCompte == _idCompte).Select(c => c.IdCompte).First() == _idCompte
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

        public async Task ParametrerGvG(GvgCompteUniteImport _gvgCompteUnite)
        {
            List<GvgUniteCompte> listeAjout = new();
            List<GvgUniteCompte> listeSupp = new();

            foreach (var element in _gvgCompteUnite.ListeUniteAjouter)
            {
                listeAjout.Add(new GvgUniteCompte { IdCompte = element.IdCompte, IdGvg = element.IdGvg, IdUnite = element.IdUnite });
            }

            foreach (var element in _gvgCompteUnite.ListeUniteAsupprimer)
            {
                listeSupp.Add(new GvgUniteCompte { IdCompte = element.IdCompte, IdGvg = element.IdGvg, IdUnite = element.IdUnite });
            }

            if (listeSupp.Count > 0)
                context.GvgUniteComptes.RemoveRange(listeSupp);

            if(listeAjout.Count > 0)
                await context.GvgUniteComptes.AddRangeAsync(listeAjout);

            await context.SaveChangesAsync();
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
