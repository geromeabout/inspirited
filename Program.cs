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
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to inspirited!");
            Console.WriteLine("Press Enter key to continue");
            ConsoleKeyInfo _key = Console.ReadKey();

            if (_key.Key == ConsoleKey.Enter)
            {
                Characters();
            }
            Console.WriteLine("Do you wish to create a character? Yes or No!");
            char isCreate = Convert.ToChar(Console.ReadLine());
            switch (isCreate)
            {
                case 'Y':
                    CreatePlayer();
                    break;
                case 'y':
                    CreatePlayer();
                    break;
                case 'N':
                    CreatePlayer();
                    break;
                default:
                    break;
            }
            GameStart();
        }

        private static void GameStart()
        {
            Console.WriteLine("Game Start!");
            Console.Write("Enter your ID: ");
            int _id = Convert.ToInt16(Console.ReadLine());
            using (var dbContext = new ISDataContext())
            {
                var player = dbContext.Players.Find(_id);
                if (player == null)
                {
                Console.WriteLine("Not found!");
                }
                else
                {
                Console.WriteLine(player.PlayerName + "\t");
                }
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