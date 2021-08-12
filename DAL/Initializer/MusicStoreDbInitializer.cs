using System.Data.Entity;

namespace DAL
{
    public class MusicStoreDbInitializer : CreateDatabaseIfNotExists<MusicStoreModel>
    {
        protected override void Seed(MusicStoreModel context)
        {
            base.Seed(context);

            context.Genres.Add(new Genre() { Name = "asdd" });
            context.Genres.Add(new Genre() { Name = "jkh" });
            context.Genres.Add(new Genre() { Name = "hfg" });

            context.Plates.Add(new Plate() { Name = "qwe", CountOfTrecks = 50, LocalPrice = 505, GroupId = 1, MakerId = 1, GenreId = 1, IsAvailable = false, Price = 5005, PublishYear = 2000, SoldCopies = 50 });
            context.Plates.Add(new Plate() { Name = "Jets", CountOfTrecks = 50, LocalPrice = 505, GroupId = 1, MakerId = 1, GenreId = 1, IsAvailable = false, Price = 5005, PublishYear = 2000, SoldCopies = 50 });
            context.Plates.Add(new Plate() { Name = "Props", CountOfTrecks = 50, LocalPrice = 505, GroupId = 1, MakerId = 1, GenreId = 1, IsAvailable = false, Price = 5005, PublishYear = 2000, SoldCopies = 50 });

            context.Makers.Add(new Maker() { Name = "qwe" });
            context.Makers.Add(new Maker() { Name = "sdf" });
            context.Makers.Add(new Maker() { Name = "sdfg" });

            context.Groups.Add(new Group() { Name = "cvx" });
            context.Groups.Add(new Group() { Name = "asd" });
            context.Groups.Add(new Group() { Name = "casdvx" });

            context.Users.Add(new User() { Login = "lolo", Password = LoginRegisterService.ComputeSha256Hash("1"), IsAdmin = true });
            context.Users.Add(new User() { Login = "qwe", Password = LoginRegisterService.ComputeSha256Hash("qwe"), IsAdmin = false });
            context.Users.Add(new User() { Login = "login", Password = LoginRegisterService.ComputeSha256Hash("1234"), IsAdmin = false });

            context.SaveChanges();
        }
    }
}