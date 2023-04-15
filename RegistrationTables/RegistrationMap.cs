using System;
using System.Xml.Linq;
using CsvHelper.Configuration;

namespace RegistrationTables
{
	public class RegistrationMap : ClassMap<Registration>
	{
		public RegistrationMap()
		{
            Map(m => m.Timestamp).Index(0);
            Map(m => m.Email).Index(1);
            Map(m => m.Name).Index(2);
            Map(m => m.NotEmail).Index(3);
            Map(m => m.PhoneNumber).Index(4);
            Map(m => m.MensBracket).Index(5);
            Map(m => m.MixedPartnerName).Index(6);
            Map(m => m.WomensPartnerName).Index(7);
            Map(m => m.ShirtSize).Index(8);

            Map(m => m.WomensBracket).Index(9);

            Map(m => m.MixedBracket).Index(10);

            Map(m => m.MixedPartnerShirtSize).Index(11);
            Map(m => m.GenderPartnerShirtSize).Index(12);

            Map(m => m.MensPartnerName).Index(13);

            Map(m => m.MixedPartnerPhone).Index(14);

            Map(m => m.WomensPartnerPhone).Index(15);

            Map(m => m.MensPartnerPhone).Index(16);

            Map(m => m.MixedPartnerEmail).Index(17);

            Map(m => m.WomensPartnerEmail).Index(18);

            Map(m => m.MensPartnerEmail).Index(19);

        }
	}
}

