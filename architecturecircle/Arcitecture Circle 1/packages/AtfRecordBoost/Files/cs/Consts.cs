using System;
using System.Diagnostics.CodeAnalysis;

namespace AtfRecordBoost
{
	
	[ExcludeFromCodeCoverage]
	public static class FileTypeConsts
	{
		public static Guid File => Guid.Parse("529bc2f8-0ee0-df11-971b-001d60e938c6");
		public static Guid Link => Guid.Parse("539bc2f8-0ee0-df11-971b-001d60e938c6");
		public static Guid LinkToObject => Guid.Parse("549bc2f8-0ee0-df11-971b-001d60e938c6");

	}
}