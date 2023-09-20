using AutoMapper;
using CA.Application.Contracts.Ent;
using CA.Application.DTOs.Ent;
using CA.Application.DTOs.Ent.Validators;
using CA.Application.Exceptions;
using MediatR;

namespace CA.Application.Features.Ent.Selection.Commands
{
    public class CreateSelectionCommand : IRequest<int>
    {
        public SelectionCreateDto CreateBaseDto { get; set; }
    }

    public class CreateSelectionCommandHandler : IRequestHandler<CreateSelectionCommand, int>
    {

        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;

        public CreateSelectionCommandHandler(ISelectionRepository selectionRepository, IMapper mapper)
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSelectionCommand request, CancellationToken cancellationToken)
        {
            var validator = new SelectionCreateValidator();
            var validationResult = validator.Validate(request.CreateBaseDto);


            if (validationResult.IsValid == false)
            {
                throw new CustomValidationException(validationResult);
            }
            else
            {
                var data = await _selectionRepository.Add(request.CreateBaseDto);
                //await _unitOfWork.SaveChangesAsync(cancellationToken);

                return data.Id;
            }
        }
    }
}
