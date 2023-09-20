using AutoMapper;
using CA.Application.Contracts.Ent;
using CA.Application.DTOs.Ent;
using CA.Domain.Base;
using MediatR;

namespace CA.Application.Features.Ent.Selection.Queries
{
    public class GetListSelectionQuery : IRequest<List<SelectionDto>>
    {
        public Filter<Domain.Ent.Selection> filter { get; set; }
    }
    public class GetListSelectionQueryHandler : IRequestHandler<GetListSelectionQuery, List<SelectionDto>>
    {
        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        public GetListSelectionQueryHandler(ISelectionRepository selectionRepository, IMapper mapper)
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
        }
        public async Task<List<SelectionDto>> Handle(GetListSelectionQuery request, CancellationToken cancellationToken)
        {
            var list = await _selectionRepository.Get(request?.filter?.predicate,
                                          request?.filter?.orderBy,
                                          request?.filter?.includes,
                                          request?.filter?.disableTracking,
                                          request?.filter?.paging);
            return _mapper.Map<List<SelectionDto>>(list);
        }
    }
}
