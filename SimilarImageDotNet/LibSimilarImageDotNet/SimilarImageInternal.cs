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
	unsafe public class SimilarImageInternal
	{
		/// <summary>
		/// 
		/// </summary>
		public const String HexCharacters = "0123456789ABCDEF";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FullImage"></param>
		/// <param name="Side"></param>
		/// <returns></returns>
		static public Bitmap GetThumbnail(Bitmap FullImage, int Side = 64, bool Grayscale = false)
		{
			var Thumb = new Bitmap(Side, Side);
			var ThumbGraphics = Graphics.FromImage(Thumb);
			ThumbGraphics.CompositingMode = CompositingMode.SourceCopy;
			ThumbGraphics.CompositingQuality = CompositingQuality.HighQuality;
			ThumbGraphics.DrawImage(FullImage, new Rectangle(0, 0, Side, Side), new Rectangle(0, 0, FullImage.Width, FullImage.Height), GraphicsUnit.Pixel);
			if (Grayscale) ToGrayscaleInplace(Thumb);
			return Thumb;
			//FullImage
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Bitmap"></param>
		static private void ToGrayscaleInplace(Bitmap Bitmap)
		{
			int Width = Bitmap.Width, Height = Bitmap.Height;
			var BitmapData = Bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			for (int y = 0; y < Height; y++)
			{
				var Ptr = ((byte*)BitmapData.Scan0.ToPointer()) + y * BitmapData.Stride;
				for (int x = 0; x < Width; x++)
				{
					byte Luminance = (byte)((Ptr[0] + Ptr[1] + Ptr[2]) / 3);
					if (Ptr[3] < 0x80) Luminance = 0;
					Ptr[0] = Luminance;
					Ptr[1] = Luminance;
					Ptr[2] = Luminance;
					Ptr[3] = 0xFF;
					Ptr += 4;
				}
			}
			Bitmap.UnlockBits(BitmapData);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FullImage"></param>
		/// <returns></returns>
		static public byte[] GetImageLevelHash(Bitmap LastLevelImage)
		{
			int Width = LastLevelImage.Width, Height = LastLevelImage.Height;
			var Result = new MemoryStream();
			var Absolute = GetAbsoluteImageIntensity(LastLevelImage);
			var Relative = GetRelativeImageIntensity(Absolute);
			for (int y = 0; y < Height; y++) for (int x = 0; x < Width; x++) Result.WriteByte((byte)Relative[x, y]);
			return Result.ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Absolute"></param>
		/// <returns></returns>
		static public BrighterDirection[,] GetRelativeImageIntensity(byte[,] Absolute)
		{
			int Width = Absolute.GetLength(0), Height = Absolute.GetLength(1);
			var Relative = new BrighterDirection[Width, Height];
			int[] XList = new int[3], YList = new int[3];

			for (int CurrentY = 0; CurrentY < Height; CurrentY++)
			{
				YList[0] = (CurrentY == 0) ? (Height - 1) : (CurrentY - 1);
				YList[1] = CurrentY;
				YList[2] = (CurrentY == Height - 1) ? (0) : (CurrentY + 1);

				for (int CurrentX = 0; CurrentX < Width; CurrentX++)
				{
					XList[0] = (CurrentX == 0) ? (Width - 1) : (CurrentX - 1);
					XList[1] = CurrentX;
					XList[2] = (CurrentX == Width - 1) ? (0) : (CurrentX + 1);

					byte CurrentRelative = 0;
					var Current = Absolute[CurrentX, CurrentY];

					byte Bit = 1;
					foreach (var NearY in YList) foreach (var NearX in XList)
						{
							// Ignore Center
							if (NearX == 0 && NearY == 0) continue;

							var Near = Absolute[NearX, NearY];
							if (Current > Near)
							{
								CurrentRelative |= Bit;
							}
							Bit <<= 1;
						}

					Relative[CurrentX, CurrentY] = (BrighterDirection)CurrentRelative;
				}
			}
			return Relative;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Image"></param>
		/// <returns></returns>
		static public byte[,] GetAbsoluteImageIntensity(Bitmap Image)
		{
			int Width = Image.Width, Height = Image.Height;
			var Matrix = new byte[Width, Height];
			var BitmapData = Image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			{
				for (int y = 0; y < Height; y++)
				{
					var Ptr = ((byte*)BitmapData.Scan0.ToPointer()) + BitmapData.Stride * y;
					for (int x = 0; x < Width; x++)
					{
						Matrix[x, y] = (byte)((Ptr[0] + Ptr[1] + Ptr[2]) / 3);
						Ptr += 3;
					}
				}
			}
			Image.UnlockBits(BitmapData);
			return Matrix;
		}
	}
}
