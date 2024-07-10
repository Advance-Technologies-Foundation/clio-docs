using System;
using System.Globalization;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Core.Factories;

namespace AtfRecordBoost.EntityEventListener
{
	/// <summary>
	///  Handles event layer for "ContactFile" entity
	/// </summary>
	/// <remarks>
	///  Creatio triggers the mechanism of the Entity event layer after executing the object event subprocesses
	///  See
	///  <a href="https://academy.creatio.com/docs/developer/back_end_development/objects_business_logic/overview"> academy</a>
	///  documentation for details.
	/// </remarks>
	[EntityEventListener(SchemaName = "ContactFile")]
	internal class ContactFileEventListener : BaseEntityEventListener
	{

		#region Methods: Public

		/// <summary>
		///  Occurs on AFTER a record is saved.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnSaved(object sender, EntityAfterEventArgs e){
			base.OnSaved(sender, e);
			Entity entity = (Entity)sender;
			Guid contactFileId = entity.GetTypedColumnValue<Guid>("Id");
			Guid contactId = entity.GetTypedColumnValue<Guid>("ContactId");
			string fileName = entity.GetTypedColumnValue<string>("Name");
			Guid typeId = entity.GetTypedColumnValue<Guid>("TypeId");
			if (fileName.StartsWith("RecordBoost", true, CultureInfo.InvariantCulture) && typeId == FileTypeConsts.File) {
				ClassFactory.Get<IEntityBooster>().Boost(contactId, contactFileId);
			}
		}

		#endregion

	}
}