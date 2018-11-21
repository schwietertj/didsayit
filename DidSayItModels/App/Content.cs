using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidSayItModels.App
{
    public class Content : BaseEntity
    {
        public long SubdomainId { get; set; }
        [Required]
        public string Text { get; set; }
        public string Notes { get; set; }

        [ForeignKey("SubdomainId")]
        public Subdomain Subdomain { get; set; }

        public ICollection<Link> Links { get; set; }
    }
}
