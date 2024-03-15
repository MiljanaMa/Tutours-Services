using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System.Security.Cryptography;

namespace Explorer.Stakeholders.Core.UseCases;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICrudRepository<Person> _personRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationService _emailService;

    public AuthenticationService(IUserRepository userRepository, ICrudRepository<Person> personRepository,
        ITokenGenerator tokenGenerator, IEmailVerificationService emailService)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _personRepository = personRepository;
        _emailService = emailService;
    }

    public Result<AuthenticationTokensDto> Login(CredentialsDto credentials)
    {
        var user = _userRepository.GetActiveByName(credentials.Username);
        if (user == null || credentials.Password != user.Password) return Result.Fail(FailureCode.NotFound);
        if (user.IsBlocked)
            return Result.Fail(FailureCode.Forbidden);
        if (!user.IsEnabled)
            return Result.Fail(FailureCode.Forbidden);

        long personId;
        try
        {
            personId = _userRepository.GetPersonId(user.Id);
        }
        catch (KeyNotFoundException)
        {
            personId = 0;
        }

        return _tokenGenerator.GenerateAccessToken(user, personId);
    }

    public Result<AuthenticationTokensDto> RegisterTourist(AccountRegistrationDto account)
    {
        if (_userRepository.Exists(account.Username)) return Result.Fail(FailureCode.NonUniqueUsername);

        try
        {
            var user = _userRepository.Create(new User(account.Username, account.Password, UserRole.Tourist, true,
                account.Email, false, CreateRandomToken()));
            _emailService.SendEmail(
             user.Email,
             "Potvrdite svoj email",
             $"Kliknite na sledeći link kako biste potvrdili svoj email: <a href='https://localhost:44333/api/administration/users/verify?token={user.VerificationToken}'>Potvrdi email</a>");

            var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email,
                account.ProfileImage, account.Biography, account.Quote, 0, 0, null));

            return _tokenGenerator.GenerateAccessToken(user, person.Id);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            // There is a subtle issue here. Can you find it?
        }
    }
    private string CreateRandomToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }
}