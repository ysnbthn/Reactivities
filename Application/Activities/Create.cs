using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        // command geriye bişey göndermez
        public class Command : IRequest
        {
            // argüman olarak tablo gelecek
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);
                await _context.SaveChangesAsync();
                // kod patlamasın diye boş bişey döndürüyorsun
                return Unit.Value;
            }
        }
    }
}