using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hair.Core.Models
{
    public class BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [JsonIgnore]
        public Guid LogicalRef { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime EditDate { get; set; }

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int EditById { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }
    }
}
