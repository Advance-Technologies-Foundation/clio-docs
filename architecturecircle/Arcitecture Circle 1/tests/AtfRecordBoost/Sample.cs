using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace AtfRecordBoost.Tests
{
	[MockSettings(RequireMock.All)]
	[TestFixture(Category = "UnitTests")]
	public class SampleTest : BaseMarketplaceTestFixture
	{

		#region Methods: Protected
		protected override void SetUp(){
			base.SetUp();
			ISample sut = new Sample();
			ClassFactory.RebindWithFactoryMethod(() => sut);
			SetupCurrentUser();
			ClassFactory.RebindWithFactoryMethod(() => (UserConnection)UserConnection);
		}

		#endregion

		[Test(Description = "Validate that no tests without any dependencies run successfully")]
		public void AddTest_Returns_Result(){
			//Arrange
			ISample sample = ClassFactory.Get<ISample>();

			//Act
			int result = sample.Add(1, 2);

			//Assert
			result.Should().Be(3);
		}

		[Test(Description = "Validate that tests with dependencies run successfully")]
		public void GetCurrentUserNameTest_Returns_UserName(){
			//Arrange
			ISample sample = ClassFactory.Get<ISample>();

			//Act
			string result = sample.GetCurrentUserName();

			//Assert
			result.Should().Be("Supervisor");
		}

	}
}