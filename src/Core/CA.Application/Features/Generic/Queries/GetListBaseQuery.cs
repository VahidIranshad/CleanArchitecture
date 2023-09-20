using AutoMapper;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using CA.Domain.Base;
using MediatR;

namespace CA.Application.Features.Generic.Queries
{
    public class GetListBaseQuery<X, Z> : IRequest<List<X>>
        where X : IDto
        where Z : BaseEntity
    {
        public Filter<Z> filter { get; set; }
    }
    public class GetListBaseQueryHandler<X, Z> : IRequestHandler<GetListBaseQuery<X, Z>, List<X>>
        where X : IDto
        where Z : BaseEntity
    {

        private readonly IUnitOfWork<Z> _unitOfWork;
        private readonly IMapper _mapper;

        public GetListBaseQueryHandler(IUnitOfWork<Z> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<X>> Handle(GetListBaseQuery<X, Z> request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository();
            var list = await repository.Get(request?.filter?.predicate,
                                            request?.filter?.orderBy,
                                            request?.filter?.includes,
                                            request?.filter?.disableTracking,
                                            request?.filter?.paging);
            return _mapper.Map<List<X>>(list);
        }
    }
}
