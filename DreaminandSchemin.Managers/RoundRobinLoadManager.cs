using System;
using DreaminandSchemin.Data;

namespace DreaminandSchemin.Managers
{
	public class RoundRobinLoadManager
	{
		private ApplicationDbContext _dbContext = null;
		public RoundRobinLoadManager(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public RoundRobinLoadModel Load(string? tournamentName)
		{
			var model = new RoundRobinLoadModel
			{
				Tournaments = _dbContext.Tournaments.ToList()
			};

			if (tournamentName != null)
			{
				var tournament = model.Tournaments.FirstOrDefault(x => x.TournamentName.CompareTo(tournamentName) == 0);
				model.SelectedId = tournament?.Id ?? 0;
			}
			return model;
		}
	}
}

