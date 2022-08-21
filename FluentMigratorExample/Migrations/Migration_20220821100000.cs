using FluentMigrator;
using FluentMigrator.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentMigratorExample.Migrations
{
	[Migration(20220821100000)]
	public class Migration_20220821100000 : Migration
	{
		public override void Down()
		{
			Delete.Table("Product");
		}

		public override void Up()
		{
			Create.Table("Product")
				.WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
				.WithColumn("Name").AsString().NotNullable()
				.WithColumn("Price").AsDecimal().NotNullable()
				.WithColumn("Quantity").AsInt32().Nullable();
		}
	}
}
