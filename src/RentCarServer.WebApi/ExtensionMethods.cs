using GenericRepository;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.WebApi;

public static class ExtensionMethods
{
    public static async Task CreateFirstUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();

        var userRepository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();

        var unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (!(await userRepository.AnyAsync(q => q.Username.Value == "admin")))
        {
            FirstName firstName = new("Onur");
            LastName lastName = new("Abdulaji");
            Username username = new("admin");
            Email email = new("onurabdulaji@gmail.com");
            Password password = new("1");
            FullName fullName = new($"{firstName.Value} {lastName.Value} ({email.Value})");
            
            var user = new User(firstName, lastName, fullName, email, username, password);
            
            await userRepository.AddAsync(user);
            
            await unitOfWork.SaveChangesAsync();
        }
    }
}