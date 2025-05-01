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
        view.DisplayBars(results, faces, rolls);
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

        public void DisplayBars(List<int> results, int faces, int rolls)
        {
            Console.Clear();
            Console.WriteLine($"You rolled a {faces}-sided die {rolls} times.\n");
            Random random = new Random();
            Console.WriteLine("Results: ");
            int maxLength = results.Max();
            for (int i = 0; i < results.Count; i++)
            {
                string start = $"Face {i + 1}: ";
                int padding = 10 - start.Length; // Adjust padding based on the length of the start string
                Console.Write(start + new string(' ', padding)); // Add spaces for alignment
                int length = (int)((double)results[i] / maxLength * 50); // Scale to 50 characters
                string times = $" {results[i]} times";
                int barLength = Math.Max(0, length - times.Length);
                Console.BackgroundColor = ConsoleColorHelper.GetUniqueRandomColor(); // Random background color
                Console.ForegroundColor = ConsoleColor.Black; // Text color
                Console.Write(times);
                Console.Write(new string(' ', barLength)); // Fill the rest with spaces
                Console.ResetColor(); // Reset colors
                Console.WriteLine(); // Move to the next line
            }
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
