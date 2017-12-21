
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace Projekt.Model
{
    [Serializable]
    public class NamespaceMetadata 
    {
        #region Constructors
        public NamespaceMetadata(string name, IEnumerable<Type> types)
        {
            IsExpanded = false;
            m_NamespaceName = name;
            m_Types = (from type in types orderby type.Name select new TypeMetadata(type)).ToList();
        }
        public NamespaceMetadata()
        {
            m_NamespaceName = "namespace";
        }
        #endregion

        #region properties
        [JsonProperty]
        public int NamespaceMetadataId { get; set; }
        [JsonProperty] public string m_NamespaceName { get; set; }
        [JsonProperty] public virtual List<TypeMetadata> m_Types { get; set; }
        [JsonProperty] public bool IsExpanded { get; set; }
        #endregion

        #region Methods
        public string getNamespaceName()
        {
            return m_NamespaceName;
        }

        public IEnumerable<TypeMetadata> getNamespaceTypes()
        {
            return m_Types;
        }
    

        public bool Equals(NamespaceMetadata obj)
        {
            foreach (TypeMetadata typeMetadata in m_Types)
            {
                bool finded = false;
                foreach (TypeMetadata VARIABLE in obj.m_Types)
                {
                    if (typeMetadata.Equals(VARIABLE)) finded = true;
                }
                if (!finded) return false;
            }
            return true;
        }

        public IEnumerable<TypeMetadata> GeTypeMetadatas()
        {
            return m_Types;
        }
        #endregion
    }
}