using Abp.Domain.Repositories;
using FluentValidation;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Validators;

public class UserValidator : AbstractValidator<User>
{
    // private readonly IRepository<User, long> _userRepository;
    // public UserValidator(IRepository<User, long> userRepository)
    // {
    //     _userRepository = userRepository;
    //     
    //     RuleFor(x => x.WorkEmailAddress)
    //         .Must((user, email) => BeUnique(email))
    //         .WithMessage("This email is already registered.");
    //     
    // }
    // private bool BeUnique(string email)
    // {
    //     _userRepository.FirstOrDefaultAsync(u => u.WorkEmailAddress ==)
    //
    //     return true;
    // }
}