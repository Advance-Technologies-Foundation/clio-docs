using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using AtfRecordBoost.FileParser;
using Common.Logging;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;
using Terrasoft.File;
using Terrasoft.File.Abstractions;

namespace AtfRecordBoost
{
	public interface IEntityBooster
	{

		#region Methods: Public

		void Boost(Guid contactId, Guid contactFileId);

		/// <summary>
		///  Gets string content from file
		/// </summary>
		/// <param name="contactFileId">ContactFileId</param>
		/// <returns>string of value of a file</returns>
		string GetStringContent(Guid contactFileId);

		#endregion

	}

	[DefaultBinding(typeof(IEntityBooster))]
	public class EntityBooster : IEntityBooster
	{

		#region Constants: Private

		private const string EntitySchemaName = "ContactFile";
		private const int MaxFileSizeInMb = 10;
		private const int MaxSize = MaxFileSizeInMb * 1024 * 1024;

		#endregion

		#region Methods: Private

		private string GetStringContentFromFile(IFile file){
			using (Stream stream = file.ReadAsync().ConfigureAwait(false).GetAwaiter().GetResult()) {
				using (MemoryStream ms = new MemoryStream()) {
					try {
						byte[] buffer = new byte[81920];
						int dataCount = 0;
						int bytesRead;
						while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
							if ((dataCount += bytesRead) > MaxSize) {
								throw new Exception($"File size more than {MaxFileSizeInMb} MB");
							}
							ms.Write(buffer, 0, bytesRead);
						}

						byte[] bytes = ms.ToArray();
						string textContent = Encoding.UTF8.GetString(bytes);
						return textContent;
					} catch (Exception ex) {
						LogManager.GetLogger("RecordBooster")
							.ErrorFormat("Could not get bytes from file {0} \n exception:{1}", file.Name, ex.Message);
						throw;
					}
				}
			}
		}

		#endregion

		#region Methods: Public

		[ExcludeFromCodeCoverage]
		public void Boost(Guid contactId, Guid contactFileId){
			var yamlContent = GetStringContent(contactFileId);
			var yamlParser = ClassFactory.Get<IParser>("Yaml");
			var userConnection = ClassFactory.Get<UserConnection>();
			Entity contact = userConnection.EntitySchemaManager
				.GetInstanceByName("Contact").CreateEntity(userConnection);
			contact.FetchFromDB(contactId);
			var parsedContent = yamlParser.Parse(yamlContent);
			
			parsedContent.ToList().ForEach(c=> {
				contact.SetColumnValue(c.Key, c.Value);
			});
			contact.Save();
		}

		public string GetStringContent(Guid contactFileId){
			EntityFileLocator locator = new EntityFileLocator(EntitySchemaName, contactFileId);
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			IFile file = userConnection.GetFile(locator);

			string[] supportedFileExtensions = {"yaml", "json", "xml"};
			bool isSupportedFileExtensions = supportedFileExtensions
				.Any(extension => file.Name.EndsWith(extension));

			return isSupportedFileExtensions
				? GetStringContentFromFile(file)
				: null;
		}

		#endregion

	}
}