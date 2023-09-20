using AutoMapper;
using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Generic;
using CA.Application.Exceptions;
using MediatR;

namespace CA.Application.Features.Generic.Commands
{

    public class CreateBaseCommand<Z> : IRequest<int>
        where Z : Domain.Base.BaseEntity
    {

        public ICreateBaseDto CreateBaseDto { get; set; }
    }

    public class CreateBaseCommandHandler<Z> : IRequestHandler<CreateBaseCommand<Z>, int>
        where Z : Domain.Base.BaseEntity
    {

        private readonly IUnitOfWork<Z> _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBaseCommandHandler(IUnitOfWork<Z> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateBaseCommand<Z> request, CancellationToken cancellationToken)
        {
            var validationResult = await request.CreateBaseDto.BaseValidation.ValidateAsync(request.CreateBaseDto);

            if (validationResult.IsValid == false)
            {
                throw new CustomValidationException(validationResult);
            }
            else
            {
                var data = _mapper.Map<Z>(request.CreateBaseDto);

                data = await _unitOfWork.Repository().Add(data);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return data.Id;
            }

        }
    }
}
