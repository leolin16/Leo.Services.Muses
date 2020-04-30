using System;


namespace Leo.Services.Muses.Entities
{
    public abstract class BaseEntity
    {
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}