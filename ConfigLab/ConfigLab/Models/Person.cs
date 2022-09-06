namespace ConfigLab.Models;

public class Person
{
  public int Id { get; set; }
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public Address Address { get; set; } = default!;
  public int AddressId { get; set; }
}
