using System;
using Microsoft.EntityFrameworkCore;

namespace inspirited
{
    public class Character
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public Statistics Stats { get; set; }
    }
    public class Statistics
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Constitution { get; set; }
        public int Dexterity { get; set; }
        public int Charisma { get; set; }
    }
    public class Player
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int CharacterId { get; set; }
    }
    public class ISDataContext : DbContext
    {
        string pomeloString = "server=localhost;database=InspiritedDb;user=root;password=passw0rd";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(pomeloString, ServerVersion.AutoDetect(pomeloString));
        }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Player> Players { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to inspirited!");
            LoadingCharacters();
            LoadingStats();
            Characters();
        }

        private static async void Characters()
        {
            Console.WriteLine("List of Characters");
            using (var dbContext = new ISDataContext())
            {
                var characters = dbContext.Characters;
                foreach (var character in characters)
                {
                    Console.WriteLine(character.Id+ "\t" +character.Type);
                }
            }
            Console.WriteLine("Do you want to create your character? Y(es) or N(o)!");
            char isCreate = Convert.ToChar(Console.ReadLine());
            if (isCreate == 'Y' || isCreate =='y')
            {
                CreatePlayer();
            }
            else
            {
                Console.WriteLine("Enter player name:");
                string playerName = Console.ReadLine();
                using (var dbContext = new ISDataContext())
                {
                    var playerwithName = dbContext.Players.SingleOrDefault(c => c.PlayerName == playerName);
                    if (playerwithName is null)
                    {
                        Console.WriteLine("Player Not Found");
                    }
                    else
                    {
                        IsStart(playerwithName.Id);
                    }
                }
            }
        }

        private async static void CreatePlayer()
        {
                Console.WriteLine("Enter Player Name:");
                string playerName = Console.ReadLine();
                Console.WriteLine("Enter selected Character ID:");
                int charactedId = Convert.ToInt16(Console.ReadLine());
                using (var dbContext = new ISDataContext())
                {
                    var playerwithSameName = dbContext.Players.FirstAsync(c => c.PlayerName == playerName);
                    if (playerwithSameName is not null) return;
                    var player = new Player()
                    {
                        PlayerName = playerName,
                        CharacterId = charactedId
                    };
                    dbContext.Add(player);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine(player.PlayerName +" is created.");
                    IsStart(player.Id);
                }
        }

        private static void IsStart(int Id)
        {
                    Console.WriteLine("Do you want to start? Y(es) or N(o)!");
                    char isStart = Convert.ToChar(Console.ReadLine());
                    if (isStart == 'Y' || isStart == 'y')
                    {
                        GameStart(Id);
                    }
                    else
                    {
                        return;
                    }
        }

        private static void GameStart(int Id)
        {
            LoadPlayer(Id);
        }

        private static void LoadPlayer(int Id)
        {
            using (var dbContext = new ISDataContext())
            {
                var playerwithId = dbContext.Players.Find(Id);
                if (playerwithId is null) return;
            }
        }

        private static void LoadingStats()
        {
            using (var dbContext = new ISDataContext())
            {
                if (dbContext.Statistics.Any())
                {
                    return;
                }
                var stat = new Statistics
                {
                    CharacterId = 1,
                    Constitution = 25,
                    Strength = 25,
                    Wisdom = 25,
                    Intelligence = 25,
                    Dexterity = 25,
                    Charisma = 25
                };
            }
        }

        private static async void LoadingCharacters()
        {
            using (var dbContext = new ISDataContext())
            {
                if (dbContext.Characters.Any())
                {
                    return;
                }

                var character = new Character
                {
                    Type = "Swordsman",
                    Details = "A person with swords"
                };
                dbContext.Add(character);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}