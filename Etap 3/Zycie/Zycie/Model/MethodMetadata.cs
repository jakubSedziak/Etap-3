
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;
using Projekt.Model;

namespace Projekt.Model
{
    [Serializable]

    public class MethodMetadata : IModelWithRelation
    {
        internal static List<MethodMetadata> EmitMethods(IEnumerable<MethodBase> methods)
        {
            return (from MethodBase _currentMethod in methods
                   where _currentMethod.GetVisible()
                   select new MethodMetadata(_currentMethod)).ToList();
        }

        #region property
        public int MethodMetadataId { get; set; }

        [JsonProperty]
        public bool IsExpanded { get; set; }
        [JsonProperty]
        public string m_Name { get; set; }
        [JsonProperty]
        public virtual List<TypeMetadata> m_GenericArguments { get; set; }
        [JsonProperty]
        public Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum> m_Modifiers { get; set; }
        [JsonProperty]
        public TypeMetadata m_ReturnType { get; set; }
        [JsonProperty]
        public bool m_Extension { get; set; }
        [JsonProperty]
        public virtual List<ParameterMetadata> m_Parameters { get; set; }
        #endregion

        #region constructors
        private MethodMetadata(MethodBase method)
        {
            IsExpanded = false;
            m_Name = method.Name;
            m_GenericArguments = !method.IsGenericMethodDefinition ? null : TypeMetadata.EmitGenericArguments(method.GetGenericArguments().ToList());
            m_ReturnType = EmitReturnType(method);
            m_Parameters = null; //EmitParameters(method.GetParameters());
            m_Modifiers = EmitModifiers(method);
            m_Extension = EmitExtension(method);
        }

        private MethodMetadata()
        {

        }


        #endregion

        #region Methods



        private static List<ParameterMetadata> EmitParameters(IEnumerable<ParameterInfo> parms)
        {
            return (from parm in parms
                   select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType))).ToList();
        }
        private static TypeMetadata EmitReturnType(MethodBase method)
        {

            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            return TypeMetadata.EmitReference(methodInfo.ReturnType);
        }
        private static bool EmitExtension(MethodBase method)
        {
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }
        private static Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            AccessLevel _access = AccessLevel.IsPrivate;
            if (method.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (method.IsFamily)
                _access = AccessLevel.IsProtected;
            else if (method.IsFamilyAndAssembly)
                _access = AccessLevel.IsProtectedInternal;
            AbstractENum _abstract = AbstractENum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractENum.Abstract;
            StaticEnum _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            VirtualEnum _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum>(_access, _abstract, _static, _virtual);
        }


        #endregion
        #region Getters

        public bool getExtention()
        {
            return m_Extension;
        }
        public string getName()
        {
            return m_Name;
        }

        public IEnumerable<TypeMetadata> getGenerics()
        {
            return m_GenericArguments;
        }

        public TypeMetadata getReturnedType()
        {
            return m_ReturnType;
        }

        public IEnumerable<ParameterMetadata> getParametr()
        {
            return m_Parameters;
        }
        public Tuple<AccessLevel, AbstractENum, StaticEnum, VirtualEnum> getAccessLEvel()
        {
            return m_Modifiers;
        }
        #endregion

    }
}