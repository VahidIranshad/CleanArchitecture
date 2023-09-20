using AutoMapper;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using CA.Application.Exceptions;
using MediatR;

namespace CA.Application.Features.Generic.Commands
{

    public class UpdateBaseCommand<Z> : IRequest<Unit>
        where Z : Domain.Base.BaseEntity
    {
        public IUpdateBaseDto UpdateBaseDto { get; set; }
    }
    public class UpdateBaseCommandHandler<Z> : IRequestHandler<UpdateBaseCommand<Z>, Unit>
        where Z : Domain.Base.BaseEntity
    {

        private readonly IUnitOfWork<Z> _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBaseCommandHandler(IUnitOfWork<Z> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateBaseCommand<Z> request, CancellationToken cancellationToken)
        {
            var validationResult = await request.UpdateBaseDto.BaseValidation.ValidateAsync(request.UpdateBaseDto);

            if (validationResult.IsValid == false)
                throw new CustomValidationException(validationResult);

            var data = await _unitOfWork.Repository().Get(request.UpdateBaseDto.Id);

            if (data is null)
                throw new NotFoundException(nameof(data), request.UpdateBaseDto.Id);

            _mapper.Map(request.UpdateBaseDto, data);

            await _unitOfWork.Repository().Update(data);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
