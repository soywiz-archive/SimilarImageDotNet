using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSimilarImageDotNet
{
	public class SimilarImage
    {
		const int DefaultMaxLevels = 4;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="FullImage"></param>
		/// <param name="Levels"></param>
		/// <returns></returns>
		static public String GetCompressedImageHashAsString(Bitmap FullImage, int Levels = DefaultMaxLevels)
		{
			string Result = "";
			foreach (var Byte in GetImageHash(FullImage, Levels))
			{
				Result += SimilarImageInternal.HexCharacters[(Byte & 0xF)];
			}
			return Result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FullImage"></param>
		/// <param name="Levels"></param>
		/// <returns></returns>
		static public byte[] GetImageHash(Bitmap FullImage, int Levels = DefaultMaxLevels)
		{
			var Result = new MemoryStream();
			Bitmap SmallImage = SimilarImageInternal.GetThumbnail(FullImage, 128, Grayscale: true);
			//SmallImage.Save(@"c:\temp\test-" + 0 + ".png");
			for (int Level = 1; Level <= Levels; Level++)
			{
				var LevelSide = GetLevelSide(Level);
				var LastLevelImage = SimilarImageInternal.GetThumbnail(SmallImage, LevelSide);
				//LastLevelImage.Save(@"c:\temp\test-" + Level + ".png");
				var Bytes = SimilarImageInternal.GetImageLevelHash(LastLevelImage);
				Result.Write(Bytes, 0, Bytes.Length);
			}
			return Result.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Level"></param>
		/// <returns></returns>
		static private int GetLevelSide(int Level)
		{
			return (int)Math.Pow(2, Level);
		}

		private static int CountBits(uint Bits)
		{
			int Count = 0;
			while (Bits != 0)
			{
				if ((Bits & 1) != 0) Count++;
				Bits >>= 1;
			}
			return Count;
		}

		private static int CountEqualBits(uint Bits1, uint Bits2, int Total)
		{
			int Count = 0;
			for (int n = 0; n < Total; n++)
			{
				int Mask = (1 << n);
				if ((Bits1 & Mask) == (Bits2 & Mask)) Count++;
			}
			return Count;
		}

		public static bool CompareHashesBool(string Hash1, string Hash2, double Threeshold = 0.9)
		{
			return CompareHashes(Hash1, Hash2) >= Threeshold;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Hash1"></param>
		/// <param name="Hash2"></param>
		/// <returns></returns>
		public static float CompareHashes(string Hash1, string Hash2)
		{
			int Levels = 16;
			int n = 0;
			float Similarity = 1;
			for (int Level = 1; Level <= Levels; Level++)
			{
				var LevelSide = GetLevelSide(Level);
				int TotalSimilar = LevelSide * LevelSide * 4; // Number of bits per pixel
				int SumSimilar = 0;
				for (int y = 0; y < LevelSide; y++) for (int x = 0; x < LevelSide; x++)
				{
					//if (Hash1[n] != Hash2[n])
					{
						var C1 = Convert.ToUInt32("" + Hash1[n], 16);
						var C2 = Convert.ToUInt32("" + Hash2[n], 16);
						var BitsEqual = CountEqualBits(C1, C2, 4);
						SumSimilar += BitsEqual;
						//Console.Write(Distinct);
					}
					n++;
				}
				float LevelSimilar = (float)((double)SumSimilar / (double)TotalSimilar);
				Similarity = Math.Min(Similarity, LevelSimilar);

				if (n >= Hash1.Length || n >= Hash2.Length) break;
			}
			return Similarity;
		}
	}
}
