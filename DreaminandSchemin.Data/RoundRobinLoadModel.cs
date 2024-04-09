using System;

namespace DreaminandSchemin.Data
{
	public class RoundRobinLoadModel
    {
		public List<Tournament>? Tournaments { get; set; } = null;
		public int SelectedId { get; set; } = 0;
		public List<string>? Divisions { get; set; } = null;
		public List<DivisionModel>? DivisionModels { get; set; } = null;
	}
}