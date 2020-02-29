using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Leo.Services.Muses.Models
{
    public class CriticismDtoForEdit
    {
        public CriticismDtoForEdit()
        {
        }
        [MaxLength(50)]
        public string Critic { get; set; }
        [Required]
        public string Opinion { get; set; }

    }
}
