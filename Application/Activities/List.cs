using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<Activity>>> { }
        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            // eğer datayı almak uzun sürecekse ve kullanıcının requesti iptal etme gibi bir ihtimali varsa
            // işlemi durdurmak için cancellationToken kullan
            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // datayı controller yerine burada çek gönder
                return Result<List<Activity>>.Success(await _context.Activities.ToListAsync()); 
            }
        }
    }
}