using AutoMapper;
using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Ent.Selection;
using CA.Application.DTOs.Ent.Validators;
using CA.Application.Exceptions;
using MediatR;

namespace CA.Application.Features.Ent.Selection.Commands
{
    public class UpdateSelectionCommand : IRequest<Unit>
    {
        public SelectionUpdateDto UpdateBaseDto { get; set; }
    }

    public class UpdateSelectionCommandHandler : IRequestHandler<UpdateSelectionCommand, Unit>
    {

        private readonly ISelectionRepository _selectionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<CA.Domain.Ent.Selection> _unitOfWork;

        public UpdateSelectionCommandHandler(IUnitOfWork<Domain.Ent.Selection> unitOfWork, ISelectionRepository selectionRepository, IMapper mapper)
        {
            _selectionRepository = selectionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateSelectionCommand request, CancellationToken cancellationToken)
        {
            var validator = new SelectionUpdateValidator();
            var validationResult = validator.Validate(request.UpdateBaseDto);


            if (validationResult.IsValid == false)
            {
                throw new CustomValidationException(validationResult);
            }
            else
            {
                var data = _mapper.Map<Domain.Ent.Selection>(request.UpdateBaseDto);
                await _selectionRepository.Update(data);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
