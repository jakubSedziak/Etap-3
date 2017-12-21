
using System;
using Newtonsoft.Json;


namespace Projekt.Model
{
    [Serializable]
    public class ParameterMetadata : IModelWithRelation
    {
     

        #region constructors
        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.m_Name = name;
            this.m_TypeMetadata = typeMetadata;
        }

        public ParameterMetadata()
        {
            m_TypeMetadata = null;
        }

        #endregion


            #region property
        [JsonProperty]
        public int ParameterMetadataId { get; set; }
        [JsonProperty]
        public string m_Name { get; set; }
        [JsonProperty]
        public virtual TypeMetadata m_TypeMetadata { get; set; }


        #endregion


        #region getters

        public string getName()
      {
          return m_Name;
      }

      public TypeMetadata getTypeMetadata()
      {
          return m_TypeMetadata;
      }
      

      #endregion

    }
}