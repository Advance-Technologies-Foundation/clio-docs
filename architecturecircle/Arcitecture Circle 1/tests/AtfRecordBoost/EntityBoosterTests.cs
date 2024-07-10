using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.File;
using Terrasoft.File.Abstractions;
using Terrasoft.TestFramework;

namespace AtfRecordBoost.Tests
{
	[MockSettings(RequireMock.All)]
	[TestFixture(Category = "UnitTests")]
	public class EntityBoosterTests : BaseMarketplaceTestFixture
	{
		private IEntityBooster _entityBooster;
		protected override void SetUp(){
			base.SetUp();
			UserConnection.SetupFileFactory();
			ClassFactory.RebindWithFactoryMethod(()=>(UserConnection)UserConnection);
			_entityBooster = new EntityBooster();
		}
		
		[TestCase("RecordBoost.yaml")]
		public void GetStringContent_Returns_Correct_StringValue(string fileName){

			using (FileStream fs = new FileStream($"SampleFiles/{fileName}", FileMode.Open)) {
				//Arrange
				Guid contactFileId = Guid.NewGuid();
				IFile fileMock = Substitute.For<IFile>();
				fileMock.Name.Returns(fileName);
				fileMock.Length.Returns(fs.Length);
				fileMock.Exists.Returns(true);
				fs.Seek(0, SeekOrigin.Begin);
				fileMock.ReadAsync().Returns(Task.FromResult((Stream)fs));
				
				UserConnection.GetFileFactory().Get(Arg.Is<EntityFileLocator>(l=> 
						l.RecordId == contactFileId && l.EntitySchemaName == "ContactFile"))
					.Returns(fileMock);
				
				//Act
				string fileContent = _entityBooster.GetStringContent(contactFileId);

				//Assert
				fileContent.Should().Be("FullName: Kirill Krylov");
			}

		}
		
		[TestCase("RecordBoost.txt")]
		[TestCase("RecordBoost.ini")]
		[TestCase("RecordBoost")]
		public void GetStringContent_Returns_Null(string fileName){
			using (FileStream fs = new FileStream($"SampleFiles/RecordBoost.yaml", FileMode.Open)) {
				//Arrange
				Guid contactFileId = Guid.NewGuid();
				IFile fileMock = Substitute.For<IFile>();
				fileMock.Name.Returns(fileName);
				fileMock.Length.Returns(fs.Length);
				fileMock.Exists.Returns(true);
				fs.Seek(0, SeekOrigin.Begin);
				fileMock.ReadAsync().Returns(Task.FromResult((Stream)fs));
				
				UserConnection.GetFileFactory().Get(Arg.Is<EntityFileLocator>(l=> 
						l.RecordId == contactFileId && l.EntitySchemaName == "ContactFile"))
					.Returns(fileMock);
				
				//Act
				string fileContent = _entityBooster.GetStringContent(contactFileId);

				//Assert
				fileContent.Should().BeNull();
			}

		}

	}

}