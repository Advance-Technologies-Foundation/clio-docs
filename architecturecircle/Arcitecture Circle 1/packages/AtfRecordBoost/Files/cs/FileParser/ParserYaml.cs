using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Factories;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AtfRecordBoost.FileParser
{
	[DefaultBinding(typeof(IParser), Name = "Yaml")]
	public class ParserYaml : IParser
	{
		private const string EntitySchemaName = "Contact";
		private readonly IDeserializer _deserializer = new DeserializerBuilder()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.IgnoreUnmatchedProperties()
			.Build();

		public Dictionary<string, object> Parse(string content){
			Dictionary<string, object> yaml = _deserializer.Deserialize<Dictionary<string, object>>(content);
			Entity entity = CreateEntity();
			var result = new Dictionary<string, object>(); 
			entity.Schema.Columns.ToList().ForEach(column => {
				object lookupValue = null;
				object upperValue = null;
				object lowerValue = null;
				bool isUpperValue = column.DataValueType.Name != "Lookup" && yaml.TryGetValue(column.Name, out upperValue);
				bool isLowerValue = column.DataValueType.Name != "Lookup" && yaml.TryGetValue(column.Name.ToLower(CultureInfo.InvariantCulture), out lowerValue);
				bool isLookupValue = column.DataValueType.Name == "Lookup" && yaml.TryGetValue(column.ReferenceSchema.Name, out lookupValue);
				if(!isUpperValue && !isLowerValue && !isLookupValue){
					return;
				}
				object obj = null;
				if(isLookupValue) {
					obj = CastValueToEntityType(column.DataValueType, lookupValue, column.ReferenceSchema.Name);
					if(obj!= null && (Guid)obj != Guid.Empty) {
						result[column.Name+"Id"] = obj;
						return;
					}
				}
				if(isUpperValue) {
					obj = CastValueToEntityType(column.DataValueType, upperValue, column.Name);
					if(obj != null) {
						result[column.Name] = obj;
						return;
					}
				}
				if(isLowerValue) {
					obj = CastValueToEntityType(column.DataValueType, lowerValue, column.Name);
					if(obj != null) {
						result[column.Name] = obj;
					}
				}
				
			});
			return result;
		}
		
		private Entity CreateEntity(){
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			EntitySchema entitySchema = userConnection.EntitySchemaManager
				.FindInstanceByName(EntitySchemaName);
			return entitySchema.CreateEntity(userConnection);
		}

		private object CastValueToEntityType(DataValueType type, object value, string columnName = ""){
			switch (type.Name) {
				case "ShortText":
				case "MediumText":
				case "LongText":
				case "MaxSizeText":
				case "Text":
				case "PhoneText":
				case "WebText":
				case "EmailText":
				case "RichText":
				case "SecureText":
				case "HashText":
					return value.ToString();
				case "Integer":
					return int.Parse(value.ToString());
				case "Float":
				case "Float1":
				case "Float2":
				case "Float3":
				case "Float4":
				case "Float8":
				case "Money":
				case "Currency":
					return decimal.Parse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);
				case "Guid":
					return Guid.Parse(value.ToString());
				case "DateTime":
				case "Date":
				case "Time":
					return DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
				case "Boolean":
					return bool.Parse(value.ToString());
				case "Lookup":
					return GetLookupIdByValue(value.ToString(), columnName);
				default:
					return value;
			}
		}
		private object GetLookupIdByValue(string value, string columnName){
			var userConnection = ClassFactory.Get<UserConnection>();
			
			var lookupSchema = userConnection.EntitySchemaManager
				.FindInstanceByName(columnName);
			var displayValue = lookupSchema.PrimaryDisplayColumn;
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, columnName);
			var displayValueFilter = esq.CreateFilterWithParameters(FilterComparisonType.Equal, displayValue.Name,value);
			esq.PrimaryQueryColumn.IsVisible = true;
			esq.AddColumn(displayValue.Name);
			esq.Filters.Add(displayValueFilter);
			
			var collection = esq.GetEntityCollection(userConnection);
			if(collection.Count > 0){
				var ent =collection.First();
				var idCol = ent.Schema.PrimaryColumn.Name;
				Guid id = ent.GetTypedColumnValue<Guid>(idCol);
				return id;
			}
			return Guid.Empty;
		}
	}
}