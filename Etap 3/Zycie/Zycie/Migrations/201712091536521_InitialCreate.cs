namespace Zycie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssemblyMetadatas",
                c => new
                    {
                        AssemblyMetadataId = c.Int(nullable: false, identity: true),
                        m_Name = c.String(),
                        IsExpanded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssemblyMetadataId);
            
            CreateTable(
                "dbo.NamespaceMetadatas",
                c => new
                    {
                        NamespaceMetadataId = c.Int(nullable: false, identity: true),
                        m_NamespaceName = c.String(),
                        IsExpanded = c.Boolean(nullable: false),
                        AssemblyMetadata_AssemblyMetadataId = c.Int(),
                    })
                .PrimaryKey(t => t.NamespaceMetadataId)
                .ForeignKey("dbo.AssemblyMetadatas", t => t.AssemblyMetadata_AssemblyMetadataId)
                .Index(t => t.AssemblyMetadata_AssemblyMetadataId);
            
            CreateTable(
                "dbo.TypeMetadatas",
                c => new
                    {
                        TypeMetadataId = c.Int(nullable: false, identity: true),
                        IsExpanded = c.Boolean(nullable: false),
                        m_typeName = c.String(nullable: false),
                        m_NamespaceName = c.String(nullable: true),
                        m_TypeKind = c.Int(nullable: true),
                        m_BaseType_TypeMetadataId = c.Int(nullable: true),
                        MethodMetadata_MethodMetadataId = c.Int(),
                        m_DeclaringType_TypeMetadataId = c.Int(),
                        TypeMetadata_TypeMetadataId = c.Int(),
                        TypeMetadata_TypeMetadataId1 = c.Int(),
                        TypeMetadata_TypeMetadataId2 = c.Int(),
                        PropertyMetadata_PropertyMetadataId=c.Int(),
                        NamespaceMetadata_NamespaceMetadataId = c.Int(),
                    })
                .PrimaryKey(t => t.TypeMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.m_BaseType_TypeMetadataId)
                .ForeignKey("dbo.MethodMetadatas", t => t.MethodMetadata_MethodMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.m_DeclaringType_TypeMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_TypeMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_TypeMetadataId1)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_TypeMetadataId2)
                .ForeignKey("dbo.PropertyMetadatas", t => t.PropertyMetadata_PropertyMetadataId)
                .ForeignKey("dbo.NamespaceMetadatas", t => t.NamespaceMetadata_NamespaceMetadataId)
                .Index(t => t.m_BaseType_TypeMetadataId)
                .Index(t => t.MethodMetadata_MethodMetadataId)
                .Index(t => t.m_DeclaringType_TypeMetadataId)
                .Index(t => t.TypeMetadata_TypeMetadataId)
                .Index(t => t.TypeMetadata_TypeMetadataId1)
                .Index(t => t.TypeMetadata_TypeMetadataId2)
                .Index(t=>t.PropertyMetadata_PropertyMetadataId)
                .Index(t => t.NamespaceMetadata_NamespaceMetadataId);
            
            CreateTable(
                "dbo.MethodMetadatas",
                c => new
                    {
                        MethodMetadataId = c.Int(nullable: false, identity: true),
                        IsExpanded = c.Boolean(nullable: false),
                        m_Name = c.String(),
                        m_Extension = c.Boolean(nullable: true),
                        m_ReturnType_TypeMetadataId = c.Int(),
                        TypeMetadata_TypeMetadataId = c.Int(),
                        TypeMetadata_TypeMetadataId1 = c.Int(),
                    })
                .PrimaryKey(t => t.MethodMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.m_ReturnType_TypeMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_TypeMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.TypeMetadata_TypeMetadataId1)
                .Index(t => t.m_ReturnType_TypeMetadataId)
                .Index(t => t.TypeMetadata_TypeMetadataId)
                .Index(t => t.TypeMetadata_TypeMetadataId1);
            
            CreateTable(
                "dbo.ParameterMetadatas",
                c => new
                    {
                        ParameterMetadataId = c.Int(nullable: false, identity: true),
                        m_Name = c.String(),
                        m_TypeMetadata_TypeMetadataId = c.Int(nullable: true),
                        MethodMetadata_MethodMetadataId = c.Int(),
                    })
                .PrimaryKey(t => t.ParameterMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.m_TypeMetadata_TypeMetadataId, cascadeDelete: true)
                .ForeignKey("dbo.MethodMetadatas", t => t.MethodMetadata_MethodMetadataId)
                .Index(t => t.m_TypeMetadata_TypeMetadataId)
                .Index(t => t.MethodMetadata_MethodMetadataId);
            
            CreateTable(
                "dbo.PropertyMetadatas",
                c => new
                    {
                        PropertyMetadataId = c.Int(nullable: false, identity: true),
                        m_Name = c.String(),
                        IsExpanded = c.Boolean(nullable: false),
                        m_TypeMetadata_TypeMetadataId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.PropertyMetadataId)
                .ForeignKey("dbo.TypeMetadatas", t => t.m_TypeMetadata_TypeMetadataId)
                .Index(t => t.m_TypeMetadata_TypeMetadataId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NamespaceMetadatas", "AssemblyMetadata_AssemblyMetadataId", "dbo.AssemblyMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "NamespaceMetadata_NamespaceMetadataId", "dbo.NamespaceMetadatas");
            DropForeignKey("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "TypeMetadata_TypeMetadataId2", "dbo.TypeMetadatas");
            DropForeignKey("dbo.MethodMetadatas", "TypeMetadata_TypeMetadataId1", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "TypeMetadata_TypeMetadataId1", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "m_DeclaringType_TypeMetadataId", "dbo.TypeMetadatas");
            DropForeignKey("dbo.MethodMetadatas", "TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas");
            DropForeignKey("dbo.MethodMetadatas", "m_ReturnType_TypeMetadataId", "dbo.TypeMetadatas");
            DropForeignKey("dbo.ParameterMetadatas", "MethodMetadata_MethodMetadataId", "dbo.MethodMetadatas");
            DropForeignKey("dbo.ParameterMetadatas", "m_TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "MethodMetadata_MethodMetadataId", "dbo.MethodMetadatas");
            DropForeignKey("dbo.TypeMetadatas", "m_BaseType_TypeMetadataId", "dbo.TypeMetadatas");
            DropIndex("dbo.PropertyMetadatas", new[] { "m_TypeMetadata_TypeMetadataId" });
            DropIndex("dbo.ParameterMetadatas", new[] { "MethodMetadata_MethodMetadataId" });
            DropIndex("dbo.ParameterMetadatas", new[] { "m_TypeMetadata_TypeMetadataId" });
            DropIndex("dbo.MethodMetadatas", new[] { "TypeMetadata_TypeMetadataId1" });
            DropIndex("dbo.MethodMetadatas", new[] { "TypeMetadata_TypeMetadataId" });
            DropIndex("dbo.MethodMetadatas", new[] { "m_ReturnType_TypeMetadataId" });
            DropIndex("dbo.TypeMetadatas", new[] { "NamespaceMetadata_NamespaceMetadataId" });
            DropIndex("dbo.TypeMetadatas", new[] { "TypeMetadata_TypeMetadataId2" });
            DropIndex("dbo.TypeMetadatas", new[] { "TypeMetadata_TypeMetadataId1" });
            DropIndex("dbo.TypeMetadatas", new[] { "TypeMetadata_TypeMetadataId" });
            DropIndex("dbo.TypeMetadatas", new[] { "m_DeclaringType_TypeMetadataId" });
            DropIndex("dbo.TypeMetadatas", new[] { "MethodMetadata_MethodMetadataId" });
            DropIndex("dbo.TypeMetadatas", new[] { "m_BaseType_TypeMetadataId" });
            DropIndex("dbo.NamespaceMetadatas", new[] { "AssemblyMetadata_AssemblyMetadataId" });
            DropTable("dbo.PropertyMetadatas");
            DropTable("dbo.ParameterMetadatas");
            DropTable("dbo.MethodMetadatas");
            DropTable("dbo.TypeMetadatas");
            DropTable("dbo.NamespaceMetadatas");
            DropTable("dbo.AssemblyMetadatas");
        }
    }
}
