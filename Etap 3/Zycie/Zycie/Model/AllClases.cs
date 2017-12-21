using Projekt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Model
{
    [Serializable]
    public class AllClases
    {
        public List<TypeMetadata> assemblytypes = new List<TypeMetadata>();
        public List<Type> standardtypes = new List<Type>();
        public AllClases(AssemblyMetadata assembly)
        {
            foreach (NamespaceMetadata namespaces in assembly.getListMetadata())
            {
                foreach (TypeMetadata typ in namespaces.getNamespaceTypes())
                {
                    assemblytypes.Add(typ);
                }
            }
            var mscorlib = typeof(string).Assembly;
            var typek = mscorlib.GetTypes().Where(t => t.Namespace == "System");
            foreach(Type t in typek)
            {
                standardtypes.Add(t);
            }
        }
        public TypeMetadata apropriateType(TypeMetadata typ)
        {
            foreach(TypeMetadata t in assemblytypes)
            {
                if(t.getName()==typ.getName())
                {
                    return t;
                }
            }
            foreach (Type t in standardtypes)
            {
                if (t.Name == typ.getName())
                {
                    return new TypeMetadata(t);
                }
            }
            return null;
        }
        public TypeMetadata retType(TypeMetadata typ)
        {
            TypeMetadata t =apropriateType(typ);
                if(t!=null)
                typ = t ;
            return typ;
        }
    }
}
