namespace CopyLarger
{
	internal sealed class CopyResult
	{
		internal int InputCount { get; set; }
		public int TotalCopied { get; set; }
		public int Overwritten { get; set; }
		public long TotalCopiedSize { get; set; }
		public long TotalInputSize { get; set; }
	}
}