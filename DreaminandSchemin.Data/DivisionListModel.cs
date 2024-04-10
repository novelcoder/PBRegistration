using System;
namespace DreaminandSchemin.Data
{
	public class DivisionListModel
	{
		public int TournamentId { get; set; } = 0;
		public string TournamentName { get; set; } = string.Empty;
		public List<string> Divisions { get; set; } = new List<string>();
	}
}

