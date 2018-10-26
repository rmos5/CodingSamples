using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerDemo.Data
{
    [Table("ModifyInfo")]
    public class ModifyInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid TaskId { get; set; }

        public virtual TaskInfo Task { get; set; }

        [Required]
        public DateTime Modified { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Modifier { get; set; }
    }
}
