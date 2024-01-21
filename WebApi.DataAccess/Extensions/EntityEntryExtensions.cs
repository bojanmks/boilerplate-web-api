using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.DataAccess.Entities.Abstraction;

namespace WebApi.DataAccess.Extensions
{
    public static class EntityEntryExtensions
    {
        public static void OnAddedBehaviour(this EntityEntry entry)
        {
            if (entry.Entity is Entity entity)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }

        public static void OnDeletedBehaviour(this EntityEntry entry)
        {
            if (entry.Entity is ISoftDeletable softDeletableEntity)
            {
                entry.State = EntityState.Modified;
                softDeletableEntity.DeletedAt = DateTime.UtcNow;
                softDeletableEntity.IsActive = false;
            }
        }
    }
}
