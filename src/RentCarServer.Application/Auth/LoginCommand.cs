using FluentValidation;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record LoginCommand(string EmailOrUsername, string Password) : IRequest<Result<string>>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(q => q.EmailOrUsername).NotEmpty().WithErrorCode("Gecerli Bir Email Yada Kullanici Adi Girin");
        RuleFor(q => q.Password).NotEmpty().WithErrorCode("Gecerli Bir Sifre Girin");
    }
}

public sealed class LoginCommandHandler(IUserRepository userRepository , IJwtProvider jwtProvider) : IRequestHandler<LoginCommand,Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(q =>
            q.Email.Value == request.EmailOrUsername || q.Username.Value == request.EmailOrUsername);

        if (user is null)
        {
            return Result<string>.Failure("Kullanici Yada Sifre Yanlis");
        }

        var checkPassword = user.VerifyPasswordHash(request.Password);

        if (!checkPassword)
        {
            return Result<string>.Failure("Kullanici Yada Sifre Yanlis");
        }

        var token = jwtProvider.CreateToken(user);

        return token;
    }
}