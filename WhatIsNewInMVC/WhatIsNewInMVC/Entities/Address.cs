namespace WhatIsNewInMVC.Entities;

public class Address : EntityBase
{
  public string StreetName { get; set; } = default!;
  public string City { get; set; } = default!;
  public string HouseNumber { get; set; } = default!;
  public string PostalCode { get; set; } = default!;
}
