using AutoMapper;
using CA.Application.Contracts.Ent;
using CA.Application.DTOs.Ent.Selection;
using MediatR;

namespace CA.Application.Features.Ent.Selection.Queries
{
    public class GetSelectionDetailQuery : IRequest<SelectionDto>
    {
        public int id { get; set; }
    }
    public class GetSelectionDetailQueryHandler : IRequestHandler<GetSelectionDetailQuery, SelectionDto>
    {

        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        public GetSelectionDetailQueryHandler(ISelectionRepository selectionRepository, IMapper mapper)
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
        }

        public async Task<SelectionDto> Handle(GetSelectionDetailQuery request, CancellationToken cancellationToken)
        {
            var data = await _selectionRepository.Get(request.id);
            return _mapper.Map<SelectionDto>(data);
        }
    }
}
