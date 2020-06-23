using System;
using System.Collections.Generic;
using System.Linq;

namespace FengShuiNumbers
{
	/// <summary>
	/// Number Controller
	/// contain complex method to handle data and return to View to print out,
	/// access database through NumberDAL
	/// </summary>
	public class NumberController
	{
		
		/// <summary>
		/// Get the GetFengShuiConfiguration from NumberDAL
		/// Put the parameters in class because many methods will use these list
		/// </summary>	
		private static int[] tabooList = Array.ConvertAll(NumberDAL.TabooString, int.Parse);
		private static int[] nicePairList = Array.ConvertAll(NumberDAL.NicePair, int.Parse);
		private static string[] sumProportion = NumberDAL.SumProportion;
		//keep the sumProportion as string to prevent mislead proportion 12/14 and 24/28

		/// <summary>
		/// Get the specific list from database, based on user's choice
		/// </summary>
		/// <param name="provider"></param>
		public static void GetList(string? provider)
		{
			//GetAppSettingsFile();
			var numberDal = new NumberDAL();
			List<NumberModel> numbersList;
			if (provider == null)
			{
				numbersList = numberDal.GetList(null);
			}
			else
			{
				numbersList = numberDal.GetList(provider);
			}
			CheckFSN(numbersList);
		}

		/// <summary>
		/// Find good FengShuiNumber in the given numbersList from GetList method and return a new List to print out
		/// </summary>
		/// <param name="numbersList"></param>
		private static void CheckFSN(List<NumberModel> numbersList)
		{
			List<NumberModel> result = new List<NumberModel>();
			foreach (var number in numbersList)
			{
				int firstSum = 0;
				int secondSum = 0;
				int phone = number.PhoneNumber;
				int last2digits = phone % 100;
				if (tabooList.Contains(last2digits)) continue;
				else
				{
					for (int i = 0; i < 10; i++)
					{
						if (i < 5)
						{
							secondSum += phone % 10;
							phone = phone / 10;
						}
						else
						{
							firstSum += phone % 10;
							phone = phone / 10;
						}
					}
					Convert.ToString(firstSum); //Convert sum to String to get the right proportion for 24/28 (no mislead to 12/14)
					Convert.ToString(secondSum);
					if (sumProportion.Contains($"{firstSum}/{secondSum}") && nicePairList.Contains(number.PhoneNumber % 100)) result.Add(number);
				}
			}
			PrintNumber(result);
		}

		/// <summary>
		/// Print out the given List
		/// </summary>
		/// <param name="numbersList"></param>
		private static void PrintNumber(List<NumberModel> numbersList)
		{
			numbersList.ForEach(item =>
			{
				Console.WriteLine($"\t|0{item.PhoneNumber}\t|\t{item.NetworkProvider}\t\t|");
			});
			Console.WriteLine("\t|---------------------------------------|");
			Console.WriteLine("\tPress Any key to continue");
			Console.ReadKey();
		}

		/// <summary>
		/// Check if the input number has existed in database,
		/// if existed return the number,
		/// if not return null
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static NumberModel CheckDuplicate(NumberModel number)
		{
			//GetAppSettingsFile();
			var numberDal = new NumberDAL();
			if (numberDal.CheckDuplicate(number) == null)
			{
				return null;
			}
			return number;
		}

		/// <summary>
		/// Check Feng Shui of single input number
		/// </summary>
		/// <param name="number"></param>
		public static bool CheckSingleNumber(NumberModel number)
		{
			int firstSum = 0;
			int secondSum = 0;
			int phone = number.PhoneNumber; //create a temp phonenumber variable for calculation
			if (tabooList.Contains(phone % 100)) return false;//if last 2 digit in tabooList, not good FengShui
			else
			{
				for (int i = 0; i < 10; i++)
				{
					if (i < 5)//the last 5 digit sum will be 2nd-sum
					{
						secondSum += phone % 10;
						phone = phone / 10;
					}
					else//the first 4 digit(the first digit is 0) will be 1st-sum
					{
						firstSum += phone % 10;
						phone = phone / 10;
					}
				}
				Convert.ToString(firstSum); //Convert sum to String to get the right proportion for 24/28 (no mislead to 12/14)
				Convert.ToString(secondSum);
				if (sumProportion.Contains($"{firstSum}/{secondSum}") && nicePairList.Contains(number.PhoneNumber % 100)) return true;
			}
			return false;
		}

		/// <summary>
		/// Add the input number to database if not existed
		/// </summary>
		/// <param name="numberModel"></param>
		public static void AddNumberToDb(NumberModel number)
		{
			var numberDal = new NumberDAL();
			numberDal.AddNumber(number);
		}
	}
}