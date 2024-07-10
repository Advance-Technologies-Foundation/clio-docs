using System;
using System.Collections.Generic;
using AtfRecordBoost.EntityEventListener;
using NSubstitute;
using NUnit.Framework;
using Terrasoft.Configuration.Tests;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;

namespace AtfRecordBoost.Tests.EntityEventListener
{
	[MockSettings(RequireMock.All)]
	[TestFixture(Category = "UnitTests")]
	public class ContactFileEventListenerTests : BaseMarketplaceTestFixture
	{

		#region Constants: Private

		private const string EntitySchemaName = "ContactFile";

		#endregion

		#region Fields: Private

		private ContactFileEventListener _contactFileEventListener;
		private IEntityBooster _entityBooster;
		private Entity _contactFileEntity;

		#endregion

		#region Methods: Private

		private void CreateContactFileEntity(Guid contactId, Guid contactFileId, Guid typeId, string fileName){
			MockEmptyEntitySchema("Contact");
			MockEmptyEntitySchema("FileType");
			Dictionary<string, string> lookupColumns = new Dictionary<string, string> {
				{"Contact", "Contact"},
				{"Type", "FileType"}
			};
			Dictionary<string, DataValueType> columns = new Dictionary<string, DataValueType> {
				{"Name", DataValueType.Text}
			};

			MockEntitySchemaWithColumns(EntitySchemaName, columns, lookupColumns);
			SetUpTestData(EntitySchemaName, new Dictionary<string, object> {
				{"Id", contactFileId},
				{"ContactId", contactId},
				{"TypeId", typeId},
				{"Name", fileName}
			});
			_contactFileEntity = SubstituteEmptyEntity(EntitySchemaName);
			_contactFileEntity.FetchFromDB(contactFileId);
		}

		#endregion

		#region Methods: Protected

		protected override void SetUp(){
			base.SetUp();
			_contactFileEventListener = new ContactFileEventListener();
			ClassFactory.RebindWithFactoryMethod(() => (UserConnection)UserConnection);

			_entityBooster = Substitute.For<IEntityBooster>();
			ClassFactory.RebindWithFactoryMethod(() => _entityBooster);
		}

		#endregion

		#region Methods: Public

		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8", "RecordBoost.yaml")]
		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8", "recordboost.yaml")]
		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8",
			"recordboost-junk.yaml")]
		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8", "rEcOrDbOOst.yaml")]
		public void OnSaved_CallBoost_When_FileType_IsFile_AndNameMatches(string contact, string contactFile, string fileName){
			//Arrange
			Guid contactId = Guid.Parse(contact);
			Guid contactFileId = Guid.Parse(contactFile);
			EntityAfterEventArgs entityAfterEventArgs = new EntityAfterEventArgs();
			CreateContactFileEntity(contactId, contactFileId, FileTypeConsts.File, fileName);

			//Act
			_contactFileEventListener.OnSaved(_contactFileEntity, entityAfterEventArgs);

			//Assert
			_entityBooster.Received(1).Boost(contactId, contactFileId);
		}

		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8", "SomeFile.yaml")]
		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8", "OtherFile.yaml")]
		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8",
			"Not-recordboost.yaml")]
		public void OnSaved_ShouldNot_CallBoost_When_NameDoesNotMatch(string contact, string contactFile, string fileName){
			//Arrange
			Guid contactId = Guid.Parse(contact);
			Guid contactFileId = Guid.Parse(contactFile);
			EntityAfterEventArgs entityAfterEventArgs = new EntityAfterEventArgs();
			CreateContactFileEntity(contactId, contactFileId, FileTypeConsts.File, fileName);

			//Act
			_contactFileEventListener.OnSaved(_contactFileEntity, entityAfterEventArgs);

			//Assert
			_entityBooster.Received(0).Boost(contactId, contactFileId);
		}
		
		
		[TestCase("2DE15640-461B-48F2-B8EC-23C86CA260EF", "17535A5C-25CF-4B9A-99D3-8F6853118AE8",
			"recordboost.yaml")]
		public void OnSaved_ShouldNot_CallBoost_When_FileTypeDoesNotMatch(string contact, string contactFile, string fileName){
			//Arrange
			Guid contactId = Guid.Parse(contact);
			Guid contactFileId = Guid.Parse(contactFile);
			EntityAfterEventArgs entityAfterEventArgs = new EntityAfterEventArgs();
			CreateContactFileEntity(contactId, contactFileId, FileTypeConsts.Link, fileName);

			//Act
			_contactFileEventListener.OnSaved(_contactFileEntity, entityAfterEventArgs);

			//Assert
			_entityBooster.Received(0).Boost(contactId, contactFileId);
		}

		#endregion

	}
}