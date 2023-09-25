using Microsoft.AspNetCore.Identity;

namespace CA.Identity.Models
{
    public class Role : IdentityRole
    {
        public Role() : base()
        {
            RoleClaims = new HashSet<RoleClaim>();
        }

        public Role(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<RoleClaim>();
            Description = roleDescription;
        }
        public Role(string roleName, string roleDescription, string userID) : this(roleName, roleDescription)
        {
            this.CreatedBy = this.LastModifiedBy = userID;
            this.CreatedOn = DateTime.Now;
            this.LastModifiedOn = DateTime.Now;
        }

        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    }
}
