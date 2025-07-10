using System;
using System.Runtime.CompilerServices;
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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to inspirited!");
            LoadStatistics();
            Console.WriteLine("Press Enter key to continue");
            ConsoleKeyInfo _key = Console.ReadKey();

            if (_key.Key == ConsoleKey.Enter)
            {
                Characters();
            }
            Console.WriteLine("Do you wish to create a character? Yes or No!");
            ConsoleKeyInfo isCreate = Console.ReadKey();
            if (isCreate.Key == ConsoleKey.Y)
            {
                CreatePlayer();
                GameStart();
            }
            else if (isCreate.Key == ConsoleKey.N)
            {
                GameStart();
            }
        }

        private static void LoadStatistics()
        {
            using (var dbContext = new ISDataContext())
            {
                if (!dbContext.Characters.Any())
                {
                    var characters = new List<Character>()
                    {
                        new Character {Id=1, Type = "Warrior", Details="A long sworded person"},
                        new Character {Id=2,Type="Archer", Details="Arrow and Bow person"},
                        new Character {Id=3,Type="Mage", Details="Spell Caster"}
                    };
                    dbContext.Characters.AddRange(characters);
                    dbContext.SaveChanges();
                }
                if (!dbContext.Statistics.Any())
                {
                    var stats = new List<Statistics>()
                    {
                        new Statistics {CharacterId=1, Charisma=10, Constitution=10, Strength=10, Dexterity=0, Intelligence=0,Wisdom=0},
                        new Statistics {CharacterId=2, Charisma=10, Constitution=0, Strength=10, Dexterity=10, Intelligence=0,Wisdom=0},
                        new Statistics {CharacterId=3, Charisma=10, Constitution=0, Strength=0, Dexterity=0, Intelligence=10,Wisdom=10}
                    };

                    dbContext.Statistics.AddRange(stats);
                    dbContext.SaveChanges();
                }
            }
        }

        private static void GameStart()
        {
            Console.WriteLine("Game Start!");
            string result;
            do
            {
                Console.Write("Enter your ID: ");
                int _id = Convert.ToInt16(Console.ReadLine());
                result = GetPlayerName(_id);
            } while (result == string.Empty);
            Console.WriteLine($"Welcome to Inspirited, {result}");

        }
        static string GetPlayerName(int id)
        {
            using (var dbContext = new ISDataContext())
                {
                    var player = dbContext.Players.Where(c => c.Id == id).Select(p => p.PlayerName).FirstOrDefault();
                    return player ?? string.Empty;
                }
        }
        private static void CreatePlayer()
        {
            Console.Write("Enter your name: ");
            string _name = Console.ReadLine();
            Console.Write("Character ID:");
            int _id = Convert.ToInt16(Console.ReadLine());
            var player = new Player
            {
                PlayerName = _name,
                CharacterId = _id
            };

            using (var dbContext = new ISDataContext())
            {
                dbContext.Players.Add(player);
                dbContext.SaveChanges();
                Console.WriteLine("Character Successfully Created!");
            }
        }

        private static void Characters()
        {
            Console.WriteLine("List of characters:");
            using (var dbContext = new ISDataContext())
            {
                var characters = dbContext.Characters;
                foreach (var character in characters)
                {
                    Console.WriteLine(character.Id + "\t" + character.Type);
                }
            }
        }
    }
}