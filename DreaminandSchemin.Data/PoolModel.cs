using System;
namespace DreaminandSchemin.Data
{
	public class PoolModel
	{
		public string PoolName { get; set; } = string.Empty;
		public List<RoundModel>? Rounds {get; set; } = null;
	}
}

