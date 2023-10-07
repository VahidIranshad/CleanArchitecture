using CA.Application.Contracts.Generic;
using CA.Application.DTOs.Ent.TValue.Validators;
using CA.Application.DTOs.Generic;

namespace CA.Application.DTOs.Ent.TValue
{
    public class TValueDto : BaseDto, IUpdateBaseDto, ICreateBaseDto, IDeleteBaseDto
    {
        public string Title { get; set; }

        IBaseValidation ICreateBaseDto.BaseValidation { get { return new CreateTValueValidatior(); } }

        IBaseValidation IUpdateBaseDto.BaseValidation { get { return new UpdateTValueValidatior(); } }

        IBaseValidation IDeleteBaseDto.BaseValidation { get { return new DeleteTValueValidatior(); } }
    }
}
