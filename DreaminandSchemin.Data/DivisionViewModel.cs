using System;
namespace DreaminandSchemin.Data
{
	public class DivisionViewModel
    {
        public int TournamentId { get; set; } = 0;
        public string TournamentName { get; set; } = string.Empty;
        public string DivisionName { get; set; } = string.Empty;
        public List<PoolModel>? Pools { get; set; } = null;

        public DivisionViewModel()
		{
		}
	}
}

