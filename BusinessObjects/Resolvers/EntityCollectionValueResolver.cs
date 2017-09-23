using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.Resolvers;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;
using DAL;

namespace BusinessObjects.Resolvers
{
    public class EntityCollectionValueResolver<TSourceParent, TSource, TDest> : IValueResolver
        where TSource : class
        where TDest : IEntity, new()
    {
        private Expression<Func<TSourceParent, ICollection>> sourceMember;

        public EntityCollectionValueResolver(
          Expression<Func<TSourceParent, ICollection>> sourceMember)
        {
            this.sourceMember = sourceMember;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            //get source collection
            var sourceCollection = ((TSourceParent)source.Value).GetPropertyValue(sourceMember);
            //if we are mapping to existing collection of entities...
            if (source.DestinationValue != null)
            {
                var destinationCollection = (ICollection<TDest>)
                    //get entities collection parent
                    source.Context.DestinationValue
                    //get entities collection by member name defined in mapping profile
                    .GetPropertyValue(source.Context.MemberName);
                //delete entities that are not in source collection
                var sourceIds = sourceCollection.Select(i => i.Id).ToList();
                foreach (var item in destinationCollection)
                {
                    if (!sourceIds.Contains(item.Id))
                    {
                        destinationCollection.Remove(item);
                    }
                }
                //map entities that are in source collection
                foreach (var sourceItem in sourceCollection)
                {
                    //if item is in destination collection...
                    var originalItem = destinationCollection.Where(
                         o => o.Id == sourceItem.Id).SingleOrDefault();
                    if (originalItem != null)
                    {
                        //...map to existing item
                        sourceItem.MapTo(originalItem);
                    }
                    else
                    {
                        //...or create new entity in collection
                        destinationCollection.Add(sourceItem.MapTo<TDest>());
                    }
                }
                return source.New(destinationCollection, source.Context.DestinationType);
            }
            //we are mapping to new collection of entities...
            else
            {
                //...then just create new collection
                var value = new HashSet<TDest>();
                //...and map every item from source collection
                foreach (var item in sourceCollection)
                {
                    //map item
                    value.Add(item.MapTo<TDest>());
                }
                //create new result mapping context
                source = source.New(value, source.Context.DestinationType);
            }
            return source;
        }
    }
}
