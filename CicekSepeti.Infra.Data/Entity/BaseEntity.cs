using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CicekSepeti.Infra.Data.Entity
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
    public abstract class BaseEntityWithDate : BaseEntity
    {
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

    }
}
