using AutoMapper;
using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Ent.Selection;
using CA.Application.DTOs.Ent.Validators;
using CA.Application.Exceptions;
using MediatR;

namespace CA.Application.Features.Ent.Selection.Commands
{
    public class CreateSelectionCommand : IRequest<int>
    {
        public SelectionCreateDto model { get; set; }
    }

    public class CreateSelectionCommandHandler : IRequestHandler<CreateSelectionCommand, int>
    {

        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<CA.Domain.Ent.Selection> _unitOfWork;

        public CreateSelectionCommandHandler(IUnitOfWork<Domain.Ent.Selection> unitOfWork, 
            ISelectionRepository selectionRepository, 
            IMapper mapper)
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSelectionCommand request, CancellationToken cancellationToken)
        {
            var validator = new SelectionCreateValidator();
            var validationResult = validator.Validate(request.model);


            if (validationResult.IsValid == false)
            {
                throw new CustomValidationException(validationResult);
            }
            else
            {
                var data = _mapper.Map<Domain.Ent.Selection>(request.model);
                var result = await _selectionRepository.Add(data);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Id;
            }
        }
    }
}
