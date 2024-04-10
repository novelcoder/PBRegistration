using RoundRobin;
using DreaminandSchemin.Data;
using Google.Apis.Sheets.v4.Data;

namespace DreaminandSchemin.Managers
{
	public class RoundRobinManager
	{
		private ApplicationDbContext _dbContext = null;
		public RoundRobinManager(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public DivisionListModel LoadDivisionList(int tournamentId)
		{
			var result = new DivisionListModel {  TournamentId = tournamentId };

            var tournament = _dbContext.Tournaments.FirstOrDefault(x => x.Id == tournamentId);


            if (tournament != null )
            {
				result.TournamentName = tournament.TournamentName;
                var divisionModels = LoadDivisionModel(tournament.BracketSheetName);
				foreach ( var division in divisionModels)
				{
					result.Divisions.Add(division.DivisionName);
				}
            }
			return result;
        }

		public TournamentListModel LoadTournaments()
		{
			var model = new TournamentListModel
			{
				Tournaments = _dbContext.Tournaments.ToList()
			};
			return model;
		}

		public DivisionViewModel? LoadDivisionViewModel(int tournamentId, string divisionName)
		{
			DivisionViewModel? result = null;

			var tournament = _dbContext.Tournaments.FirstOrDefault(x => x.Id == tournamentId);
			if (tournament != null)
			{
				result = new DivisionViewModel
					{
						TournamentId = tournamentId,
						TournamentName = tournament.TournamentName ?? string.Empty,
						DivisionName = divisionName
					};

                var rdr = new PoolReader();
				string sheetName = string.Empty;
				string currentDivisionName = string.Empty;
                var divisions = rdr.ReadSheet(tournament.BracketSheetName ?? string.Empty);
				foreach ( var division in divisions)
                {
                    var poolsForDivision = PoolDistribution.CalcMatches(division, ref sheetName, ref currentDivisionName);
                    if ( currentDivisionName.CompareTo(divisionName) == 0)
					{
						var divisionModel = PoolDistribution.LoadDivisionModel(poolsForDivision, divisionName);
						result.Pools = divisionModel.Pools;
					}
				}
            }

			return result;
        }

		public List<DivisionModel> LoadDivisionModel(string sheetId)
		{
			var result = new List<DivisionModel>();
			string sheetName = string.Empty;
			string divisionName = string.Empty;

            var rdr = new PoolReader();
            var divisions = rdr.ReadSheet(sheetId);
			foreach (var division in divisions)
			{
				var poolsForDivision = PoolDistribution.CalcMatches(division, ref sheetName, ref divisionName);
				var divisionModel = PoolDistribution.LoadDivisionModel(poolsForDivision, divisionName);
				result.Add(divisionModel);
			}

            return result;
		}
	}
}

