namespace Zycie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAtributes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AtributeMetadatas",
                c => new
                    {
                        AtributeMetadataId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.AtributeMetadataId);
            
            AddColumn("dbo.TypeMetadatas", "m_Attributes_AtributeMetadataId", c => c.Int());
            CreateIndex("dbo.TypeMetadatas", "m_Attributes_AtributeMetadataId");
            AddForeignKey("dbo.TypeMetadatas", "m_Attributes_AtributeMetadataId", "dbo.AtributeMetadatas", "AtributeMetadataId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TypeMetadatas", "m_Attributes_AtributeMetadataId", "dbo.AtributeMetadatas");
            DropIndex("dbo.TypeMetadatas", new[] { "m_Attributes_AtributeMetadataId" });
            DropColumn("dbo.TypeMetadatas", "m_Attributes_AtributeMetadataId");
            DropTable("dbo.AtributeMetadatas");
        }
    }
}
