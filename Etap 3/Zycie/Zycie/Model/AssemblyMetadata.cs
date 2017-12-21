
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;


namespace Projekt.Model
{
    [Serializable]
    public class AssemblyMetadata 
    {
        #region constructors
        internal AssemblyMetadata(Assembly assembly)
        {
            IsExpanded = false;
            m_Name = assembly.ManifestModule.Name;
            m_Namespaces = (from Type _type in assembly.GetTypes()
                            where _type.GetVisible()
                            group _type by _type.GetNamespace() into _group
                            orderby _group.Key
                            select new NamespaceMetadata(_group.Key, _group)).ToList();
        }

        public AssemblyMetadata()
        {
            m_Namespaces = new List<NamespaceMetadata>();
        }
        #endregion

        #region Properties

        [JsonProperty]
        public int AssemblyMetadataId { get; set; }

        [JsonProperty]
        public string m_Name { get; set; }

        [JsonProperty]
        public virtual List<NamespaceMetadata> m_Namespaces { get; set; }

        [JsonProperty]
        public bool IsExpanded { get; set; }

        #endregion

        #region Methods

        public string getName()
        {
            return m_Name;
        }

        public List<NamespaceMetadata> getListMetadata()
        {
            return m_Namespaces;
        }

        public bool Equals(AssemblyMetadata obj)
        {
            foreach (NamespaceMetadata VARIABLE in m_Namespaces)
            {
                bool finded = false;
                foreach (NamespaceMetadata VARIABLE1 in obj.m_Namespaces)
                {
                    if (VARIABLE.Equals(VARIABLE1)) finded = true;
                }
                if (!finded) return false;
            }

            return true;
        }
     

        #endregion
    }
}