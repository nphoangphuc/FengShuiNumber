using System;

namespace FengShuiNumbers
{
	/// <summary>
	/// The program view contains view, menus,output and validates input
	/// </summary>
	internal class Program
	{
		private static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine();
				Menu();
				Console.Write("\t\tPlease choose: ");
				int.TryParse(Console.ReadLine(), out int choice);
				Console.WriteLine("");
				Choose(choice);
			}
		}

		/// <summary>
		/// The main menu for user to input choice
		/// </summary
		public static void Menu()
		{
			Console.WriteLine("\n\t||=============FengShuiNumber==================\t||");
			Console.WriteLine("\t||1.Get All Good Feng Shui Phone Number\t\t||");
			Console.WriteLine("\t||2.Get Viettel Good Feng Shui Phone Number\t||");
			Console.WriteLine("\t||3.Get Mobi Good Feng Shui Phone Number\t||");
			Console.WriteLine("\t||4.Get Vina Good Feng Shui Phone Number\t||");
			Console.WriteLine("\t||5.Add Phone Number and check Feng Shui\t||");
			Console.WriteLine("\t||6.Explain Good Feng Shui Number\t\t||");
			Console.WriteLine("\t||=============================================\t||");
		}

		/// <summary>
		/// Respond based on the user's choice
		/// </summary>
		/// <param name="choice"></param>
		public static void Choose(int choice)
		{
			switch (choice)
			{
				case 1:
					Console.WriteLine("\n\t|---------------------------------------|");
					Console.WriteLine("\t|All the known good Feng Shui numbers\t|");
					Console.WriteLine("\t|Phone Number \t|\tProvider\t|");
					NumberController.GetList(null);
					break;

				case 2:
					Console.WriteLine("\n\t|---------------------------------------|");
					Console.WriteLine("\t|The good Feng Shui Viettel numbers\t|");
					Console.WriteLine("\t|Phone Number \t|\tProvider\t|");
					NumberController.GetList("Viettel");
					break;

				case 3:
					Console.WriteLine("\n\t|---------------------------------------|");
					Console.WriteLine("\t|The known good Feng Shui Mobi numbers\t|");
					Console.WriteLine("\t|Phone Number \t|\tProvider\t|");
					NumberController.GetList("Mobi");
					break;

				case 4:
					Console.WriteLine("\n\t|---------------------------------------|");
					Console.WriteLine("\t|The known good Feng Shui Vina numbers\t|");
					Console.WriteLine("\t|Phone Number \t|\tProvider\t|");
					NumberController.GetList("Vina");
					break;

				case 5:
					NumberModel number = AddPhone(); //create a NumberModel object to contain data
					if (number.PhoneNumber == 999) break; //Check again to make sure when the number is 999 will break and back to main menu 
					NumberModel checkDuplicate = NumberController.CheckDuplicate(number); //check if the number has existed in database
					if (checkDuplicate == null)//if not existed, add to db and check FengShui
					{
						NumberController.AddNumberToDb(number);
						Console.WriteLine($"0{number.PhoneNumber} has been added to the database");
						bool check = NumberController.CheckSingleNumber(number);
						if (check)
						{
							Console.WriteLine($"0{number.PhoneNumber} is a Good Feng Shui Number");
						}
						else
						{
							Console.WriteLine($"0{number.PhoneNumber} is a Not Good Feng Shui Number");
						}
						break;
					}
					else //if existed, print the alert and FengShui status
					{
						bool check = NumberController.CheckSingleNumber(number);
						Console.WriteLine($"0{number.PhoneNumber} has been existed in the database");
						Console.Write($"and ");
						if (check)
						{
							Console.WriteLine($"0{number.PhoneNumber} is a Good Feng Shui Number");
						}
						else
						{
							Console.WriteLine($"0{number.PhoneNumber} is a Not Good Feng Shui Number");
						}
						break;
					}
				case 6:
					Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
					Console.WriteLine("(1). The last 2 num chars is taboo: ");
					Console.WriteLine("If this rule is violated, other criterias at (2) are not checked in any case.");
					Console.WriteLine("00, 66, 04, 45, 85, 27, 67, ");
					Console.WriteLine("17, 57, 97, 98, 58, 42, 82 69");
					Console.WriteLine("(2) Good feng shui numbers: match all criteria below:");
					Console.WriteLine("Total first 5 nums / Total last 5 nums: matches 1 in 2 conditions: 24 / 29 or 24 / 28");
					Console.WriteLine("Last nice pair of numbers: 19, 24, 26, 37, 34 ");
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Menu to add a new phonenumber and check Validation, if valid return the phonenumber to continue check FengShui
		/// </summary>
		/// <returns></returns>
		public static NumberModel AddPhone()
		{
			int phoneNumber = 0;
			int choiceProvider;
			string networkProvider = "";
			do
			{
				Console.WriteLine("\n\t|---------------------------------------------\t|");
				Console.WriteLine("\t|Please choose the providers (Enter number):\t|");
				Console.WriteLine("\t|1/Viettel: 086xxx, 096xxx, 097xxx\t\t|");
				Console.WriteLine("\t|2/Mobi: 089xxx, 090xxx, 093xxx\t\t\t|");
				Console.WriteLine("\t|3/Vina: 088xxx, 091xxx, 094xxx\t\t\t|");
				Console.WriteLine("\t|----------------------------------------------\t|");
				Console.Write("\t\tPlease choose: ");
				int.TryParse(Console.ReadLine(), out choiceProvider);
			}
			while (choiceProvider != 1 && choiceProvider != 2 && choiceProvider != 3);
			switch (choiceProvider)
			{
				case 1:
					networkProvider = "Viettel";
					do
					{
						Console.WriteLine("\nPlease input the phone number (Enter 999 to comeback)");
						Console.WriteLine("Viettel phone number has to start with 086,096,097 and has 10 digit");
						Console.Write("Input number: ");
						int.TryParse(Console.ReadLine(), out phoneNumber);
						if (phoneNumber == 999) break;
					} while (phoneNumber / 10000000 != 86 && phoneNumber / 10000000 != 96 && phoneNumber / 10000000 != 97 && phoneNumber.ToString().Length != 9);

					break;

				case 2:
					networkProvider = "Mobi";
					do
					{
						Console.WriteLine("\nPlease input the phone number (Enter 999 to comeback)");
						Console.WriteLine("Mobi phone number has to start with 089, 090, 093 and has 10 digit");
						Console.Write("Input number: ");
						int.TryParse(Console.ReadLine(), out phoneNumber);
						if (phoneNumber == 999) break;
					} while (phoneNumber / 10000000 != 89 && phoneNumber / 10000000 != 90 && phoneNumber / 10000000 != 93 && phoneNumber.ToString().Length != 9);

					break;

				case 3:
					networkProvider = "Vina";
					do
					{
						Console.WriteLine("\nPlease input the phone number (Enter 999 to comeback)");
						Console.WriteLine("Vina phone number has to start with 088, 091, 094 and has 10 digit");
						Console.Write("Input number: ");
						int.TryParse(Console.ReadLine(), out phoneNumber);
						if (phoneNumber == 999) break;
					} while (phoneNumber / 10000000 != 88 && phoneNumber / 10000000 != 91 && phoneNumber / 10000000 != 94 && phoneNumber.ToString().Length != 9);
					break;

				default:
					break;
			}
			//Create NumberModel object and continue to check FengShui
			NumberModel number = new NumberModel
			{
				PhoneNumber = phoneNumber,
				NetworkProvider = networkProvider
			};
			return number;
		}
	}
}