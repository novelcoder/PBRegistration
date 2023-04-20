using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RegistrationTables
{
	public class PartnerEmailReport
	{
		public PartnerEmailReport()
		{
        }

        public static List<IList<object>> PartnerEmail(List<Person> people, List<Payment> payments)
        {
            var result = new List<IList<object>>();
            var colData = new List<object>();

            var partnerEmailReader = new PartnerEmailReader();
            var savedEmails = partnerEmailReader.ReadSpreadsheet();

            var added = new List<string>();

            result.Add(new List<object>() { "Player / Partner", "Division", "Player Due", "Partner Due", "Emails", "Player Paid", "Partner Paid", "Congrats EML" });
            foreach (var person in people)
            {
                foreach (var evt in person.Events)
                {
                    var partner = people.FirstOrDefault(x => x.Name == evt.PartnerName);
                    if (partner != null)
                    {
                        // switch order to alphabetic
                        bool flipPartner = person.Name.CompareTo(partner.Name) > 0;
                        
                        string nameToAdd = $"{person.Name} / {partner.Name}";
                        string salutation = Salutation(person.Name, partner.Name);
                        if (flipPartner)
                        {
                            nameToAdd = $"{partner.Name} / {person.Name}";
                            salutation = Salutation(partner.Name, person.Name);
                        }

                        var personPaymentRow = payments.FirstOrDefault( x => x.Name == person.Name);
                        var partnerPaymentRow = payments.FirstOrDefault(x => x.Name == partner.Name);
                        var personPaid = personPaymentRow?.Paid ?? string.Empty;
                        var partnerPaid = partnerPaymentRow?.Paid ?? string.Empty;

                        if (added.FirstOrDefault(x => x == nameToAdd) == null)
                        {
                            string div = Registration.DivisionName(evt);
                            string personDue = Event.Due(person.Events);
                            string partnerDue = Event.Due(partner.Events);

                            added.Add(nameToAdd);
                            colData = new List<object>();
                            if (flipPartner)
                            {
                                colData.Add(nameToAdd);
                                colData.Add(div);
                                colData.Add(partnerDue);
                                colData.Add(personDue);
                                colData.Add($"{partner.Email}; {person.Email}");
                                colData.Add(partnerPaid);
                                colData.Add(personPaid);
                                colData.Add(SuccessMessage(salutation, div, partnerDue, personDue ));
                            }
                            else 
                            {
                                colData.Add(nameToAdd);
                                colData.Add(div);
                                colData.Add(personDue);
                                colData.Add(partnerDue);
                                colData.Add($"{person.Email}; {partner.Email}");
                                colData.Add(personPaid);
                                colData.Add(partnerPaid);
                                colData.Add(SuccessMessage(salutation, div, partnerDue, personDue));
                            }

                            if (! FindPartnerEmail(savedEmails, person.Name, partner.Name))
                            //if ( result.Count() < 5)
                                result.Add(colData);
                        }
                    }
                }
            }
            return result;
        }

        private static object SuccessMessage(string salutation, string div, string partnerDue, string personDue)
        {
            var sb = new StringBuilder();
            sb.Append(salutation);
            sb.Append($"I have you registered for {div} and all payments have been received.\n\n");
            sb.Append($"We look forward to seeing you!\n\n");
            sb.Append($"Have a great night!\n\n");
            sb.Append("Cindy");
            return sb.ToString();
        }

        private static string Salutation(string name1, string name2)
        {
            var firstNameOne = FirstName(name1);
            var firstNameTwo = FirstName(name2);

            return $"Congratulations {firstNameOne} and {firstNameTwo} - \n\n";
        }

        private static string FirstName(string name2)
        {
            var result = name2;
            var names = name2.Split();
            if (names.Count() >= 1)
                result = names[0];
            return result;
        }

        private static bool FindPartnerEmail(List<PartnerEmail> savedEmails, string personName, string partnerName)
        {
            bool result = false;

            foreach (var emailRow in savedEmails)
            {
                if ( emailRow.CombinedName.IndexOf(personName) >= 0
                  && emailRow.CombinedName.IndexOf(partnerName) >= 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}

