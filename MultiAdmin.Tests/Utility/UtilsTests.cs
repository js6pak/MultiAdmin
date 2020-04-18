using System;
using MultiAdmin.Utility;
using Xunit;
using Xunit.Sdk;

namespace MultiAdmin.Tests.Utility
{
	public class UtilsTests
	{
		private struct StringMatchingTemplate
		{
			public readonly string input;
			public readonly string pattern;

			public readonly bool expectedResult;

			public StringMatchingTemplate(string input, string pattern, bool expectedResult)
			{
				this.input = input;
				this.pattern = pattern;
				this.expectedResult = expectedResult;
			}
		}

		private struct CompareVersionTemplate
		{
			public readonly string firstVersion;
			public readonly string secondVersion;

			public readonly int expectedResult;

			public CompareVersionTemplate(string firstVersion, string secondVersion, int expectedResult)
			{
				this.firstVersion = firstVersion;
				this.secondVersion = secondVersion;
				this.expectedResult = expectedResult;
			}

			public bool CheckResult(int result)
			{
				if (expectedResult == result)
					return true;

				if (expectedResult < 0 && result < 0)
					return true;

				if (expectedResult > 0 && result > 0)
					return true;

				return false;
			}
		}

		[Fact]
		public void GetFullPathSafeTest()
		{
			string result = Utils.GetFullPathSafe(" ");
			Assert.Null(result);
		}

		[Fact]
		public void StringMatchesTest()
		{
			StringMatchingTemplate[] matchTests =
			{
				new StringMatchingTemplate("test", "*", true),
				new StringMatchingTemplate("test", "te*", true),
				new StringMatchingTemplate("test", "*st", true),
				new StringMatchingTemplate("test", "******", true),
				new StringMatchingTemplate("test", "te*t", true),
				new StringMatchingTemplate("test", "t**st", true),
				new StringMatchingTemplate("test", "s*", false),
				new StringMatchingTemplate("longstringtestmessage", "l*s*t*e*g*", true),
				new StringMatchingTemplate("AdminToolbox", "config_remoteadmin.txt", false),
				new StringMatchingTemplate("config_remoteadmin.txt", "config_remoteadmin.txt", true),
				new StringMatchingTemplate("sizetest", "sizetest1", false)
			};

			for (int i = 0; i < matchTests.Length; i++)
			{
				try
				{
					StringMatchingTemplate test = matchTests[i];

					bool result = Utils.StringMatches(test.input, test.pattern);

					Assert.True(test.expectedResult == result, $"Failed on test index {i}: Expected \"{test.expectedResult}\", got \"{result}\"");
				}
				catch (Exception e)
				{
					throw new XunitException($"Failed on test index {i}: {e}");
				}
			}
		}

		[Fact]
		public void CompareVersionStringsTest()
		{
			CompareVersionTemplate[] versionTests =
			{
				new CompareVersionTemplate("1.0.0.0", "2.0.0.0", -1),
				new CompareVersionTemplate("1.0.0.0", "1.0.0.0", 0),
				new CompareVersionTemplate("2.0.0.0", "1.0.0.0", 1),

				new CompareVersionTemplate("1.0", "2.0.0.0", -1),
				new CompareVersionTemplate("1.0", "1.0.0.0", -1), // The first version is shorter, so it's lower
				new CompareVersionTemplate("2.0", "1.0.0.0", 1),

				new CompareVersionTemplate("1.0.0.0", "2.0", -1),
				new CompareVersionTemplate("1.0.0.0", "1.0", 1), // The first version is longer, so it's higher
				new CompareVersionTemplate("2.0.0.0", "1.0", 1),

				new CompareVersionTemplate("6.0.0.313", "5.18.0", 1),
				new CompareVersionTemplate("5.18.0", "6.0.0.313", -1),

				new CompareVersionTemplate("5.18.0", "5.18.0", 0),
				new CompareVersionTemplate("5.18", "5.18.0", -1) // The first version is shorter, so it's lower
			};

			for (int i = 0; i < versionTests.Length; i++)
			{
				CompareVersionTemplate test = versionTests[i];

				int result = Utils.CompareVersionStrings(test.firstVersion, test.secondVersion);

				Assert.True(test.CheckResult(result), $"Failed on test index {i}: Expected \"{test.expectedResult}\", got \"{result}\"");
			}
		}
	}
}
