using System;
using System.Collections.Generic;
using System.IO;
using AtfRecordBoost.FileParser;
using FluentAssertions;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace AtfRecordBoost.Tests.FileParser
{
	[TestFixture (Category = "UnitTests")]
	[MockSettings(RequireMock.All)]
	public class ParseYamlTests : BaseMarketplaceTestFixture
	{

		protected override void SetUp(){
			base.SetUp();
			IParser parser = new ParserYaml();
			ClassFactory.RebindWithFactoryMethod(()=>parser,"Yaml");
		}
		
		[TestCase("RecordBoost-Simple.yaml")]
		public void TestOne(string fileName){
			//Arrange
			Guid contactTypeId = Guid.NewGuid();
			string contactTypeName = "MockType";
			MockContactTypeEntity(contactTypeId, contactTypeName);
			MockContact();
			
			IParser parser = ClassFactory.Get<IParser>("Yaml");
			string fileContent = System.IO.File.ReadAllText(Path.Combine("SampleFiles","ParserYaml",fileName));
			ClassFactory.RebindWithFactoryMethod(()=>(UserConnection)UserConnection);

			//Act
			Dictionary<string, object> result = parser.Parse(fileContent);

			//Assert
			result["Name"].Should().Be("Kirill Krylov");
			result["Email"].Should().Be("email@creatio.com");
			result["Phone"].Should().Be("+14165438857");
			result["SomeGuid"].Should().Be(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));
			result["SomeFloat"].Should().Be(3.14159);
			result["SomeDateTime"].Should().Be(new DateTime(2020, 12, 31, 23, 59,59, DateTimeKind.Utc));
			result["SomeDate"].Should().Be(new DateTime(2020, 12, 31, 23, 59,59, DateTimeKind.Utc).Date);
			result["SomeDateTime"].Should().Be(new DateTime(2020, 12, 31, 23, 59,59, DateTimeKind.Utc).ToUniversalTime());
			result["ContactTypeId"].Should().Be(contactTypeId);
		}
		
		private void MockContactTypeEntity(Guid contactTypeId, string name){
			MockEntitySchemaWithColumns3("ContactType", new Dictionary<string, DataValueType> {
				{"Name", DataValueType.Text}
			});
			var ct = SubstituteEmptyEntity("ContactType");
			ct.PrimaryColumnValue = contactTypeId;
			ct.PrimaryDisplayColumnValue = name;
			SetUpTestData("ContactType", new Dictionary<string, object> {
				{ "Id", contactTypeId },
				{ "Name", name }
			});
		}
		
		private void MockContact(){
			var columns = new Dictionary<string, DataValueType>() {
				{"Name",DataValueType.Text},
				{"Email", DataValueType.Text},
				{"Phone", DataValueType.Text},
				{"SomeGuid", DataValueType.Guid},
				{"SomeFloat", DataValueType.Float8},
				{"SomeDateTime", DataValueType.DateTime},
				{"SomeDate", DataValueType.Date},
				{"SomeTime", DataValueType.Time},
				
			};
			var lookups = new Dictionary<string,string>{
				{"ContactType","ContactType"}
			};
			
			MockEntitySchemaWithColumns2("Contact", columns, lookups);
		}
	}
	

}