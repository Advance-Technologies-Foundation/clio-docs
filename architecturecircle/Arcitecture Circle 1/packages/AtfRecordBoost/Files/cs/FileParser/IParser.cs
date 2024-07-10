using System.Collections.Generic;

namespace AtfRecordBoost.FileParser
{
	public interface IParser
	{

		Dictionary<string, object> Parse(string content);

	}
}