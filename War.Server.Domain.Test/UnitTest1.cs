using MongoDB.Bson;
using War.Server.Domain.Items;
using War.Server.Domain.Items.PersonObjects;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.MapObjects;
using War.Server.Domain.ObjectSets;
using War.Server.Domain.Repositories;
using War.Server.Domain.Services;

namespace War.Server.Domain.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var items = ItemObjectSet.GetAll();

            var configuration = new Utility.Configuration()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "WarGame"
            };


            var appSettings = new AppSettings();

            var game = GameBoardRepository.GetAll().FirstOrDefault();
            game ??= new GameBoard() { Name = Guid.NewGuid().ToString().Split('-')[0] };

            var user = UserRepository.GetAll().FirstOrDefault(x => x.Email == "emrahdogru@gmail.com");

            if (user == null)
            {
                user = new();
                user.Email = "emrahdogru@gmail.com";
                user.SetPassword("Ed12345679");
                user.FirstName = "Emrah";
                user.LastName = "DOĞRU";
                user.IsActive = true;
                user.Language = Languages.English;
            }

            var player = PlayerRepository.GetAll().FirstOrDefault(x => x.UserId == user.Id);

            if (player == null)
            {
                player = new Player(game);
                player.User = user;
            }

            var city = CityRepository.GetAll().FirstOrDefault(x => x.GameBoardId == game.Id && x.UserId == user.Id);

            if(city == null)
                city = new City(game, user, new Point(10, 10));

            try
            {
                var doc = city.ToBsonDocument();
            }
            catch(Exception ex)
            {
                var x = ex;
            }

            var resources = new ItemCollection<Items.ItemObject>(new Dictionary<string, int>()
            {
                { nameof(Person), 100 },
                { nameof(Items.ResourceObjects.Coal), 100 },
                { nameof(Items.ResourceObjects.Food), 100 },
                { nameof(Items.ResourceObjects.Grain), 100 },
                { nameof(Items.ResourceObjects.Iron), 100 },
                { nameof(Items.ResourceObjects.Petrol), 100 },
                { nameof(Items.ResourceObjects.Stone), 100 },
                { nameof(Items.ResourceObjects.Water), 100 },
                { nameof(Items.ResourceObjects.Wood), 100 },
            });

            city.AddResources(resources);

            await GameBoardRepository.SaveAsync(game);
            await UserRepository.SaveAsync(user);
            await PlayerRepository.SaveAsync(player);
            await CityRepository.SaveAsync(city);


        }
    }
}