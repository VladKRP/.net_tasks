namespace EFORMSample.Migrations
{
    using ORMSample.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFORMSample.NorthwindContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NorthwindContext context)
        {
            ////Task 3
            if (!context.Categories.Any())
            {
                Category[] categories =
                {
                    new Category(){ CategoryName = "Electronic", Description = "Description conserning electric devices" },
                    new Category(){ CategoryName = "Food", Description = "Food description"}
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Categories, categories);
                context.SaveChanges();
            }

            if (!context.Regions.Any())
            {
                Region[] regions =
                {
                    new Region(){ RegionDescription = "REG 1"},
                    new Region(){ RegionDescription = "REG 2"}
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Regions, regions);
                context.SaveChanges();
            }

            if (!context.Territories.Any())
            {
                Territory[] territories =
                {
                    new Territory(){ TerritoryDescription = "TER 1"},
                    new Territory(){ TerritoryDescription = "TER 2"}
                };

                DbSetMigrationsExtensions.AddOrUpdate(context.Territories, territories);
                context.SaveChanges();
            }
            
            
            base.Seed(context);
        }
    }
}
