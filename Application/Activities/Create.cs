using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        // command geriye bişey göndermez
        // Result<Unit> boş döndür demek
        public class Command : IRequest<Result<Unit>>
        {
            // argüman olarak tablo gelecek
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x=>x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);
                // eğer db de bişey değişmezse result false olucak yoksa true
                var result = await _context.SaveChangesAsync() > 0;

                if(!result){

                    return Result<Unit>.Failure("Failed to create activity");
                }

                // kod patlamasın diye boş bişey döndürüyorsun
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}