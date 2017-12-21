using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt.Model;
using Projekt.Model;

namespace Projekt.Model
{
   // [Serializable]
    public class MethodTreeViewItem : ITreeViewItem
    {
        private MethodMetadata methodMetadata;
        AllClases all;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MethodTreeViewItem(MethodMetadata methodMetadata,AllClases allClases)
        {
            all = allClases;
            this.methodMetadata = methodMetadata;
            if (methodMetadata.IsExpanded) this.IsExpanded = true;
        }

        public override void buildMyself()
        {
            methodMetadata.IsExpanded = true;
            log.Info("Odwiedzono Metode: " + Name);
            if (methodMetadata.getGenerics() != null)
                foreach (TypeMetadata types in methodMetadata.getGenerics())
                {
                    TypeMetadata tm = types;
                    tm = all.retType(tm);
                    Children.Add(new TypeTreeViewItem(tm,all) { Name = tm.getName() });
                }
            if (methodMetadata.getReturnedType() != null)
            {
                TypeMetadata tm = methodMetadata.getReturnedType();
               
                if (tm.getName() != "Void")
                {
                    tm = all.retType(tm);
                    Children.Add(new TypeTreeViewItem(tm,all) { Name = tm.getName() });
                }
            }
            if (methodMetadata.getParametr() != null)
            {
                foreach (ParameterMetadata pm in methodMetadata.getParametr())
                {
                    TypeMetadata tm = pm.getTypeMetadata();
                   
                    tm = all.retType(tm);
                    Children.Add(new TypeTreeViewItem(tm,all) { Name = tm.getName() });
                }
            }
        }

        public override string ToString()
        {
            log.Info("Wyswietlono info o Metodzie: " + Name);
            return "Method  Name: " + Name + " Access Level: "+ methodMetadata.getAccessLEvel().Item1+
                " AbstractEnum: "+ methodMetadata.getAccessLEvel().Item2+" StaticEnum: "+ methodMetadata.getAccessLEvel().Item3
                +" VirtualEnum: "+ methodMetadata.getAccessLEvel().Item4;
        }
    }
}
