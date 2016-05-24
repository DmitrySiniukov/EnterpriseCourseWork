using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

		public static List<Product> GetProducts()
		{
			var result = new List<Product>();

			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					cmd.CommandText = @"select p.Id, p.Name, p.Description, p.C_Date, p.C_User, u.UserName from Products p left join AspNetUsers u on p.C_User = u.Id";
					cmd.Connection = connection;
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(new Product
							{
								Id = reader.GetFieldValue<int>(0),
								Name = reader.GetFieldValue<string>(1),
								Description = reader.GetFieldValue<string>(2),
								CreationDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetFieldValue<DateTime>(3),
								CreationUserId = reader.GetFieldValue<string>(4),
								CreationUserLogin = reader.GetFieldValue<string>(5)
							});
						}
					}
				}
				connection.Close();
			}
			return result;
		}

		public static bool DeleteProduct(int id)
		{
			var result = false;
			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					cmd.CommandText = @"delete Products where Id = @idParam";
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

		public static void UpdateProducts(IEnumerable<Product> products)
		{
			var stringBuilder = new StringBuilder();

			using (var connection = new SqlConnection(DefaultConnection))
			{
				connection.Open();
				using (var cmd = new SqlCommand())
				{
					stringBuilder.AppendLine("BEGIN TRANSACTION UpdateProducts");
					var i = 0;
					foreach (var p in products)
					{
						++i;
						var nameParam = string.Format("nameParam{0}", i);
						var descriptionParam = string.Format("descriptionParam{0}", i);
						var idParam = string.Format("idParam{0}", i);
						stringBuilder.AppendLine(string.Format("update Products set Name = @{0}, Description = @{1} where Id = @{2}", nameParam, descriptionParam, idParam));
						cmd.Parameters.AddRange(new[] {
							new SqlParameter(nameParam, p.Name ?? string.Empty),
							new SqlParameter(descriptionParam, p.Description ?? string.Empty),
							new SqlParameter(idParam, p.Id)
						});
					}
					stringBuilder.AppendLine("COMMIT TRANSACTION UpdateProducts");
					cmd.CommandText = stringBuilder.ToString();
					cmd.Connection = connection;
					cmd.ExecuteNonQuery();
				}
				connection.Close();
			}
		}
	}
}