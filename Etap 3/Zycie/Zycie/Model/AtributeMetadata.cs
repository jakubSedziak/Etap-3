using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Projekt.Model
{
    [Serializable]
   public class AtributeMetadata
    {

        public int AtributeMetadataId { get; set; }

        [JsonProperty]
        private string atributes;

        public AtributeMetadata(IEnumerable<Attribute> list)
        {
            atributes = "";
            foreach (Attribute art in list)
            {
                atributes += art.ToString();
            }
        }

        public AtributeMetadata()
        {
            
        }

        public override string ToString()
        {
            return atributes;
        }
    }
}
