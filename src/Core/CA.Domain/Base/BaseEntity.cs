namespace CA.Domain.Base
{
    public abstract class BaseEntity
    {

        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatorID { get; set; }
        public DateTime LastEditDate { get; set; }
        public string LastEditorID { get; set; }
    }
}
