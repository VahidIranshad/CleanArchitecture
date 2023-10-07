using CA.Application.DTOs.Ent.Selection;
using CA.Application.DTOs.Ent.Validators;

namespace UnitTest.DTOsValidation.Ent
{
    public class SelectionDtoValidation
    {
        private readonly SelectionCreateValidator _createValidator;
        private readonly SelectionUpdateValidator _updateValidator;

        public SelectionDtoValidation()
        {
            _createValidator = new SelectionCreateValidator();
            _updateValidator = new SelectionUpdateValidator();
        }

        [Theory]
        [MemberData(nameof(GetDataForCreate))]
        public async Task Applications_Validators_CreateData_ReturnsCorrectNumberOfErrors(SelectionCreateDto data, int numberExpectedErrors)
        {
            var validationResult = await _createValidator.ValidateAsync(data);
            Assert.Equal(numberExpectedErrors, validationResult.Errors.Count);
        }

        [Theory]
        [MemberData(nameof(GetDataForUpdate))]
        public async Task Applications_Validators_UpdateData_ReturnsCorrectNumberOfErrors(SelectionUpdateDto data, int numberExpectedErrors)
        {
            var validationResult = await _updateValidator.ValidateAsync(data);
            Assert.Equal(numberExpectedErrors, validationResult.Errors.Count);
        }


        public static IEnumerable<object[]> GetDataForCreate()
        {
            var allData = new List<object[]>
        {
            new object[] { new SelectionCreateDto { SelectionType="A", Title = "A", }, 0},
            new object[] { new SelectionCreateDto { SelectionType= null, Title = null, }, 4},
            new object[] { new SelectionCreateDto { SelectionType="", Title = "A"}, 1},
            new object[] { new SelectionCreateDto { SelectionType= "A", Title = ""}, 1},
            new object[] { new SelectionCreateDto { SelectionType="", Title = ""}, 2},
            new object[] { new SelectionCreateDto { SelectionType = "A", Title = String.Concat(Enumerable.Repeat("-", 201))}, 1},
            new object[] { new SelectionCreateDto { SelectionType = String.Concat(Enumerable.Repeat("-", 101)), Title = "A"}, 1},
        };

            return allData;
        }
        public static IEnumerable<object[]> GetDataForUpdate()
        {
            var allData = new List<object[]>
        {
            new object[] { new SelectionUpdateDto { SelectionType="A", Id = 1, Title = "A", }, 0},
            new object[] { new SelectionUpdateDto { SelectionType= null, Id = 1, Title = null, }, 4},
            new object[] { new SelectionUpdateDto { SelectionType="", Id = 1, Title = "A"}, 1},
            new object[] { new SelectionUpdateDto { SelectionType= "A", Id = 1, Title = ""}, 1},
            new object[] { new SelectionUpdateDto { SelectionType="", Id = 1, Title = ""}, 2},
            new object[] { new SelectionUpdateDto { SelectionType = "A", Id = 1, Title = String.Concat(Enumerable.Repeat("-", 201))}, 1},
            new object[] { new SelectionUpdateDto { SelectionType = String.Concat(Enumerable.Repeat("-", 101)), Id = 1, Title = "A"}, 1},
        };

            return allData;
        }
    }
}
