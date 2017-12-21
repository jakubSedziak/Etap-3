using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Projekt.Context;
using Projekt.Model;
using Projekt.Services;

namespace Zycie.Services
{
  public  class SQLDeserialization : IDeserialize
    {
        public SQLDeserialization()
        {
            asmMetadata = new AssemblyMetadata();
            asm = new AssemblyContext();
        }

        private AssemblyMetadata asmMetadata;
        private AssemblyContext asm;
        public Task<List<AssemblyMetadata>> DeserializeMetadata()
        {
            return asm.Set<AssemblyMetadata>().ToListAsync();
        }

        public async void fillAssembly(int assemblyid)
        {
            foreach (var VARIABLE in await DeserializeMetadata())
            {
                if (VARIABLE.AssemblyMetadataId == assemblyid)
                {
                    asmMetadata.m_Namespaces = VARIABLE.m_Namespaces;
                    asmMetadata.m_Name = VARIABLE.m_Name;
                    asmMetadata.IsExpanded = VARIABLE.IsExpanded;
                }
            }
            if (asmMetadata.m_Name == null) MessageBox.Show("Niestety nie ma Assembly z takim id");
        }
        public AssemblyMetadata Deserialize(string path)
        {
          fillAssembly(Convert.ToInt32(path));
            return asmMetadata;
        }
    }
}
