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
		private readonly string _outFolder;
		private readonly string _inputFolder;


		internal Copier(ArgsDto options)
		{
			_options = options;
			_result = new CopyResult();
			_sourceFiles = new List<FileInfo>();
			_outFolder = _options.Output.FullName + "\\";
			_inputFolder = _options.Input.FullName;
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
			var targetFile = sourceFile.FullName.Replace(_inputFolder, _outFolder);

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
			var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
			_sourceFiles.AddRange(files);
		}
	}
}