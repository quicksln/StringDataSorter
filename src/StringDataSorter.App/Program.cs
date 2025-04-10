using HPCsharp;
using Spectre.Console;
using StringDataSorter.Core;
using StringDataSorter.Core.Helpers;
using StringDataSorter.Core.Comparer;
using System.Diagnostics;
using StringDataSorter.App.Settings;


class Program
{
    static async Task Main(string[] args)
    {
        try
        {
                // Prerequisites
                double creatingFileTime = 0;
                double readingFileTime = 0;
                double sortingTime = 0;
                double savingSortedDataTime = 0;
                uint fileSize = 0;

                var dataManager = new FileDataManager();
                var data = DataSampler.Get();
                var settings = ApplicationConfiguration.GetSettings();

                // Application 
                AnsiConsole.Write(new FigletText("Sorter App").LeftJustified().Color(Color.Cyan1));
                var rule = new Rule();
                rule.Style = Style.Parse("cyan2");
                AnsiConsole.Write(rule);
                AnsiConsole.MarkupLine("");

                fileSize = AnsiConsole.Ask<uint>("Provide file size in MB:");

                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine($"Creating data file ({fileSize} MB)...");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                await dataManager.CreateDataAsync(data, settings.FilePath!, fileSize);

                stopwatch.Stop();
                creatingFileTime = stopwatch.ElapsedMilliseconds / 1000d;
                AnsiConsole.MarkupLine($"[green bold]Completed: {stopwatch.ElapsedMilliseconds}ms.[/]");

                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine("Reading data from file...");
                stopwatch.Restart();

                var dataNotSorted = await dataManager.ReadDataAsync(settings.FilePath!);

                stopwatch.Stop();
                readingFileTime = stopwatch.ElapsedMilliseconds / 1000d;
                AnsiConsole.MarkupLine($"[green bold]Completed: {stopwatch.ElapsedMilliseconds}ms.[/]");

                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine("Sorting data...");
                stopwatch.Restart();

                var sortedData = dataNotSorted.SortMergePar<string>(comparer: new StringDataComparer());

                stopwatch.Stop();
                sortingTime = stopwatch.ElapsedMilliseconds / 1000d;
                AnsiConsole.MarkupLine($"[green bold]Completed: {stopwatch.ElapsedMilliseconds}ms.[/]");

                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine("Writing sorted data to file...");
                stopwatch.Restart();

                await dataManager.WriteDataAsync(settings.SortedFilePath!, sortedData);

                stopwatch.Stop();
                savingSortedDataTime = stopwatch.ElapsedMilliseconds / 1000d;
                AnsiConsole.MarkupLine($"[green bold]Completed: {stopwatch.ElapsedMilliseconds}ms.[/]");

                AnsiConsole.MarkupLine("Sorted data has been written successfully!");
                AnsiConsole.MarkupLine("");
                AnsiConsole.Write(rule);
                AnsiConsole.MarkupLine("");

                var panelResult = new Panel(new BarChart()
                    .Width(100)
                    .Label("[green bold underline]Program statistics (sec.)[/]")
                    .CenterLabel()
                    .AddItem($"Creating data file", creatingFileTime, Color.Yellow)
                    .AddItem($"Reading data file (1)", readingFileTime, Color.Blue)
                    .AddItem($"Sorting data (2)", sortingTime, Color.Red)
                    .AddItem($"Saving sorted data (3)", savingSortedDataTime, Color.Purple_2)
                    .AddItem($"Total (1+2+3)", Math.Round(readingFileTime + sortingTime + savingSortedDataTime, 3), Color.Purple_2))
                {
                    Header = new PanelHeader("Results"),
                    Expand = true
                };

                AnsiConsole.Write(panelResult);
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        } finally
        {
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}