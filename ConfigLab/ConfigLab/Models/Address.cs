namespace ConfigLab.Models;

public class Address
{
  public int Id { get; set; }
  public string StreetName { get; set; } = default!;
  public string City { get; set; } = default!;
  public string HouseNumber { get; set; } = default!;
  public string PostalCode { get; set; } = default!;
}
