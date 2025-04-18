﻿using WebApi.Common;

namespace WebApi.DataAccess.Entities.Abstraction
{
    public abstract class Entity : IIdentifyable
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
