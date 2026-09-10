namespace ApiDoces.Dtos.Customer;

public class UpdateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty; 
    public string Obs { get; set; } = string.Empty;
    public bool Active { get; set; }
}