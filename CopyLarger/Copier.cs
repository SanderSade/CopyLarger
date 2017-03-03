using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CopyLarger
{
	internal sealed class Copier
	{
		private readonly ArgsDto _options;
		private readonly CopyResult _result;
		private readonly List<FileInfo> _sourceFiles;


		internal Copier(ArgsDto options)
		{
			_options = options;
			_result = new CopyResult();
			_sourceFiles = new List<FileInfo>();
		}


		internal CopyResult Copy()
		{
			ProcessFolder(_options.Input);

			if (_sourceFiles.Count == 0)
			{
				Console.WriteLine($"No files found in the source folder {_options.Input.Name}");
				Utils.WriteExit(14);
				return _result;
			}

			_result.TotalInputSize = _sourceFiles.Sum(x => x.Length);
			_result.InputCount = _sourceFiles.Count;

			Console.WriteLine($"Found {_result.InputCount} files, total size {Utils.BytesToString(_result.TotalInputSize)}");

			foreach (var sourceFile in _sourceFiles)
			{
				CopyFile(sourceFile);
			}


			return _result;
		}


		private void CopyFile(FileInfo sourceFile)
		{
			//this is not a good way to do this, but I don't have a need for better replace atm
			var targetFile = sourceFile.FullName.Replace(_options.Input.FullName, _options.Output.FullName);

			bool isOverwrite = false;

			if (File.Exists(targetFile))
			{
				var targetInfo = new FileInfo(targetFile);

				if (targetInfo.Length >= sourceFile.Length)
					return;
				_result.Overwritten++;
				isOverwrite = true;
			}

			var outPath = Path.GetDirectoryName(targetFile);
			Directory.CreateDirectory(outPath);

			Console.WriteLine(
				$"{(isOverwrite ? "Overwriting" : "Copying")} {sourceFile.Name} ({Utils.BytesToString(sourceFile.Length)}) to {outPath}");

			File.Copy(sourceFile.FullName, targetFile, true);



			_result.TotalCopied++;
			_result.TotalCopiedSize += sourceFile.Length;
		}


		private void ProcessFolder(DirectoryInfo directoryInfo)
		{
			var folders = directoryInfo.EnumerateDirectories();

			foreach (var folder in folders)
			{
				ProcessFolder(folder);
			}

			var files = directoryInfo.GetFiles();
			_sourceFiles.AddRange(files);
		}
	}
}