using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DidSayItModels.App
{
    public class Link : BaseEntity
    {
        public long ContentId { get; set; }

        [Required, MaxLength(1024), DataType(DataType.Url)]
        public string Url { get; set; }
        [Required, MaxLength(1024)]
        public string LinkText { get; set; }
        public string Notes { get; set; }

        [ForeignKey("ContentId")]
        public Content Content { get; set; }
    }
}
