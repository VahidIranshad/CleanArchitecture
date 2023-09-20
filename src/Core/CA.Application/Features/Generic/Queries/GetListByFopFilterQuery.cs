using AutoMapper;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using CA.Domain.Base;
using MediatR;

namespace CA.Application.Features.Generic.Queries
{
    public class GetListByFopFilterQuery<X, Z> : IRequest<(List<X>, int)>
        where X : IDto
        where Z : BaseEntity
    {
        public FopFilter filter { get; set; }
    }

    public class GetListByFopFilterQueryHandler<X, Z> : IRequestHandler<GetListByFopFilterQuery<X, Z>, (List<X>, int)>
        where X : IDto
        where Z : BaseEntity
    {


        private readonly IUnitOfWork<Z> _unitOfWork;
        private readonly IMapper _mapper;

        public GetListByFopFilterQueryHandler(IUnitOfWork<Z> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(List<X>, int)> Handle(GetListByFopFilterQuery<X, Z> request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository();

            var (datas, totalCount) = await repository.Get(request?.filter?.Filter, request?.filter?.Order, request?.filter?.PageNumber ?? 0, request?.filter?.PageSize ?? 0, request?.filter.DisableTracking);
            var mapDatas = _mapper.Map<List<X>>(datas.ToList());
            return (mapDatas, totalCount);
        }
    }
}
