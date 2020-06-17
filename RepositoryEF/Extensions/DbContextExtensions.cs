using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace RepositoryEF.Extensions
{
    public static class DbContextExtensions
    {
        internal static IEnumerable<ScalarPropertyMapping> GetEntityPropertyMappings<TEntity>(this DbContext dbContext)
        {
            // Get the metadata
            MetadataWorkspace metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;

            // Get the space within the metadata which contains information about CLR types
            ObjectItemCollection clrSpace = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the metadata that maps to the CLR type
            EntityType entityEntityType = metadata.GetItems<EntityType>(DataSpace.OSpace).Single(e => clrSpace.GetClrType(e) == typeof(TEntity));

            // Get the entity set that uses this entity type
            EntitySet entityEntitySet = metadata.GetItems<EntityContainer>(DataSpace.CSpace).Single().EntitySets.Single(s => s.ElementType.Name == entityEntityType.Name);

            // Get the mapping between conceptual and storage model for this entity set
            EntitySetMapping entityEntitySetMapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace).Single().EntitySetMappings.Single(m => m.EntitySet == entityEntitySet);

            // Get the entity columns
            return entityEntitySetMapping.EntityTypeMappings.Single().Fragments.Single().PropertyMappings.OfType<ScalarPropertyMapping>();
        }
    }
}
