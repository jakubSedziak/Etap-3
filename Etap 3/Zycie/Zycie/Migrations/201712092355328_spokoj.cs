namespace Zycie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spokoj : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TypeMetadatas", "m_NamespaceName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TypeMetadatas", "m_NamespaceName", c => c.String(nullable: false));
        }
    }
}
