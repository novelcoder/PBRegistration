using System;
namespace RoundRobin
{
	public class PoolDistribution
	{
		private static string[,] fourTeam = new string[3,2]
			{ { "1-2", "3-4" },
			  { "1-3", "2-4" },
			  { "1-4", "2-3" } 
            };

		private static string[,] sixTeam = new string[5,3]
			{   {"1-2", "3-4", "5-6" },
				{"1-3", "2-5", "4-6" },
				{"1-4", "2-6", "3-5" },
				{"1-5", "2-3", "4-6" },
				{"1-6", "2-5", "3-4" }
			};


        private static string[,] eightTeam = new string[7,4]
            {   {"1-2", "3-4", "5-6", "7-8" },
                {"1-3", "2-4", "5-7", "6-8" },
                {"1-4", "2-3", "5-8", "6-7" },
                {"1-5", "2-6", "3-7", "4-8" },
                {"1-6", "2-5", "3-8", "4-7" },
                {"1-7", "2-8", "3-5", "4-6" },
                {"1-8", "2-7", "3-6", "4-5" }
            };

        public PoolDistribution()
		{
		}

		public static List<List<Match>> MatchesByRound(List<string> teams)
		{
			var result = new List<List<Match>>();

			// if odd number, add "bye"
			if (teams.Count() % 2 == 1)
			{
				teams.Add("bye");
			}

			switch (teams.Count())
			{
				case 4:
					result = DistributeGames(teams, fourTeam);
					break;
				case 6:
                    result = DistributeGames(teams, sixTeam);
                    break;
				case 8:
                    result = DistributeGames(teams, eightTeam);
                    break;
				default:
					throw new Exception($"Unable to process {teams.Count()} teams.");
			}

			return result;
		}

		private static List<List<Match>> DistributeGames(List<string> teams, string[,] distro)
		{
			var result = new List<List<Match>>();
			var teamArray = teams.ToArray();

			for (int row = 0; row < distro.GetLength(0); row++)
			{
				var round = new List<Match>();
				for (int col = 0; col < distro.GetLength(1); col++)
				{
					var games = distro[row, col].Split('-');
					int leftGame = 0;
					int rightGame = 0;
					int.TryParse(games[0], out leftGame);
					int.TryParse(games[1], out rightGame);

					var match = new Match() { LeftTeam = teamArray[leftGame - 1], RightTeam = teamArray[rightGame - 1] };
					round.Add(match);
				}
				result.Add(round);
			}

			return result;
		}


        public static List<List<Match>>[] CalcMatches(List<string> rawRows, ref string sheetName, ref string bracketName)
        {
            int row = 0;
            int numPools = 1;
            var teamList = new List<string>();

            foreach (var item in rawRows)
            {
                string? poolValue = FindKeyValue(item, "pools:");
                string? sheetValue = FindKeyValue(item, "sheet:");

                if (row++ == 0)
                    bracketName = item;
                else if (poolValue != null)
                    int.TryParse(poolValue, out numPools);
                else if (sheetValue != null)
                    sheetName = sheetValue.Trim();
                else
                    teamList.Add(item);
            }

            // get a separate list of each pool
            var maxTeamsPerPool = MaxTeamsPerPool(teamList.Count, numPools);
            var pools = new List<List<string>>();
            var currentPool = new List<string>();
            foreach (var team in teamList)
            {
                currentPool.Add(team);
                if (currentPool.Count() >= maxTeamsPerPool)
                {
                    pools.Add(currentPool);
                    currentPool = new List<string>();
                }
            }

            if (currentPool.Count > 0)
                pools.Add(currentPool);

            // print out pools for debug
            //int poolNum = 1;
            //foreach (var pool in pools)
            //{
            //    Console.WriteLine($"Pool {poolNum++}");
            //    foreach ( var team in pool)
            //    {
            //        Console.WriteLine($"\t{team}");
            //    }
            //}

            var result = new List<List<Match>>[pools.Count];
            int poolNum = 0;
            foreach (var pool in pools)
            {
                result[poolNum++] = PoolDistribution.MatchesByRound(pool);
            }

            return result;
        }



        public static int MaxTeamsPerPool(int numTeams, int numPools)
        {
            int remainder = numTeams % numPools;
            if (remainder == 0) // even division, just return the number
                return numTeams / numPools;

            // otherwise, the max is one more than simple division
            return (numTeams / numPools) + 1;
        }

        private static string? FindKeyValue(string value, string key)
        {
            string? result = null;
            if (!string.IsNullOrWhiteSpace(value)
              && !string.IsNullOrWhiteSpace(key))
            {
                if (value.ToLower().Trim().IndexOf(key) == 0)
                {
                    result = value.Trim().Substring(key.Length).Trim();
                }
            }

            return result;
        }
    }
}

