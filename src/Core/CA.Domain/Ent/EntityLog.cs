using CA.Domain.Base;

namespace CA.Domain.Ent
{
    public class EntityLog : BaseEntity
    {
        public string ClassName { get; set; }
        public string ChangeType { get; set; }
        public string Key { get; set; }
    }
}
