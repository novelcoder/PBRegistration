﻿using System;
using System.Text;

namespace RegistrationTables
{
	public class Registration
	{
		public string Timestamp = string.Empty;
		public string Email = string.Empty;
		public string Name = string.Empty;
		public string PhoneNumber = string.Empty;
        public string ShirtSize = string.Empty;
		public string RNR_MixedSkillLevel = string.Empty;
		public string RNR_MixedPartnerName = string.Empty;
		public string RNR_MixedPartnerPhoneNumber = string.Empty;
		public string RNR_MensWomensSkillLevel = string.Empty;
		public string RNR_MensWomensPartnerName = string.Empty;
		public string RNR_MensWomensPhoneNumber = string.Empty;
        public string PINKED_MixedSkillLevel = string.Empty;
        public string PINKED_MixedPartnerName = string.Empty;
        public string PINKED_MixedPartnerPhoneNumber = string.Empty;
        public string PINKED_MensWomensSkillLevel = string.Empty;
        public string PINKED_MensWomensPartnerName = string.Empty;
        public string PINKED_MensWomensPhoneNumber = string.Empty;
		public string NumberOfEvents = string.Empty;
		public string Remove = string.Empty;

		// keep track of phone numbers handed out
		private static Dictionary<string, string> FalsePhone = new Dictionary<string, string>();
		private static int FalsePhoneSeed = 1000000001;

		private static List<string> ignoreNames = new List<string>
			{	"None", "none", "xxx",
				"Not entered in mixed"};
		// make sure key is lower case
		private static Dictionary<string, string> translateName
			= new Dictionary<string, string> {
				{ "jose casarin", "José Casarin" },
                { "matthew barlow", "Matt Barlow" },
                { "m", string.Empty } };
		private static Dictionary<string, string> translateEmail = new Dictionary<string, string> {
				{ "acker.matthee3@gmail.com", "acker.matthew3@gmail.com" } };
		private static List<string> ignorePartnerNames = new List<string>
				{ "Not entered in mixed", "None" };

        internal static List<Person> Parse(IEnumerable<Registration> records)
        {
			var persons = new List<Person>();
			var recList = new List<Registration>();

            // first,clean bad data 
            foreach (var record in records)
            {
                record.Email = TranslateEmail(record.Email);
                record.Name = TranslateName(record.Name);
				record.PhoneNumber = TranslatePhoneNumber(record.PhoneNumber, record.Name);

                record.RNR_MensWomensPartnerName = TranslateName(record.RNR_MensWomensPartnerName);
                record.RNR_MensWomensPhoneNumber = TranslatePhoneNumber(record.RNR_MensWomensPhoneNumber, record.RNR_MensWomensPartnerName);
                record.RNR_MixedPartnerName = TranslateName(record.RNR_MixedPartnerName);
                record.RNR_MixedPartnerPhoneNumber = TranslatePhoneNumber(record.RNR_MixedPartnerPhoneNumber, record.RNR_MixedPartnerName);

                record.PINKED_MensWomensPartnerName = TranslateName(record.PINKED_MensWomensPartnerName);
				record.PINKED_MensWomensPhoneNumber = TranslatePhoneNumber(record.PINKED_MensWomensPhoneNumber, record.PINKED_MensWomensPartnerName);
				record.PINKED_MixedPartnerName = TranslateName(record.PINKED_MixedPartnerName);
                record.PINKED_MixedPartnerPhoneNumber = TranslatePhoneNumber(record.PINKED_MixedPartnerPhoneNumber, record.PINKED_MixedPartnerName);
                recList.Add(record);
            }

            //flesh out the list of people
            foreach ( var record in recList)
			{
				if (record.Remove.ToLower().Trim() != "ignore" &&
					record.Remove.ToLower().Trim() != "withdraw")
				{
					AddPerson(persons, record.Name, record.Email, record.PhoneNumber, record.ShirtSize, record.NumberOfEvents, true);

					AddPerson(persons, record.RNR_MensWomensPartnerName, record.RNR_MensWomensPhoneNumber);
					AddPerson(persons, record.RNR_MixedPartnerName, record.RNR_MixedPartnerPhoneNumber);
					AddPerson(persons, record.PINKED_MensWomensPartnerName, record.PINKED_MensWomensPhoneNumber);
					AddPerson(persons, record.PINKED_MixedPartnerName, record.PINKED_MixedPartnerPhoneNumber);
				}
            }

			//second, add events
			foreach (var record in recList)
			{
				var person = Person.FindPerson(persons, record.Name, record.PhoneNumber);
				if (person != null)
				{
					if (!string.IsNullOrWhiteSpace(record.RNR_MixedSkillLevel))
						Event.AddEvents(persons, person, record.RNR_MixedSkillLevel, record.RNR_MixedPartnerName, record.RNR_MixedPartnerPhoneNumber, Tournaments.rockNRoll);
                      
					if (!string.IsNullOrWhiteSpace(record.RNR_MensWomensSkillLevel))
						Event.AddEvents(persons, person, record.RNR_MensWomensSkillLevel, record.RNR_MensWomensPartnerName, record.RNR_MensWomensPhoneNumber, Tournaments.rockNRoll);

					if (!string.IsNullOrWhiteSpace(record.PINKED_MixedSkillLevel))
						Event.AddEvents(persons, person, record.PINKED_MixedSkillLevel, record.PINKED_MixedPartnerName, record.PINKED_MixedPartnerPhoneNumber, Tournaments.pinked);

					if (!string.IsNullOrWhiteSpace(record.PINKED_MensWomensSkillLevel))
						Event.AddEvents(persons, person, record.PINKED_MensWomensSkillLevel, record.PINKED_MensWomensPartnerName, record.PINKED_MensWomensPhoneNumber, Tournaments.pinked);
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(record.Name))
						Console.WriteLine($"ERROR - {record.Name} not found.");
				}
			}

			return persons;
        }

		private static void AddPerson(List<Person> persons, string name, string phoneNumber)
		{
			AddPerson(persons, name, string.Empty, phoneNumber, string.Empty, string.Empty, false);
		}

        private static void AddPerson(List<Person> persons, string name, string email, string phoneNumber, string shirtSize, string reportedNumEvents, bool masterRegistration)
        {
			Person? person = null;

			if ( ! string.IsNullOrWhiteSpace(name)
			  && ignoreNames.FirstOrDefault(x => x == name) == null )
            {
                person = persons.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
				if (person == null)
					person = persons.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

				if (person == null)
				{
					persons.Add(new Person
					{
						Email = email,
						PhoneNumber = phoneNumber,
						Name = name,
						ShirtSize = shirtSize,
						IsRegistered = masterRegistration,
						SelfReportedNumEvents = SelfRegisteredNumEvents(reportedNumEvents)
					});
				}
				else
				{
					//if this is a masterRegistration, just update the person
					if (masterRegistration)
					{
						person.IsRegistered = masterRegistration;
						person.SelfReportedNumEvents = SelfRegisteredNumEvents(reportedNumEvents);
					}

					// if incoming name is valid
					if (!string.IsNullOrWhiteSpace(name))
					{
						// if master registration or it's empty, copy value in
						if ( masterRegistration
						  || string.IsNullOrEmpty(person.Name))
						{
							person.Name = name;
						}
					}
					// not an master registration and name is not filled it, go ahead and replace
					if ( ! masterRegistration && string.IsNullOrEmpty(person.Name))
					{
						person.Name = name;
					}
					if (! string.IsNullOrWhiteSpace(email))
					{
						person.Email = email;
					}
					if (! string.IsNullOrWhiteSpace(phoneNumber))
					{
						person.PhoneNumber = phoneNumber;
					}
					if (!string.IsNullOrWhiteSpace(shirtSize))
					{
						person.ShirtSize = shirtSize;
					}
				}
			}
        }

		private static int SelfRegisteredNumEvents(string numEvents)
		{
			int result = 0;
			string[] words = numEvents.Split();
			if (words.Length >= 1)
			{
				switch (words[0].ToLower())
				{
					case "one":
						result = 1;
						break;
					case "two":
						result = 2;
						break;
					case "three":
						result = 3;
						break;
					case "four":
						result = 4;
						break;
				}
			}

			return result;
		}

		private static string TranslatePhoneNumber(string phoneNumber, string name)
		{
			StringBuilder sb = new StringBuilder();

			bool firstDigit = true;
			for (int iii=0; iii<phoneNumber.Length; iii++)
			{
				bool keepIt = false;
				if (phoneNumber[iii] >= '0' && phoneNumber[iii] <='9')
				{
					keepIt = true;
				}
				if (firstDigit) //don't need country code
				{
					firstDigit = false;
					if (phoneNumber[iii] == '1')
						keepIt = false;
				}

				if (keepIt)
				{
					sb.Append(phoneNumber[iii]);
				}
            }

			string result = sb.ToString();
			if (result.Length != 10)
			{
				if (!FalsePhone.ContainsKey(name))
				{
					FalsePhone.Add(name, FalsePhoneSeed++.ToString());
				}

				if (FalsePhone.ContainsKey(name))
				{
					result = $"{FalsePhone[name]}";
                }
            }

			sb.Clear();
			sb.Append('(');
			sb.Append(result.Substring(0, 3));
			sb.Append(')');
			sb.Append(' ');
			sb.Append(result.Substring(3, 3));
			sb.Append('-');
			sb.Append(result.Substring(6));

			result = sb.ToString();

			return result;
		}
        private static string TranslateEmail(string email)
        {
			string? result = email;
			if (translateEmail.TryGetValue(email.Trim(), out result))
                return result;
            else
                return email.Trim();
        }

        private static string TranslateName(string name)
        {
            string? result = name;
			if (translateName.TryGetValue(name.Trim().ToLower(), out result))
				return result;
			else
				return name.Trim();
        }

		internal static string DivisionName(Event evt)
		{
			return DivisionName(evt.EventType, evt.DivisionLevel);
		}

        internal static string DivisionName(EventType evtType, DivisionLevel blvl)
        {
			var sb = new StringBuilder();

			switch (evtType)
			{
				case EventType.mens:
					sb.Append("Mens");
					break;
				case EventType.mixed:
					sb.Append("Mixed");
					break;
				case EventType.womens:
					sb.Append("Women's");
					break;
            }

			sb.Append(": ");

            switch (blvl)
            {
                case DivisionLevel.l25:
					sb.Append("2.5");
                    break;
                case DivisionLevel.l30:
					sb.Append("3.0");
                    break;
                case DivisionLevel.l35:
					sb.Append("3.5");
                    break;
                case DivisionLevel.l40:
					sb.Append("4.0");
                    break;
                case DivisionLevel.l45:
					sb.Append("4.5");
                    break;
            }

			return sb.ToString();
        }
    }
}

