namespace ConfigLab.Models;

public class PeopleContext : DbContext
{
  public PeopleContext(DbContextOptions<PeopleContext> options)
   : base(options) { }

  public DbSet<Person> People { get; set; } = default!;
  public DbSet<Address> Addresses { get; set; } = default!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    EntityTypeBuilder<Person> personEntity = modelBuilder.Entity<Person>();
    personEntity.HasKey(person => person.Id);

    EntityTypeBuilder<Address> addressEntity = modelBuilder.Entity<Address>();
    addressEntity.HasKey(address => address.Id);

    personEntity.HasOne(person => person.Address);

    addressEntity.HasData(
      new Address
      {
        Id = 1,
        StreetName = "Carrettestraat",
        HouseNumber = "28",
        City = "Merksem",
        PostalCode = "2170"
      }, new Address
      {
        Id = 2,
        StreetName = "Mechelsesteenweg",
        HouseNumber = "107",
        City = "Antwerpen",
        PostalCode = "2018"
      }
      );

    personEntity.HasData(
      new Person
      {
        Id = 1,
        FirstName = "Jef",
        LastName = "Versmossen",
        AddressId = 1
      }, new Person
      {
        Id = 2,
        FirstName = "Joske",
        LastName = "Vermeulen",
        AddressId = 2
      }
      );
  }
}
