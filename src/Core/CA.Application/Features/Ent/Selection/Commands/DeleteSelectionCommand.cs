using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Generic;
using CA.Application.Exceptions;
using MediatR;


namespace CA.Application.Features.Ent.Selection.Commands
{
    public class DeleteSelectionCommand: IRequest<Unit>
    {
        public int id { get; set; }
    }
    public class DeleteSelectionCommandHandler : IRequestHandler<DeleteSelectionCommand, Unit>
    {
        private readonly ISelectionRepository _selectionRepository;
        private readonly IUnitOfWork<CA.Domain.Ent.Selection> _unitOfWork;
        public DeleteSelectionCommandHandler(IUnitOfWork<Domain.Ent.Selection> unitOfWork, ISelectionRepository selectionRepository)
        {
            _selectionRepository = selectionRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Unit> Handle(DeleteSelectionCommand request, CancellationToken cancellationToken)
        {
            var data = await _selectionRepository.Get(request.id);

            if (data == null)
                throw new NotFoundException(nameof(Selection), request.id);

            await _selectionRepository.Delete(request.id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

