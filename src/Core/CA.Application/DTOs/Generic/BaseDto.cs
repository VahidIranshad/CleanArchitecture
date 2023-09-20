namespace CA.Application.DTOs.Generic
{
    public abstract class BaseDto : IDto, IBaseDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateStr
        {
            get
            {
                return CreateDate.ToString("yyyy-MM-dd");
            }
        }
        public DateTime LastEditDate { get; set; }
        public string LastEditDateStr
        {
            get
            {
                return LastEditDate.ToString("yyyy-MM-dd");
            }
        }
    }
}
