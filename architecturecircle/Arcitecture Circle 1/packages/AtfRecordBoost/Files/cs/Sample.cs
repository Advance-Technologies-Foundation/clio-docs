using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace AtfRecordBoost
{
	public interface ISample
	{

		#region Methods: Public

		int Add(int a, int b);

		string GetCurrentUserName();

		#endregion

	}

	[DefaultBinding(typeof(ISample))]
	public class Sample : ISample
	{

		#region Methods: Public

		public int Add(int a, int b) => a + b;

		public string GetCurrentUserName(){
			UserConnection userConnection = ClassFactory.Get<UserConnection>();
			return userConnection.CurrentUser.Name;
		}

		#endregion

	}
}