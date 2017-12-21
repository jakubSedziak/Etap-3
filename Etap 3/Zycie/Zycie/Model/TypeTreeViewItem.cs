using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Projekt.Model;

namespace Projekt.Model
{
  //  [Serializable]
    public class TypeTreeViewItem : ITreeViewItem
    {
        private TypeMetadata typeMetadata;
       public  AllClases all;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TypeTreeViewItem(TypeMetadata typeMetadata,AllClases allClases)
        {
            all = allClases;
            this.typeMetadata = typeMetadata;
            if (typeMetadata.IsExpanded) this.IsExpanded = true;
        }

        public TypeMetadata GetTypeMetadata()
        {
            return typeMetadata;
        }
        public override void buildMyself()
        {
            typeMetadata.IsExpanded = true;
            log.Info("Odwiedzono Typ: " + Name);
            if (typeMetadata.getNestedTypes() != null)
                foreach (TypeMetadata types in typeMetadata.getNestedTypes())
                {
                    TypeMetadata tm = types;
                    tm = all.retType(tm);
                    Children.Add(new TypeTreeViewItem(tm,all) { Name = types.getName() });
                }
            if (typeMetadata.getGenericArguments() != null)
                foreach (TypeMetadata types in typeMetadata.getGenericArguments())
                {
                    TypeMetadata tm = types;
                    tm = all.retType(tm);
                    Children.Add(new TypeTreeViewItem(tm,all) { Name = types.getName() });
                }
            if (typeMetadata.getImplementedInterfaces() != null)
                foreach (TypeMetadata types in typeMetadata.getImplementedInterfaces())
                {
                    TypeMetadata tm = types;
                    tm = all.retType(tm);
                    Children.Add(new TypeTreeViewItem(tm,all) { Name = types.getName() });
                }
            if (typeMetadata.getMethods() != null)
                foreach (MethodMetadata methods in typeMetadata.getMethods())
                {
                    Children.Add(new MethodTreeViewItem(methods,all) { Name = methods.getName() });
                }
            if (typeMetadata.getConstructors() != null)
                foreach (MethodMetadata methods in typeMetadata.getConstructors())
                {
                    Children.Add(new MethodTreeViewItem(methods,all) { Name = methods.getName() });
                }
            if (typeMetadata.getProperties() != null)
                foreach (PropertyMetadata properties in typeMetadata.getProperties())
                {
                    Children.Add(new PropertyTreeViewItem(properties,all) { Name = properties.getName() });
                }
        }

        public override string ToString()
        {
            log.Info("Wyswietlono info o Typie: " + Name);
            return "Type  Name: " + Name +"   Aceess modifire: "+ typeMetadata.getAcceessLevel().Item1.ToString() + "   Sealed: " +
                   typeMetadata.getAcceessLevel().Item2.ToString() + "  Abstract: " +
                   typeMetadata.getAcceessLevel().Item3.ToString()+"   Atributes: " + typeMetadata.getAttributes().ToString();
        }
        public bool Equals(TypeTreeViewItem item)
        {
            if (this.typeMetadata == item.typeMetadata)
                return true;
            return false;
        }
    }
}
