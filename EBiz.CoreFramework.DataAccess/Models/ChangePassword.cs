//using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace EBiz.CoreFramework.DataAccess.Models
{
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string DeviceToken { get; set; }
    }

    //public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    //{
    //    public ChangePasswordValidator()
    //    {
    //        RuleFor(x => x.UserName).NotEmpty().WithMessage("Please enter user name.");
    //        RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter the password.");
    //        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please enter the confirmation password.");
    //        RuleFor(x => x).Custom((x, context) =>
    //        {
    //            if (x.Password != x.ConfirmPassword)
    //            {
    //                context.AddFailure(nameof(x.Password), "Passwords and Confirm Password does not match.");
    //            }
    //        });
    //        RuleFor(x => x.DeviceToken).NotEmpty();
    //    }
    //}
}
