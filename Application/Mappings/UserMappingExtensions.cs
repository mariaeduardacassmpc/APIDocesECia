using Application.Dtos.User;
using Data.Entities;

namespace Application.Mappings;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.UserId,
            Email = user.Email
        };
    }
}