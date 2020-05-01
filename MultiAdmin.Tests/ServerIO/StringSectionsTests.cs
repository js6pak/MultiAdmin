using System;
using Xunit;
using MultiAdmin.ConsoleTools;
using MultiAdmin.ServerIO;
using Xunit.Sdk;

namespace MultiAdmin.Tests.ServerIO
{
	public class StringSectionsTests
	{
		private struct FromStringTemplate
		{
			public readonly string testString;
			public readonly string[] expectedSections;

			public readonly int sectionLength;
			public readonly ColoredMessage leftIndictator;
			public readonly ColoredMessage rightIndictator;

			public FromStringTemplate(string testString, string[] expectedSections, int sectionLength, ColoredMessage leftIndictator = null, ColoredMessage rightIndictator = null)
			{
				this.testString = testString;
				this.expectedSections = expectedSections;

				this.sectionLength = sectionLength;
				this.leftIndictator = leftIndictator;
				this.rightIndictator = rightIndictator;
			}
		}

		[Fact]
		public void FromStringTest()
		{
			try
			{
				StringSections.FromString("test string", 2, new ColoredMessage("."), new ColoredMessage("."));
				throw new XunitException("This case should not be allowed, no further characters can be output because of the prefix and suffix");
			}
			catch (ArgumentException)
			{
				// Expected behaviour
			}

			FromStringTemplate[] sectionTests =
			{
				new FromStringTemplate("test string", new string[] {"te", "st", " s", "tr", "in", "g"}, 2),
				new FromStringTemplate("test string", new string[] {"tes..", ".t ..", ".st..", ".ring"}, 5, new ColoredMessage("."), new ColoredMessage(".."))
			};

			for (var i = 0; i < sectionTests.Length; i++)
			{
				var sectionTest = sectionTests[i];

				var sections = StringSections.FromString(sectionTest.testString, sectionTest.sectionLength, sectionTest.leftIndictator, sectionTest.rightIndictator);

				Assert.NotNull(sections);
				Assert.NotNull(sections.Sections);

				Assert.True(sections.Sections.Length == sectionTest.expectedSections.Length, $"Failed at index {i}: Expected sections length \"{sectionTest.expectedSections.Length}\", got \"{sections.Sections.Length}\"");

				for (var j = 0; j < sectionTest.expectedSections.Length; j++)
				{
					var expected = sectionTest.expectedSections[j];
					var result = sections.Sections[j].Section.GetText();

					Assert.Equal(expected, result);//, $"Failed at index {i}: Failed at section index {j}: Expected section text to be \"{expected ?? "null"}\", got \"{result ?? "null"}\"");
				}
			}
		}
	}
}
