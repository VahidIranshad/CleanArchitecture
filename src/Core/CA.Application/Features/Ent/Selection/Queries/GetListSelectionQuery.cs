using AutoMapper;
using CA.Application.Contracts.Ent;
using CA.Application.DTOs.Ent;
using CA.Domain.Base;
using MediatR;

namespace CA.Application.Features.Ent.Selection.Queries
{
    public class GetListSelectionQuery : IRequest<(List<SelectionDto>, int)>
    {
        public FopFilter filter { get; set; }
    }
    public class GetListSelectionQueryHandler : IRequestHandler<GetListSelectionQuery, (List<SelectionDto>, int)>
    {
        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        public GetListSelectionQueryHandler(ISelectionRepository selectionRepository, IMapper mapper)
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
        }
        public async Task<(List<SelectionDto>, int)> Handle(GetListSelectionQuery request, CancellationToken cancellationToken)
        {
            var (list, count) = await _selectionRepository.Get(request?.filter?.Filter, 
                request?.filter?.Order, 
                request?.filter?.PageNumber ?? 0, 
                request?.filter?.PageSize ?? 0, 
                request?.filter.DisableTracking);
            return (_mapper.Map<List<SelectionDto>>(list), count);
        }
    }
}
