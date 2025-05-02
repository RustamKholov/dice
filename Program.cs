using Spectre.Console;
namespace dice;

class Program
{
    static void Main(string[] args)
    {
        DiceRoller diceRoller = new DiceRoller();
        View view = new View(new Input());
        view.WelcomeMessage();
        int faces = view.GetFaces();
        int rolls = view.GetRolls();
        var results = diceRoller.RollMultipleList(faces, rolls);
        Console.Clear();
        view.DisplayUsualConsole();
        view.DisplayBars(results, faces, rolls);
        Console.WriteLine();
        view.DisplaySpectreConsole();
        view.DisplayBarsSpectre(results, faces, rolls);
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
    public class Input()
    {
        public int GetInt(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";
            int result;
            while (!int.TryParse(input, out result) || result <= 0)
            {
                // Check if the input is a number and greater than 0
                Console.Write("Invalid input. Please enter a number greater than 0: ");
                input = Console.ReadLine() ?? "";
            }
            return result;
        }
    }
    public class View(Input input)
    {
        public void WelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Dice Roller!\n");
        }
        public int GetFaces()
        {
            Console.WriteLine("How many sides does your die have?");
            return input.GetInt("Enter the number of sides: ");
        }
        public int GetRolls()
        {
            Console.WriteLine("How many times do you want to roll the die?");
            return input.GetInt("Enter the number of rolls: ");
        }
        public void DisplayUsualConsole(){
            Console.WriteLine(new string ('=', 50));
            Console.WriteLine("// Basic Console Display //");
        }
        public void DisplaySpectreConsole(){
            Console.WriteLine(new string ('=', 50));
            Console.WriteLine("// Spectre.Console Display //");
        }
        public void DisplayBars(List<int> results, int faces, int rolls)
        {
            Console.WriteLine($"You rolled a {faces}-sided die {rolls} times.\n");
            Console.WriteLine("Results: ");
            int maxLength = results.Max();
            for (int i = 0; i < results.Count; i++)
            {
                string start = $"Face {i + 1}: ";
                int padding = 10 - start.Length; // Adjust padding based on the length of the start string
                Console.Write(start + new string(' ', padding)); // Add spaces for alignment
                int timesCount = results[i];
                int length = (int)((double)timesCount / maxLength * 50); // Scale to 50 characters
                string times = $" {timesCount} times";
                int barLength = Math.Max(0, length - times.Length);
                if (timesCount == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray; // Text color for zero count
                    Console.Write(times);
                    Console.WriteLine();
                    Console.ResetColor(); // Reset colors

                }
                else
                {
                    Console.BackgroundColor = ConsoleColorHelper.GetUniqueRandomColor(); // Random background color
                    Console.ForegroundColor = ConsoleColor.Black; // Text color
                    Console.Write(times);
                    Console.Write(new string(' ', barLength)); // Fill the rest with spaces
                    Console.ResetColor(); // Reset colors
                    Console.WriteLine(); // Move to the next line
                }

            }
        }
        public void DisplayBarsSpectre(List<int> results, int faces, int rolls){
            
            var bar = new BarChart()
                .Width(100)
                .Label($"You rolled a {faces}-sided die {rolls} times.\n")
                .CenterLabel()
                .AddItems(results.Select((result, index) => new BarChartItem($"Face {index + 1}", result, ConsoleColorHelper.GetUniqueRandomColor())));
            AnsiConsole.Write(bar);
        }
    }
    public class DiceRoller()
    {

        public int Roll(int faces)
        {
            Random random = new Random();
            return random.Next(faces);
        }
        public List<int> RollMultipleList(int faces, int rolls)
        {
            List<int> results = new List<int>(faces);
            // Initialize the list with zeros
            for (int i = 0; i < faces; i++)
            {
                results.Add(0);
            }
            // Increment the count of the rolled number
            for (int i = 0; i < rolls; i++)
            {
                results[Roll(faces)]++;
            }
            return results;
        }
    }

    public static class ConsoleColorHelper
    {
        private static List<ConsoleColor> availableColors = new List<ConsoleColor>();
        private static Random random = new Random();

        public static ConsoleColor GetUniqueRandomColor()
        {
            if (availableColors.Count == 0)
            {
                foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                {
                    // skip Black
                    if (color != ConsoleColor.Black)
                        availableColors.Add(color);
                }
            }

            int index = random.Next(availableColors.Count);
            ConsoleColor selectedColor = availableColors[index];
            availableColors.RemoveAt(index);
            return selectedColor;
        }
    }
}
