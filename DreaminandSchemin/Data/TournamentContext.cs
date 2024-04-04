using System;
using Microsoft.EntityFrameworkCore;

namespace DreaminandSchemin.Data
{
	public class TournamentContext : DbContext
	{

        public TournamentContext(DbContextOptions<TournamentContext> options) : base(options)
		{

		}

        public DbSet<Tournament> Tournaments { get; set; }
	}
}