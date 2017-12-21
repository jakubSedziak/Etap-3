using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Projekt.Model;

namespace Projekt.Model
{
    //[Serializable]
    public class AssemblyTreeViewItem : ITreeViewItem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public AssemblyMetadata assemblyMetadata;
        AllClases all;
        public AssemblyTreeViewItem(AssemblyMetadata assembly)
        {
            assemblyMetadata=assembly;
            all = new AllClases(assemblyMetadata);
            if (assemblyMetadata.IsExpanded) this.IsExpanded = true;
        }


        public override void buildMyself()
        {
            foreach (NamespaceMetadata namespaces in assemblyMetadata.getListMetadata())
            {
                Children.Add(new NamespaceTreeViewItem(namespaces, all){Name = namespaces.getNamespaceName()});
            }
            assemblyMetadata.IsExpanded = true;
            log.Info("Odwiedzono Asembly: " + Name);
        }

        public AllClases GetAllClases()
        {
            return all;
        }

        public override string ToString()
        {
            log.Info("Wyswietlono info o Assembly: " + Name);
            return "Assembly Name: "+ Name;
        }
    }
}
