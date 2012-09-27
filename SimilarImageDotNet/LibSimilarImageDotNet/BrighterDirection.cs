using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSimilarImageDotNet
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum BrighterDirection : byte
	{
		TopLeft = (1 << 0),
		Top = (1 << 1),
		TopRight = (1 << 2),
		Left = (1 << 3),
		Right = (1 << 4),
		BottomLeft = (1 << 5),
		Bottom = (1 << 6),
		BottomRight = (1 << 7),
	}
}
