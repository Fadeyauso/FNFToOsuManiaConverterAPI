using FNFToOsuManiaConverter;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("0-Opponent, 1-BF, 2-Both.");
        Console.WriteLine("Choose a side: ");
        string side = Console.ReadLine();
        if (args.Length > 0) //drag and drop json file to the exe
        {
            if(!string.IsNullOrEmpty(args[0]))
            {
                string filePath = args[0];
                try
                {
                    FNFChart fnfChart = new FNFChart(filePath);
                    fnfChart.ParseShitAndSaveFile(filePath, Convert.ToInt32(side));
                    Console.WriteLine("Done");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed parse with: {e} error.");
                }
            }
        }
        else
        {
            Console.WriteLine("Path to the song JSON file: ");
            string dir = Console.ReadLine();
            if(!string.IsNullOrEmpty(dir))
            {
                try
                {
                    FNFChart fnfChart = new FNFChart(dir);
                    fnfChart.ParseShitAndSaveFile(dir, Convert.ToInt32(side));
                    Console.WriteLine("Done");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Failed parse with: {e} error.");
                }
            }
        }
    }
}