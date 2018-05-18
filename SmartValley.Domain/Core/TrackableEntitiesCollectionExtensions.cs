using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartValley.Domain.Core
{
    public static class TrackableEntitiesCollectionExtensions
    {
        public static void Merge<T>(this ICollection<T> sourceCollection, IReadOnlyCollection<T> updatedCollection) where T : Entity, IUpdateble<T>
        {
            RemoveDeleted(sourceCollection, updatedCollection);
            InsertCreated(sourceCollection, updatedCollection);
            UpdateModified(sourceCollection, updatedCollection);
        }
        
        private static void RemoveDeleted<T>(ICollection<T> sourceCollection, IReadOnlyCollection<T> updatedCollection)
        {
            var removedItems = sourceCollection.Except(updatedCollection).ToArray();

            foreach (var removedItem in removedItems)
            {
                sourceCollection.Remove(removedItem);
            }
        }

        private static void InsertCreated<T>(ICollection<T> sourceCollection, IReadOnlyCollection<T> updatedCollection) where T : Entity
        {
            foreach (var insertedMember in updatedCollection.Where(x => x.IsTransient()))
            {
                sourceCollection.Add(insertedMember);
            }
        }

        private static void UpdateModified<T>(ICollection<T> sourceCollection, IReadOnlyCollection<T> updatedCollection) where T : Entity, IUpdateble<T>
        {
            var updated = updatedCollection.Where(x => !x.IsTransient());

            foreach (var updatedMember in updated)
            {
                var existingMember = sourceCollection.FirstOrDefault(m => m.Equals(updatedMember));
                if (existingMember == null)
                {
                    throw new InvalidOperationException($"Can't find updated entity '{typeof(T)}' with Id='{updatedMember.Id}' in source collection.");
                }
                existingMember.Update(updatedMember);
            }
        }
    }
}
