using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidSayItModels
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool Active { get; set; } = true;
    }
}
