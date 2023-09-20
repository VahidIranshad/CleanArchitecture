using AutoMapper;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using CA.Application.Exceptions;
using MediatR;

namespace CA.Application.Features.Generic.Commands
{

    public class DeleteBaseCommand<Z> : IRequest<Unit>
        where Z : Domain.Base.BaseEntity
    {
        public IDeleteBaseDto DeleteBaseDto { get; set; }
    }
    public class DeleteBaseCommandHandler<Z> : IRequestHandler<DeleteBaseCommand<Z>, Unit>
        where Z : Domain.Base.BaseEntity
    {
        private readonly IUnitOfWork<Z> _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteBaseCommandHandler(IUnitOfWork<Z> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteBaseCommand<Z> request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository().Get(request.DeleteBaseDto.Id);

            if (data == null)
                throw new NotFoundException(nameof(Z), request.DeleteBaseDto.Id);

            await _unitOfWork.Repository().Delete(data);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
