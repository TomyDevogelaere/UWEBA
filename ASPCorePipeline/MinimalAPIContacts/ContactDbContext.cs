using Microsoft.EntityFrameworkCore;

namespace MinimalAPIContacts;

public class ContactDbContext: DbContext
{
    public DbSet<Contact> Contacts => Set<Contact>();

	public ContactDbContext(DbContextOptions options) : base(options)
	{

	}
}
