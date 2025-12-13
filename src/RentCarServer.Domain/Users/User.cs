using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.Domain.Users;

public sealed class User : Entity
{
    public User(FirstName firstName, LastName lastName, FullName fullName, Email email, Username username, Password password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Username = username;
        Password = password;
        FullName = new(FirstName.Value + " " + LastName.Value + " (" + Email.Value + ")");
    }

    private User()
    {
        
    }

    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Username Username { get; private set; } = default!;
    public Password Password { get; private set; } = default!;

    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }
}