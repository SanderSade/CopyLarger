using System.IO;

namespace CopyLarger
{
	internal sealed class ArgsDto
	{
		internal DirectoryInfo Input { get; set; }
		internal DirectoryInfo Output { get; set; }
	}
}