namespace DevGohel.Migrations
{
    using DevGohel.Models;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DevGohel.Models.BookDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DevGohel.Models.BookDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Authors.AddOrUpdate(x => x.AuthorId,
                new Author()
                {
                    EmailId = "lectroondev@gmail.com",
                    Password = "Iamdev1##",
                    Created = DateTime.Now,
                    IsActive = true,
                    Name = "Devendra Gohel",
                });
        }
    }
}
