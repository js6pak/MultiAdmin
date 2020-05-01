using System;
using Xunit;
using MultiAdmin.ServerIO;

namespace MultiAdmin.Tests.ServerIO
{
	public class ShiftingListTests
	{
		[Fact]
		public void ShiftingListTest()
		{
			const int maxCount = 2;
			var shiftingList = new ShiftingList(maxCount);

			Assert.Equal(shiftingList.MaxCount, maxCount);
		}

		[Fact]
		public void AddTest()
		{
			const int maxCount = 2;
			const int entriesToAdd = 6;
			var shiftingList = new ShiftingList(maxCount);

			for (var i = 0; i < entriesToAdd; i++)
			{
				shiftingList.Add($"Test{i}");
			}

			Assert.Equal(shiftingList.Count, maxCount);

			for (var i = 0; i < shiftingList.Count; i++)
			{
				Assert.Equal(shiftingList[i], $"Test{entriesToAdd - i - 1}");
			}
		}

		[Fact]
		public void RemoveFromEndTest()
		{
			const int maxCount = 6;
			const int entriesToRemove = 2;
			var shiftingList = new ShiftingList(maxCount);

			for (var i = 0; i < maxCount; i++)
			{
				shiftingList.Add($"Test{i}");
			}

			for (var i = 0; i < entriesToRemove; i++)
			{
				shiftingList.RemoveFromEnd();
			}

			Assert.Equal(shiftingList.Count, Math.Max(maxCount - entriesToRemove, 0));

			for (var i = 0; i < shiftingList.Count; i++)
			{
				Assert.Equal(shiftingList[i], $"Test{maxCount - i - 1}");
			}
		}

		[Fact]
		public void ReplaceTest()
		{
			const int maxCount = 6;
			const int indexToReplace = 2;
			var shiftingList = new ShiftingList(maxCount);

			for (var i = 0; i < maxCount; i++)
			{
				shiftingList.Add($"Test{i}");
			}

			for (var i = 0; i < maxCount; i++)
			{
				if (i == indexToReplace)
				{
					shiftingList.Replace("Replaced", indexToReplace);
				}
			}

			Assert.Equal(shiftingList.Count, maxCount);

			Assert.Equal("Replaced", shiftingList[indexToReplace]);
		}
	}
}
