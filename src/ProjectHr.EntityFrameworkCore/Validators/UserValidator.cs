using FluentValidation;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Validators;

// public class UserValidator : AbstractValidator<User>
// {
//     public UserValidator()
//     {
//         RuleFor(user=> user.WorkEmailAddress ).
//             .WithMessage(" e-posta adresi zaten kullanılmaktadır. Lütfen başka bir e-posta adresi girin.");
//     }
// }