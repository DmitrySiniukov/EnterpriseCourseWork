using System.Configuration;

namespace Enterprise.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public class EnterpriseDB
	{
		private static string _connectionString;

		public static string DefaultConnection
		{
			get
			{
				if (string.IsNullOrEmpty(_connectionString))
				{
					_connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
				}
				return _connectionString;
			}
		}
	}
}