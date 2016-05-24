using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Enterprise.Models;
using Microsoft.AspNet.Identity.EntityFramework;

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

		public static List<IdentityRole<string, IdentityUserRole>> GetAvailableRoles()
		{
			var result = new List<IdentityRole<string, IdentityUserRole>>();

			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					cmd.CommandText = @"select Id, Name from AspNetRoles";
					cmd.Connection = connection;
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(new IdentityRole()
							{
								Id = reader.GetFieldValue<string>(0),
								Name = reader.GetFieldValue<string>(1)
							});
						}
					}
				}
				connection.Close();
			}
			return result;
		}

		public static List<Item> GetItems(Item instance)
		{
			var result = new List<Item>();
			var isMachine = instance is Machine;

			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					cmd.CommandText = string.Format(
						@"select p.Id, p.Name, p.Description, p.C_Date, p.C_User, u.UserName{0} from {1}s p
							left join AspNetUsers u on p.C_User = u.Id{2}",
						isMachine ? ", d.Id, d.Name" : string.Empty,
						instance.InheritorName,
						isMachine ? " left join Departments d on d.Id = p.DepartmentId" : string.Empty
						);

					cmd.Connection = connection;
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var item = instance.Create(
								reader.GetFieldValue<int>(0),
								reader.GetFieldValue<string>(1),
								reader.GetFieldValue<string>(2),
								reader.IsDBNull(3) ? DateTime.MinValue : reader.GetFieldValue<DateTime>(3),
								reader.GetFieldValue<string>(4),
								reader.GetFieldValue<string>(5)
								);
							if (isMachine)
							{
								var machine = (Machine)item;
								machine.DepartmentId = reader.IsDBNull(6) ? 0 : reader.GetFieldValue<int>(6);
								machine.DepartmentName = reader.GetFieldValue<string>(7);
							}
							result.Add(item);
						}
					}
				}
				connection.Close();
			}
			return result;
		}

		public static bool DeleteItem(int id, string inheritorName)
		{
			var result = false;
			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					cmd.CommandText = string.Format(@"delete {0}s where Id = @idParam", inheritorName);
					cmd.Connection = connection;
					cmd.Parameters.AddWithValue("idParam", id);
					if (cmd.ExecuteNonQuery() > 0)
					{
						result = true;
					}
				}
				connection.Close();
			}
			return result;
		}

		public static void UpdateItems(IEnumerable<Item> products)
		{
			var productsArray = (products as List<Item>) ?? products.ToList();
			if (productsArray.Count == 0)
			{
				return;
			}

			var instance = productsArray[0];
			var isMachine = instance is Machine;
			var stringBuilder = new StringBuilder();

			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					stringBuilder.AppendLine("BEGIN TRANSACTION UpdateItems");
					var i = 0;
					foreach (var p in productsArray)
					{
						++i;
						var nameParam = string.Format("nameParam{0}", i);
						var descriptionParam = string.Format("descriptionParam{0}", i);
						var idParam = string.Format("idParam{0}", i);
						var departmentParam = string.Empty;
						if (isMachine)
						{
							departmentParam = string.Format("departmentParam{0}", i);
							cmd.Parameters.AddWithValue(departmentParam, ((Machine)p).DepartmentId);
						}

						stringBuilder.AppendLine(string.Format("update {0}s set Name = @{1}, Description = @{2}{3} where Id = @{4}", instance.InheritorName,
							nameParam, descriptionParam, isMachine ? string.Format(", DepartmentId = @{0}", departmentParam) : string.Empty, idParam));
						cmd.Parameters.AddRange(new[] {
							new SqlParameter(nameParam, p.Name ?? string.Empty),
							new SqlParameter(descriptionParam, p.Description ?? string.Empty),
							new SqlParameter(idParam, p.Id)
						});
					}
					stringBuilder.AppendLine("COMMIT TRANSACTION UpdateItems");
					cmd.CommandText = stringBuilder.ToString();
					cmd.Connection = connection;
					cmd.ExecuteNonQuery();
				}
				connection.Close();
			}
		}
	}
}