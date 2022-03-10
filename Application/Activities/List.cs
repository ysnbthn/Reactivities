using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            // eğer datayı almak uzun sürecekse ve kullanıcının requesti iptal etme gibi bir ihtimali varsa
            // işlemi durdurmak için cancellationToken kullan
            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // automapper ile projection yap
                var query = _context.Activities
                // şimdiki zamandan başla
                .Where(d=>d.Date >= request.Params.StartDate)
                .OrderBy(d => d.Date)
                // anon object yapıp kullanıcı ismini mappera yolla
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
                // queryable olarak yap database kısmına yollama
                .AsQueryable();

                // Sadece kullanıcının gittiği aktiviteleri getir
                if(request.Params.IsGoing && !request.Params.IsHost){
                    query = query.Where(x=>x.Attendees.Any(a=>a.Username == _userAccessor.GetUserName()));
                }
                // sadece kullanıcının host olduklarını getir
                if(request.Params.IsHost && !request.Params.IsGoing){
                    query = query.Where(x=>x.HostUsername == _userAccessor.GetUserName());
                }

                // datayı controller yerine burada çek gönder
                return Result<PagedList<ActivityDto>>.Success(
                    // queryi database'e yolla 
                    await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}