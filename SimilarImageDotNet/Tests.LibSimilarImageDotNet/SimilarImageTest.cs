using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibSimilarImageDotNet;

namespace Tests.SimilarImageDotNet
{
	[TestClass]
	public class SimilarImageTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			var Path = "../../../../Samples";
			var Hash1_a = SimilarImage.GetCompressedImageHashAsString(new Bitmap(Image.FromFile(Path + "/test1_a.png")));
			var Hash1_b = SimilarImage.GetCompressedImageHashAsString(new Bitmap(Image.FromFile(Path + "/test1_b.png")));
			var Hash1_c = SimilarImage.GetCompressedImageHashAsString(new Bitmap(Image.FromFile(Path + "/test1_c.png")));

			var Hash2 = SimilarImage.GetCompressedImageHashAsString(new Bitmap(Image.FromFile(Path + "/test2.png")));

			var Hash3_a = SimilarImage.GetCompressedImageHashAsString(new Bitmap(Image.FromFile(Path + "/test3_a.jpg")));
			var Hash3_b = SimilarImage.GetCompressedImageHashAsString(new Bitmap(Image.FromFile(Path + "/test3_b.jpg")));

			Assert.IsTrue(SimilarImage.CompareHashesBool(Hash1_a, Hash1_b));
			Assert.IsTrue(SimilarImage.CompareHashesBool(Hash1_a, Hash1_c));

			Assert.IsFalse(SimilarImage.CompareHashesBool(Hash1_a, Hash2));
			Assert.IsFalse(SimilarImage.CompareHashesBool(Hash1_a, Hash3_a));

			Assert.IsTrue(SimilarImage.CompareHashesBool(Hash3_a, Hash3_b));
			Assert.IsFalse(SimilarImage.CompareHashesBool(Hash3_a, Hash2));

			Assert.AreEqual(SimilarImage.CompareHashes(Hash1_a, Hash1_b), SimilarImage.CompareHashes(Hash1_b, Hash1_a));
		}
	}
}
