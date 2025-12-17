using FluentValidation;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record ForgotPasswordCommand(string Email) : IRequest<Result<string>>;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(q => q.Email)
            .NotEmpty()
            .WithMessage("Gecerli bir email adresi giriniz!")
            .EmailAddress().WithMessage("Gecerli bir email adresi giriniz!");
    }
}

internal sealed class ForgotPasswordCommandHandler
    (IUserRepository userRepository)
    : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(q => q.Email.Value == request.Email, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanici Bulunamadi");
        }
        
        // Sifre sifirlama maili Gonder

        return "Sifre Sifirlama Gonderilmistir Lutfen Email Adresinizi Gonderiniz";
    }
}