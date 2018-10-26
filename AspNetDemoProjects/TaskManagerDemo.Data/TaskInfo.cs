using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TaskManagerDemo.Data
{
    [Table("Task")]
    public class TaskInfo
    {
        public const int NameLengthMin = 3;

        public const int NameLengthMax = 50;

        public const string NameValidationRegex = @"[\w _-]{3,50}";

        public const string NameValidationMessage = "Name must be alphanumeric string from 3 to 50 characters long.";
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(NameValidationRegex, ErrorMessage = NameValidationMessage)]
        [StringLength(NameLengthMax, MinimumLength = NameLengthMin, ErrorMessage = NameValidationMessage)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(512)]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public Guid? ParentTaskId { get; set; }

        [Display(Name="Parent")]
        public virtual TaskInfo ParentTask { get; set; }

        //for concurrency management
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<TaskInfo> SubTasks { get; set; } = new HashSet<TaskInfo>();

        public virtual ICollection<ModifyInfo> ModifyInfos { get; set; } = new HashSet<ModifyInfo>();

        [NotMapped]
        public IEnumerable<ModifyInfo> ModifyInfosSorted { get { return ModifyInfos.OrderByDescending(o => o.Modified); } }

        [NotMapped]
        public IEnumerable<TaskInfo> SubTasksSorted { get { return SubTasks.OrderByDescending(o => o.Timestamp, new TimestampComparer()); } }

        private class TimestampComparer : Comparer<byte[]>
        {
            public override int Compare(byte[] x, byte[] y)
            {
                if (x == null && y == null)
                    return 0;

                if (x == null)
                {
                    return -1;
                }
                else if (y == null)
                {
                    return 1;
                }

                if (x == y || (x.Length == 0 && y.Length == 0))
                    return 0;

                for (int i = 0; i < x.Length; i++)
                    if (x[i] != y[i])
                        return x[i] > y[i] ? 1 : -1;

                return 0;
            }
        }
    }
}
