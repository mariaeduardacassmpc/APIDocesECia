namespace Data.Entities;

public class User
{
    public int UserId { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
