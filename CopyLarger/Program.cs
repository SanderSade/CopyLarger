using System;
using System.IO;
using System.Linq;

namespace CopyLarger
{
	internal static class Program
	{
		private const string Banner = @"Copy utility that overwrites existing smaller files with larger files, and non-existing files same as a normal copy.
https://github.com/SanderSade/CopyLarger
";


		private static void Main(string[] args)
		{
			Console.WriteLine(Banner);

			var options = ParseInput(args);

			if (options?.Input == null || options.Output == null)
				return;

			var startTime = DateTimeOffset.UtcNow;
			Console.WriteLine($"Starting copy at {DateTimeOffset.Now}");

			var copier = new Copier(options);

			var result = copier.Copy();
			Console.WriteLine(Environment.NewLine);
			Console.WriteLine($"Finished copy at {DateTimeOffset.Now}");
			Console.WriteLine($"Total time: {DateTimeOffset.UtcNow - startTime}");
			Console.WriteLine($"Total files in input: {result.InputCount}");
			Console.WriteLine($"Total size in input: {Utils.BytesToString(result.TotalInputSize)}");
			Console.WriteLine($"Total files copied: {result.TotalCopied}");
			Console.WriteLine($"Total size copied: {Utils.BytesToString(result.TotalCopiedSize)}");
			Console.WriteLine($"Smaller files overwritten: {result.Overwritten}");

			Utils.WriteExit(0);
		}


		private static ArgsDto ParseInput(string[] args)
		{
			if (args == null || args.Length != 2 || args.Any(x => x.Contains("?")))
			{
				Console.WriteLine(Environment.NewLine);
				Console.WriteLine("Use: CopyLarger \"<source folder>\" \"<target folder>\"");
				Console.WriteLine(Environment.NewLine);
				Console.WriteLine("CopyLarger does not check if the target folder has enough room, nor if the file content differs when the sizes are equal.");
				Console.WriteLine("Files and folders will be copied with the same structure as source folder has");
				Utils.WriteExit(1);
				return null; //make code validation happy
			}

			var options = new ArgsDto();

			if (Directory.Exists(args[0]))
				options.Input = new DirectoryInfo(args[0]);
			else
			{
				Console.WriteLine($"Invalid input folder: {args[0]}");
				Utils.WriteExit(12);
			}

			if (Directory.Exists(args[1]))
				options.Output = new DirectoryInfo(args[1]);
			else
			{
				Console.WriteLine($"Invalid output folder: {args[1]}");
				Utils.WriteExit(13);
			}

			return options;
		}
	}
}