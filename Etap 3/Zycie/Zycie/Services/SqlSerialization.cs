using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Projekt.Context;
using Projekt.Model;

namespace Projekt.Services
{
   public class SqlSerialization:ISerialize,IDisposable
    {
        private AssemblyContext asm;
        public int number = 0;
        public SqlSerialization()
        {
            asm= new AssemblyContext();
        }
        public async void Serialize(AssemblyMetadata tree, string path)
        {
            asm.AssemblyMetadatas.Add(tree);
            await SaveChangesAsync();
            number = tree.AssemblyMetadataId;
            MessageBox.Show("Assembly number is: "+Convert.ToString(tree.AssemblyMetadataId));
        }
        private async Task SaveChangesAsync()
        {
            await asm.SaveChangesAsync();
        }

        public void Dispose()
        {
            asm.Dispose();
        }
    }
}
