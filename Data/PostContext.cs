using Microsoft.EntityFrameworkCore;
using DemoBoard.Models;

namespace DemoBoard.Data
{
	public class PostContext : DbContext
	{
		public PostContext( DbContextOptions<PostContext> options )
			: base( options )
		{
		}

		public DbSet<PostModel> Post { get; set; }
	}
}
