namespace Zycie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas");
            DropIndex("dbo.PropertyMetadatas", new[] { "m_TypeMetadata_TypeMetadataId" });
            AlterColumn("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", c => c.Int(nullable: false));
            CreateIndex("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId");
            AddForeignKey("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas", "TypeMetadataId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas");
            DropIndex("dbo.PropertyMetadatas", new[] { "m_TypeMetadata_TypeMetadataId" });
            AlterColumn("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", c => c.Int());
            CreateIndex("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId");
            AddForeignKey("dbo.PropertyMetadatas", "m_TypeMetadata_TypeMetadataId", "dbo.TypeMetadatas", "TypeMetadataId");
        }
    }
}
