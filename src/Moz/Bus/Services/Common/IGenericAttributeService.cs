using System.Collections.Generic;
using Moz.Bus.Models;
using Moz.CMS.Models;
using Moz.CMS.Model;
using Moz.CMS.Model.Common;
using Moz.CMS.Models;
using Moz.Domain.Models;
using Moz.Models.Common;

namespace Moz.Service.Common
{
    public interface IGenericAttributeService
    {
        /// <summary>
        ///     Deletes an attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        void DeleteAttribute(GenericAttribute attribute);

        /// <summary>
        ///     Deletes an attributes
        /// </summary>
        /// <param name="attributes">Attributes</param>
        void DeleteAttributes(List<GenericAttribute> attributes);

        /// <summary>
        ///     Gets an attribute
        /// </summary>
        /// <param name="attributeId">Attribute identifier</param>
        /// <returns>An attribute</returns>
        GenericAttribute GetAttributeById(long attributeId);

        /// <summary>
        ///     Inserts an attribute
        /// </summary>
        /// <param name="attribute">attribute</param>
        void InsertAttribute(GenericAttribute attribute);

        /// <summary>
        ///     Updates the attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        void UpdateAttribute(GenericAttribute attribute);

        /// <summary>
        ///     Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        List<GenericAttribute> GetAttributesForEntity(long entityId, string keyGroup);

        /// <summary>
        ///     Saves the attribute.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TPropType">The 1st type parameter.</typeparam>
        void SaveAttribute<TPropType>(BaseModel entity, string key, TPropType value);
    }
}