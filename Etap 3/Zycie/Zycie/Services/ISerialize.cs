using Projekt.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace Projekt.Services
{
    public interface ISerialize
    {
        void Serialize(AssemblyMetadata tree, string path);
    }
}
