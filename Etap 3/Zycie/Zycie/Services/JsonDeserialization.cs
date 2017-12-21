using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Projekt.Model;

namespace Projekt.Services
{
    class JsonDeserialization:IDeserialize
    {
        public AssemblyMetadata Deserialize(string path)
        {
            string json = File.ReadAllText(path);
            AssemblyMetadata deserializedaseAssemblyMetadata = JsonConvert.DeserializeObject<AssemblyMetadata>(json,
                new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return deserializedaseAssemblyMetadata;
            //  MessageBox.Show("bleble");
        }
    }
}
