using FNFToOsuManiaConverter;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length > 0) //drag and drop json file to the exe
        {
            if(!string.IsNullOrEmpty(args[0]))
            {
                string filePath = args[0];
                FNFChart fnfChart = new FNFChart(filePath);
                fnfChart.ParseShitAndSaveFile(filePath);
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
                    fnfChart.ParseShitAndSaveFile(dir);
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