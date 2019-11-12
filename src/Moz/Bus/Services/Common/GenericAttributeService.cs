using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Model;
using Moz.CMS.Model.Common;
using Moz.CMS.Models;
using Moz.Events;
using Moz.Service.Common;
using Moz.Utils;

namespace Moz.CMS.Services.Common
{
    public class GenericAttributeService : IGenericAttributeService
    {
        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="eventPublisher"></param>
        public GenericAttributeService(IDistributedCache distributedCache,
            //IRepository genericAttributeRepository,
            IEventPublisher eventPublisher)
        {
            _distributedCache = distributedCache;
            //this._genericAttributeRepository = genericAttributeRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Constants

        /// <summary>
        ///     Key for caching
        /// </summary>
        /// <remarks>
        ///     {0} : entity ID
        ///     {1} : key group
        /// </remarks>
        private const string GENERICATTRIBUTE_KEY = "Moz.genericattribute.{0}-{1}";

        /// <summary>
        ///     Key pattern to clear cache
        /// </summary>
        private const string GENERICATTRIBUTE_PATTERN_KEY = "Moz.genericattribute.";

        #endregion

        #region Fields

        //private readonly IRepository _genericAttributeRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Methods

        /// <summary>
        ///     Deletes an attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        public virtual void DeleteAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            //_genericAttributeRepository.Delete(attribute);

            //cache
            //_cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(attribute);
        }

        /// <summary>
        ///     Deletes an attributes
        /// </summary>
        /// <param name="attributes">Attributes</param>
        public virtual void DeleteAttributes(List<GenericAttribute> attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            //_genericAttributeRepository.Delete(attributes);

            //cache
            //_cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //event notification
            foreach (var attribute in attributes) _eventPublisher.EntityDeleted(attribute);
        }

        /// <summary>
        ///     Gets an attribute
        /// </summary>
        /// <param name="attributeId">Attribute identifier</param>
        /// <returns>An attribute</returns>
        public virtual GenericAttribute GetAttributeById(long attributeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Inserts an attribute
        /// </summary>
        /// <param name="attribute">attribute</param>
        public virtual void InsertAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            //_genericAttributeRepository.Insert(attribute);

            //cache
            //_cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityCreated(attribute);
        }

        /// <summary>
        ///     Updates the attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        public virtual void UpdateAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            //_genericAttributeRepository.Update(attribute);

            //cache
            //_cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(attribute);
        }

        /// <summary>
        ///     Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        public virtual List<GenericAttribute> GetAttributesForEntity(long entityId, string keyGroup)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Saves the attribute.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TPropType">The 1st type parameter.</typeparam>
        public virtual void SaveAttribute<TPropType>(BaseModel entity, string key, TPropType value)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var keyGroup = entity.GetType().Name;

            var props = GetAttributesForEntity(entity.Id, keyGroup).ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var valueStr = ConvertHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    DeleteAttribute(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    UpdateAttribute(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute
                    {
                        EntityId = entity.Id,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr
                    };
                    InsertAttribute(prop);
                }
            }
        }

        #endregion
    }
}