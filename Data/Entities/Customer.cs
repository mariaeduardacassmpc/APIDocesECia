namespace Data.Entidades
{
  public class Customer
  {
    public int CustomerId { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }
    public required bool Active { get; set; }
    public string? Email { get; set; }
    public string? Obs { get; set; }
  }
}

