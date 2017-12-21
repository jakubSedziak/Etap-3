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
    class JsonSerialization:ISerialize
    {
       
            public void Serialize(AssemblyMetadata tree, string path)
            {

                var json = JsonConvert.SerializeObject(tree, Newtonsoft.Json.Formatting.Indented,
                    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                File.WriteAllText(@path, json);
            }
        
    }
}
