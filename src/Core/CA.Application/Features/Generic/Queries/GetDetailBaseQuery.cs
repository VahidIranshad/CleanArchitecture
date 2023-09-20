using AutoMapper;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using CA.Domain.Base;
using MediatR;

namespace CA.Application.Features.Generic.Queries
{
    public class GetDetailBaseQuery<X, Z> : IRequest<X>
    where X : IDto
    where Z : BaseEntity
    {
        public int id { get; set; }
    }
    public class GetDetailBaseQueryHandler<X, Z> : IRequestHandler<GetDetailBaseQuery<X, Z>, X>
        where X : IDto
        where Z : BaseEntity
    {

        private readonly IUnitOfWork<Z> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDetailBaseQueryHandler(IUnitOfWork<Z> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<X> Handle(GetDetailBaseQuery<X, Z> request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository().Get(request.id);
            return _mapper.Map<X>(data);
        }

    }
}
