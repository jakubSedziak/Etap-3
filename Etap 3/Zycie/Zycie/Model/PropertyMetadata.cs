
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Newtonsoft.Json;


namespace Projekt.Model
{
    [Serializable]
    public class PropertyMetadata : IModelWithRelation
    {
        #region property
        [JsonProperty]
        public int PropertyMetadataId { get; set; }
        [JsonProperty]
        public string m_Name { get; set; }
        [JsonProperty]
        public virtual TypeMetadata m_TypeMetadata { get; set; }
        [JsonProperty]
        public bool IsExpanded { get; set; }
        #endregion

        #region constructors

        private PropertyMetadata()
        {
            m_TypeMetadata = null;

        }
        private PropertyMetadata(string propertyName, TypeMetadata propertyType)
        {
            IsExpanded = false;
            m_Name = propertyName;
            m_TypeMetadata = propertyType;
        }


        #endregion

        #region methods
        internal static IEnumerable<PropertyMetadata> EmitProperties(IEnumerable<PropertyInfo> props)
        {
            return from prop in props
                where prop.GetGetMethod().GetVisible() || prop.GetSetMethod().GetVisible()
                select new PropertyMetadata(prop.Name, TypeMetadata.EmitReference(prop.PropertyType));
        }


        #endregion


        #region Getters

        public string getName()
        {
            return m_Name;
        }
     
        public TypeMetadata geTypeMetadata()
        {
            return m_TypeMetadata;
        }


        #endregion
    }
}