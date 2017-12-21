using Projekt.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using Newtonsoft.Json;

namespace Projekt.Services
{
    public interface IDeserialize
    {
         AssemblyMetadata Deserialize(string path);
    }
}



