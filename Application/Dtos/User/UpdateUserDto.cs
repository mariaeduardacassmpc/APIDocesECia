namespace Application.Dtos.User;
public class UpdateUserDto
{
    public required string Email { get; set; }
    public string? Password { get; set; } 
}