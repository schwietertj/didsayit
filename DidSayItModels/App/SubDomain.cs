using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DidSayItModels.App
{
    public class Subdomain : BaseEntity
    {
        [Required, MaxLength(63)]
        public string Name { get; set; }

        public ICollection<Content> Contents { get; set; }
    }
}
