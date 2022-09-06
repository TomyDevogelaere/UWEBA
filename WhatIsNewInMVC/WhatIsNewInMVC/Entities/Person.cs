namespace WhatIsNewInMVC.Entities;

public class Person : EntityBase
{
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public Address Address { get; set; } = default!;
  public int AddressId { get; set; }
}
