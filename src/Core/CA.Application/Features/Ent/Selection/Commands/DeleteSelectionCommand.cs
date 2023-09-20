using CA.Application.Contracts.Ent;
using CA.Application.Exceptions;
using MediatR;


namespace CA.Application.Features.Ent.Selection.Commands
{
    public class DeleteSelectionCommand: IRequest<Unit>
    {
        public int id { get; set; }
    }
    public class DeleteSelectionCommandHandler<Z> : IRequestHandler<DeleteSelectionCommand, Unit>
    {
        private readonly ISelectionRepository _selectionRepository;
        public DeleteSelectionCommandHandler(ISelectionRepository selectionRepository)
        {
            _selectionRepository = selectionRepository;
        }


        public async Task<Unit> Handle(DeleteSelectionCommand request, CancellationToken cancellationToken)
        {
            var data = await _selectionRepository.Get(request.id);

            if (data == null)
                throw new NotFoundException(nameof(Z), request.id);

            await _selectionRepository.Delete(request.id);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

