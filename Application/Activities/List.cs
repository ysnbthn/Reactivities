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
            public PagingParams Params { get; set; }
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
                .OrderBy(d => d.Date)
                // anon object yapıp kullanıcı ismini mappera yolla
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUserName() })
                // queryable olarak yap database kısmına yollama
                .AsQueryable();

                // datayı controller yerine burada çek gönder
                return Result<PagedList<ActivityDto>>.Success(
                    // queryi database'e yolla 
                    await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}