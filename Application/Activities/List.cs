using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>> { }
        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            // eğer datayı almak uzun sürecekse ve kullanıcının requesti iptal etme gibi bir ihtimali varsa
            // işlemi durdurmak için cancellationToken kullan
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                // datayı controller yerine burada çek gönder
                return await _context.Activities.ToListAsync(); // cancellationToken
            }
        }
    }
}