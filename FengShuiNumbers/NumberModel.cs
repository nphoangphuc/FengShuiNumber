using System;
using System.Collections.Generic;
using System.Text;

namespace FengShuiNumbers
{
	public class NumberModel
	{
		private int _id;
		private int _phoneNumber;
		private string _networkProvider;

		public int Id { get => _id; set => _id = value; }
		public int PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
		public string NetworkProvider { get => _networkProvider; set => _networkProvider = value; }
	}
}
