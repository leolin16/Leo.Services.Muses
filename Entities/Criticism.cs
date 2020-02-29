using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Leo.Services.Muses.Entities
{
    public class Criticism
    {
        public Criticism()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Critic { get; set; }
        [Required]
        public string Opinion { get; set; }
        public int SingerId { get; set; }
        public virtual Singer Singer { get; set; }
    }
}
