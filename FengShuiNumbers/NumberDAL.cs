using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FengShuiNumbers
{
	/// <summary>
	/// Number Data Access Layer,
	/// simplified access and retrieve data from database,
	/// send data to Controller for more complex process
	/// </summary>
	internal class NumberDAL
	{
		private static string _connectionString;
		private static SqlCommand cmd;
		private static SqlDataReader rdr;
		public static string[] TabooString { get; private set; }
		public static string[] SumProportion { get; private set; }
		public static string[] NicePair { get; private set; }

		/// <summary>
		/// Initialize connectionString and FengShuiConfiguration from appsettings.json
		/// </summary>
		public NumberDAL()
		{
			var builder = new ConfigurationBuilder()
								 .SetBasePath(Directory.GetCurrentDirectory())
								 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			_connectionString = builder.Build().GetConnectionString("DefaultConnection");
			TabooString = builder.Build().GetSection("FengShuiConfiguration").GetSection("Taboo").Value.Split(',').ToArray();
			SumProportion = builder.Build().GetSection("FengShuiConfiguration").GetSection("SumProportion").Value.Split(',').ToArray();
			NicePair = builder.Build().GetSection("FengShuiConfiguration").GetSection("LastNicePair").Value.Split(',').ToArray();
		}

		/// <summary>
		/// Access the Database and retrieve choosen list
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public List<NumberModel> GetList(string? provider)
		{
			var numberList = new List<NumberModel>();
			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					con.Open();
					if (provider == null)
					{
						cmd = new SqlCommand("Select * from NumberList ORDER BY provider, number", con);
					}
					else
					{
						cmd = new SqlCommand($"Select * from NumberList where provider='{provider}' ORDER BY number", con);
					}
					rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{
						numberList.Add(new NumberModel
						{
							Id = Convert.ToInt32(rdr[0]),
							PhoneNumber = Convert.ToInt32(rdr[1]),
							NetworkProvider = Convert.ToString(rdr[2])
						});
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return numberList;
		}

		/// <summary>
		/// Add the given number to database(has been checked existence)
		/// </summary>
		/// <param name="number"></param>
		public void AddNumber(NumberModel number)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					con.Open();
					cmd = new SqlCommand($"INSERT INTO NumberList VALUES((SELECT max(Id)+1 FROM NumberList),{number.PhoneNumber},'{number.NetworkProvider}')", con);
					cmd.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Check if the input number is existence in the database
		/// </summary>
		/// <param name="number"></param>
		public NumberModel CheckDuplicate(NumberModel number)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					con.Open();
					cmd = new SqlCommand($"Select * from NumberList where number='{number.PhoneNumber}'", con);
					cmd.ExecuteNonQuery();
					rdr = cmd.ExecuteReader();
					//find the phonenumber, if exist
					while (rdr.Read())
					{
						NumberModel foundnumber = new NumberModel
						{
							Id = Convert.ToInt32(rdr[0]),
							PhoneNumber = Convert.ToInt32(rdr[1]),
							NetworkProvider = Convert.ToString(rdr[2])
						};
						return foundnumber;
					}
					return null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}