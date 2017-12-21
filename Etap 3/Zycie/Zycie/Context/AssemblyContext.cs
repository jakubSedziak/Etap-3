using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
using Projekt.Model;
using System.Data.Entity;
using  Microsoft.SqlServer.Server;

namespace Projekt.Context
{
    class AssemblyContext : DbContext
    {
       
        public DbSet<AssemblyMetadata> AssemblyMetadatas { get; set; }
        public DbSet<NamespaceMetadata> NamespaceMetadatas { get; set; }

        public DbSet<TypeMetadata> TypeMetadatas { get; set; }

        public DbSet<MethodMetadata> MethodMetadatas { get; set; }

        public DbSet<PropertyMetadata> PropertyMetadatas { get; set; }
        public DbSet<ParameterMetadata> ParameterMetadatas { get; set; }

        public DbSet<AtributeMetadata> AtributeMetadatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssemblyMetadata>().HasMany(g => g.m_Namespaces) ;
            modelBuilder.Entity<NamespaceMetadata>().HasMany(s => s.m_Types);
            modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_ImplementedInterfaces);
            modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_NestedTypes);
            modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_GenericArguments);

            modelBuilder.Entity<TypeMetadata>().HasOptional(s => s.m_BaseType);
            modelBuilder.Entity<TypeMetadata>().HasOptional(s => s.m_DeclaringType);
            modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Properties).WithRequired(w=>w.m_TypeMetadata);

           modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Methods);
            modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Constructors);

            modelBuilder.Entity<TypeMetadata>().HasOptional(s => s.m_Attributes);
            modelBuilder.Entity<MethodMetadata>().HasOptional(s => s.m_ReturnType);
            modelBuilder.Entity<MethodMetadata>().HasOptional(s => s.m_GenericArguments);
            modelBuilder.Entity<MethodMetadata>().HasMany(s => s.m_GenericArguments);

            /*     modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Methods).WithOptional(g => g.m_ReturnType);
                 modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Constructors).WithOptional(g => g.m_ReturnType);
     
                 modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Constructors).WithMany(s=>s.m_GenericArguments);
                 modelBuilder.Entity<TypeMetadata>().HasMany(s => s.m_Methods).WithMany(s => s.m_GenericArguments);*/


        }

        private IEnumerable<Type> GetClassesFromNamespace()
        {
            var @interface = typeof(IModelWithRelation);
            var @namespace = "Insurinator.Models.Entities";
            return Assembly.GetAssembly(typeof(AssemblyMetadata))
                .GetTypes()
                .Where(t => t.IsClass && (t.Namespace?.Contains(@namespace) ?? false) &&
                            @interface.IsAssignableFrom(t));
        }
    }
}
