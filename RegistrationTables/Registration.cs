using System;
using System.Text;

namespace RegistrationTables
{
	public class Registration
	{
		public string Timestamp = string.Empty;
		public string Email = string.Empty;
		public string Name = string.Empty;
		public string NotEmail = string.Empty;
		public string PhoneNumber = string.Empty;
		public string MensBracket = string.Empty;
		public string MixedPartnerName = string.Empty;
		public string WomensPartnerName = string.Empty;
		public string ShirtSize = string.Empty;
		public string WomensBracket = string.Empty;
		public string MixedBracket = string.Empty;
		public string MixedPartnerShirtSize = string.Empty;
		public string GenderPartnerShirtSize = string.Empty;
		public string MensPartnerName = string.Empty;
		public string MixedPartnerPhone = string.Empty;
		public string WomensPartnerPhone = string.Empty;
		public string MensPartnerPhone = string.Empty;
		public string MixedPartnerEmail = string.Empty;
		public string WomensPartnerEmail = string.Empty;
		public string MensPartnerEmail = string.Empty;

		private static List<string> ignoreNames = new List<string> { "None", "none", "jim test", "Jim test woman", "xxx", "Not entered in mixed", "matthew test", "Cindy DeCoster" };
		private static Dictionary<string, string> translateName = new Dictionary<string, string> { { "M", string.Empty }, { "Jen Stai", "Jennifer Stai" }, { "Matt Acker", "Matthew Acker" }, { "Charles Robeson", "Charles Roberson" } };
		private static Dictionary<string, string> translateEmail = new Dictionary<string, string> { { "acker.matthee3@gmail.com", "acker.matthew3@gmail.com" } };
		private static List<string> ignorePartnerNames = new List<string> { "Not entered in mixed", "None" };

        internal static List<Person> Parse(IEnumerable<Registration> records)
        {
			var persons = new List<Person>();
			var recList = new List<Registration>();

            // first,clean bad data 
            foreach (var record in records)
            {
                record.Email = TranslateEmail(record.Email);
                record.MensPartnerEmail = TranslateEmail(record.MensPartnerEmail);
                record.WomensPartnerEmail = TranslateEmail(record.WomensPartnerEmail);
                record.MixedPartnerEmail = TranslateEmail(record.MixedPartnerEmail);

                record.Name = TranslateName(record.Name);
                record.MensPartnerName = TranslateName(record.MensPartnerName);
                record.WomensPartnerName = TranslateName(record.WomensPartnerName);
                record.MixedPartnerName = TranslateName(record.MixedPartnerName);
                recList.Add(record);
            }

            //flesh out the list of people
            foreach ( var record in recList)
			{
                AddPerson(persons, record.Name, record.Email, record.PhoneNumber, record.ShirtSize);
                AddPerson(persons, record.MixedPartnerName, record.MixedPartnerEmail, record.MixedPartnerPhone, record.MixedPartnerShirtSize);
                AddPerson(persons, record.WomensPartnerName, record.WomensPartnerEmail, record.WomensPartnerPhone, record.GenderPartnerShirtSize);
                AddPerson(persons, record.MensPartnerName, record.MensPartnerEmail, record.MensPartnerPhone, record.GenderPartnerShirtSize);
            }

			// second, add events
			foreach ( var record in recList)
			{
				var person = persons.FirstOrDefault(x => x.Name == record.Name.Trim());
				if (person != null)
				{
					var womensEvent = BuildEvent(record.WomensBracket, record.WomensPartnerName);
					var partner = persons.FirstOrDefault(x => x.Name == record.WomensPartnerName.Trim());
					AddEvent(person, womensEvent);
					AddPartnerEvent(person, partner, womensEvent, record.WomensPartnerName);

					var mensEvent = BuildEvent(record.MensBracket, record.MensPartnerName);
					partner = persons.FirstOrDefault(x => x.Name == record.MensPartnerName.Trim());
					AddEvent(person, mensEvent);
					AddPartnerEvent(person, partner, mensEvent, record.MensPartnerName);

					var mixedEvent = BuildEvent(record.MixedBracket, record.MixedPartnerName);
					partner = persons.FirstOrDefault(x => x.Name == record.MixedPartnerName.Trim());
					AddEvent(person, mixedEvent);
					AddPartnerEvent(person, partner, mixedEvent, record.MixedPartnerName);
				}
				else
				{
					if ( ! string.IsNullOrWhiteSpace(record.Name))
						Console.WriteLine($"ERROR - {record.Name} not found.");
				}

			}

			return persons;
        }

		private static void AddPartnerEvent(Person person, Person? partner, Event? eventToAdd,string partnerName)
		{
			if (partner == null)
			{
                if ( ! string.IsNullOrWhiteSpace(partnerName)
				  && ignorePartnerNames.FirstOrDefault(x => x == partnerName) == null)
                    Console.WriteLine($"Registration.Parse Partner({partnerName}) not found");
            }
			else if (eventToAdd != null)
			{ 
				var partnerEvent = new Event
				{
					DivisionLevel = eventToAdd.DivisionLevel,
					EventType = eventToAdd.EventType,
					PartnerName = person.Name
				};

                AddEvent(partner, partnerEvent);
            }
		}

        private static void AddEvent(Person person, Event? eventToAdd)
        {
			if (eventToAdd != null)
			{
				if (person.Events.FirstOrDefault( x => x.DivisionLevel == eventToAdd.DivisionLevel && x.EventType == eventToAdd.EventType) == null)
				{
					person.Events.Add(eventToAdd);
				}
			}
        }

        private static Event? BuildEvent(string bracket, string partnerName)
        {
			Event? result = null;
			switch (bracket)
			{
				case "Men's Doubles 3.0":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l30,
						EventType = EventType.mens
					};
					break;
				case "Men's Doubles 3.5":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l35,
						EventType = EventType.mens
					};
					break;
				case "Men's Doubles 4.0":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l40,
						EventType = EventType.mens
					};

					break;
				case "Men's Doubles 4.5":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l45,
						EventType = EventType.mens
					};
					break;
				case "Men's Doubles 3.5/4.0 - Age 55+ (both partners must be 55+)":
                    result = new Event
                    {
                        PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.s35plus,
                        EventType = EventType.mens
                    };
                    break;

                case "Mixed Doubles 2.5":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l25,
						EventType = EventType.mixed
					};
					break;
				case "Mixed Doubles 3.0":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l30,
						EventType = EventType.mixed
					};
					break;
				case "Mixed Doubles 3.5":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l35,
						EventType = EventType.mixed
					};
					break;
                case "Mixed Doubles 4.0":
                    result = new Event
                    {
                        PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l40,
                        EventType = EventType.mixed
                    };
                    break;
                case "Mixed doubles 4.5+":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l45plus,
						EventType = EventType.mixed
					};
					break;
				case "Mixed Doubles 2.5/3.0 - Age 55+ (both partners must be 55+)":
                    result = new Event
                    {
                        PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.s25_30,
                        EventType = EventType.mixed
                    };
                    break;
				case "Not Playing Men's Doubles":
					break;
				case "Not playing Mixed Doubles":
					break;
				case "Not Playing Women's Doubles":
					break;
				case "Women's Doubles 2.5":
                    result = new Event
                    {
                        PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l25,
                        EventType = EventType.womens
                    };
                    break;
				case "Women's Doubles 3.0":
					result = new Event
					{
						PartnerName = partnerName,
						DivisionLevel = DivisionLevel.l30,
						EventType = EventType.womens
					};
					break;
				case "Women's Doubles 3.5":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l35,
						EventType = EventType.womens
					};
					break;
                case "Women's Doubles 4.0":
                    result = new Event
                    {
                        PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l40,
                        EventType = EventType.womens
                    };
                    break;
                case "Women's Doubles 4.5+":
                    result = new Event
                    {
                        PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.l45plus,
                        EventType = EventType.womens
                    };
                    break;
                case "Women's Doubles 3.5/4.0 - Age 55+ (both partners must be 55+)":
					result = new Event
					{
						PartnerName = partnerName,
                        DivisionLevel = DivisionLevel.s35plus,
						EventType = EventType.womens
					};
                    break;

                default:
					Console.WriteLine($"Registration:BuildEvent - Missing DivisionName Name: {bracket}");
					break;
			}

			return result;
        }

        private static void AddPerson(List<Person> persons, string name, string email, string phoneNumber, string shirtSize)
        {
			if ( ! string.IsNullOrWhiteSpace(name)
			  && ignoreNames.FirstOrDefault(x => x == name.Trim()) == null 
			  && persons.FirstOrDefault(x => x.Name == name.Trim()) == null )
			{
				var ss = shirtSize;
				if (shirtSize == "Extra Large")
					ss = "XL";

				persons.Add(new Person
				{
					Email = TranslateEmail(email.Trim()),
					PhoneNumber = phoneNumber,
					Name = TranslateName(name.Trim()), ShirtSize = ss
				});
			}
        }

        private static string TranslateEmail(string email)
        {
			string result = email;
			if (translateEmail.TryGetValue(email.Trim(), out result))
                return result;
            else
                return email;
        }

        private static string TranslateName(string name)
        {
            string result = name;
			if (translateName.TryGetValue(name.Trim(), out result))
				return result;
			else
				return name;
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
                case DivisionLevel.l45plus:
					sb.Append("4.5+");
                    break;
                case DivisionLevel.s25_30:
                    sb.Append("55+ - 2.5, 3.0");
                    break;
                case DivisionLevel.s35plus:
					sb.Append("55+ - 3.5+");
                    break;
            }

			return sb.ToString();
        }
    }
}

