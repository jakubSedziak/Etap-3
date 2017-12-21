
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using Projekt.Model;


namespace Projekt.Model
{
    [Serializable]
    public class TypeMetadata : IModelWithRelation
    {
        

        #region constructors 
        public TypeMetadata()
        {
          //  m_Attributes = new AtributeMetadata();
          /*  m_Methods= new List<MethodMetadata>();
            m_Constructors= new List<MethodMetadata>();
            m_Properties= new List<PropertyMetadata>();*/
         
        }
        public TypeMetadata(Type type)
        {
          
            m_BaseType = null;
            IsExpanded = false;
            m_typeName = type.Name;
            m_DeclaringType = EmitDeclaringType(type.DeclaringType);
            m_Constructors = MethodMetadata.EmitMethods(type.GetConstructors()).ToList();
            m_Methods = MethodMetadata.EmitMethods(type.GetMethods()).ToList();
            m_NestedTypes = EmitNestedTypes(type.GetNestedTypes().ToList()).ToList();
            m_ImplementedInterfaces = EmitImplements(type.GetInterfaces().ToList()).ToList();
            m_GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments().ToList());
            m_Modifiers = EmitModifiers(type);
           
            m_BaseType = EmitExtends(type);
            m_Properties = null;//PropertyMetadata.EmitProperties(type.GetProperties()).ToList();
            m_TypeKind = GetTypeKind(type);
            List<Attribute> Attributes;
            Attributes = type.GetCustomAttributes(false).Cast<Attribute>().ToList();
            m_Attributes=new AtributeMetadata(Attributes);
           
             
            
        }
        #endregion


        #region API
        public enum TypeKind
        {
            EnumType, StructType, InterfaceType, ClassType
        }
        internal static TypeMetadata EmitReference(Type type)
        {
            if (!type.IsGenericType)
                return new TypeMetadata(type.Name, type.GetNamespace());
            else
                return new TypeMetadata(type.Name, type.GetNamespace(), EmitGenericArguments(type.GetGenericArguments().ToList()));
        }
        internal static List<TypeMetadata> EmitGenericArguments(List<Type> arguments)
        {
            return (from Type _argument in arguments select EmitReference(_argument)).ToList();
        }
        #endregion

        #region properties
        [JsonProperty]
        public int TypeMetadataId { get; set; }
        [JsonProperty]
        [Required]
        public bool IsExpanded { get; set; }
        [JsonProperty]
        [Required]
        public string m_typeName { get; set; }
        [JsonProperty]
        
        public string m_NamespaceName { get; set; }

        [JsonProperty]
     
        public virtual TypeMetadata m_BaseType { get; set; } = null;
        [JsonProperty]
        public virtual List<TypeMetadata> m_GenericArguments { get; set; }
        [JsonProperty]
        public Tuple<AccessLevel, SealedEnum, AbstractENum> m_Modifiers { get; set; }
        [JsonProperty]
        public virtual TypeKind m_TypeKind { get; set; }
        [JsonProperty]
        public AtributeMetadata m_Attributes { get; set; }
        [JsonProperty]
        public virtual List<TypeMetadata> m_ImplementedInterfaces { get; set; }
        [JsonProperty]
        public virtual List<TypeMetadata> m_NestedTypes { get; set; }
        [JsonProperty]
        public virtual List<PropertyMetadata> m_Properties { get; set; }
        [JsonProperty]
        public virtual TypeMetadata m_DeclaringType { get; set; }
        [JsonProperty]
        public virtual List<MethodMetadata> m_Methods { get; set; }
        [JsonProperty]
        public virtual List<MethodMetadata> m_Constructors { get; set; }
        #endregion

        #region Methods
        public bool Equals(TypeMetadata obj)
        {
            if (this.m_NamespaceName != obj.m_NamespaceName) return false;
            if (this.m_typeName != obj.m_typeName) return false;
            return true;
        }
        private TypeMetadata(string typeName, string namespaceName)
        {
            m_typeName = typeName;
            m_NamespaceName = namespaceName;
        }
        private TypeMetadata(string typeName, string namespaceName, List<TypeMetadata> genericArguments) : this(typeName, namespaceName)
        {
            m_GenericArguments = genericArguments;
        }
        //methods
        private TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private List<TypeMetadata> EmitNestedTypes(List<Type> nestedTypes)
        {
            return (from _type in nestedTypes
                   where _type.GetVisible()
                   select new TypeMetadata(_type)).ToList();
        }
        private List<TypeMetadata> EmitImplements(List<Type> interfaces)
        {
            return (from currentInterface in interfaces
                   select EmitReference(currentInterface)).ToList();
        }
        private static TypeKind GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKind.EnumType :
                   type.IsValueType ? TypeKind.StructType :
                   type.IsInterface ? TypeKind.InterfaceType :
                   TypeKind.ClassType;
        }
        static Tuple<AccessLevel, SealedEnum, AbstractENum> EmitModifiers(Type type)
        {
            AccessLevel _access = AccessLevel.IsPrivate;
            if (type.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (type.IsNestedPublic)
                _access = AccessLevel.IsPublic;
            else if (type.IsNestedFamily)
                _access = AccessLevel.IsProtected;
            else if (type.IsNestedFamANDAssem)
                _access = AccessLevel.IsProtectedInternal;
            SealedEnum _sealed = SealedEnum.NotSealed;
            if (type.IsSealed) _sealed = SealedEnum.Sealed;
            AbstractENum _abstract = AbstractENum.NotAbstract;
            if (type.IsAbstract)
                _abstract = AbstractENum.Abstract;
            return new Tuple<AccessLevel, SealedEnum, AbstractENum>(_access, _sealed, _abstract);
        }
        private static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }



        #endregion

        #region getters

        public List<MethodMetadata> GetMetadata()
        {
            return m_Methods;
        }
        public AtributeMetadata getAttributes()
        {
            return m_Attributes;
        }

        public Tuple<AccessLevel, SealedEnum, AbstractENum> getAcceessLevel()
        {
            return m_Modifiers;
        }

        public string getName()
        {
            return m_typeName;
        }

        #region Types
        public List<TypeMetadata> getGenericArguments()
        {
            return m_GenericArguments;
        }

        public List<TypeMetadata> getImplementedInterfaces()
        {
            return m_ImplementedInterfaces;
        }

        public List<TypeMetadata> getNestedTypes()
        {
            return m_NestedTypes;
        }
        #endregion

        #region Methods

        public List<MethodMetadata> getMethods()
        {
            return m_Methods;
        }

        public List<MethodMetadata> getConstructors()
        {
            return m_Constructors;
        }

        #endregion

        #region Properties

        public List<PropertyMetadata> getProperties()
        {
            return m_Properties;
        }


        #endregion

        #endregion
    

       
    }
}