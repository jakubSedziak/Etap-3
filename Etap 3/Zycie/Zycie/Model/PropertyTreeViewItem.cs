using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Projekt.Model;

namespace Projekt.Model
{
  //  [Serializable]
    public class PropertyTreeViewItem:ITreeViewItem
    {
        AllClases all;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private PropertyMetadata propertyMetadata;

        public PropertyTreeViewItem(PropertyMetadata propertyMetadata,AllClases allClases)
        {
            all = allClases;
            this.propertyMetadata = propertyMetadata;
            if (propertyMetadata.IsExpanded) this.IsExpanded = true;
        }

        public override void buildMyself()
        {
            TypeMetadata tm = propertyMetadata.geTypeMetadata();
            tm = all.retType(tm);
            log.Info("Odwiedzono Property: " + Name);
            Children.Add(new TypeTreeViewItem(tm,all){Name = propertyMetadata.geTypeMetadata().getName()});
        }

        public override string ToString()
        {
            log.Info("Wyswietlono info o Property: " + Name);
            return "Property Name: " + Name+" "+"Type:"+ propertyMetadata.geTypeMetadata().getName();
        }
    }
}
