using FluentValidation;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficultyRequest: AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public AddWalkDifficultyRequest()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
