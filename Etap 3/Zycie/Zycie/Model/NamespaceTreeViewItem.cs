using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.Model;

namespace Projekt.Model
{
  //  [Serializable]
    public class NamespaceTreeViewItem : ITreeViewItem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       public NamespaceMetadata mynamespace;
        AllClases allClases;
        public NamespaceTreeViewItem(NamespaceMetadata mynamespace,  AllClases a)
        {
            allClases = a;
            this.mynamespace = mynamespace;
            if (mynamespace.IsExpanded) this.IsExpanded = true;
        }


        public override void buildMyself()
        {
            mynamespace.IsExpanded = true;
            foreach (TypeMetadata type in mynamespace.getNamespaceTypes())
            {
                Children.Add(new TypeTreeViewItem(type,allClases){Name = type.getName()});
            }
            log.Info("Odwiedzono Namespace: " + Name);
        }

        public override string ToString()
        {
            log.Info("Wyswietlono info o Namespejsie: " + Name);
            return "Namespace  Name: " + Name ;
        }
    }
}
