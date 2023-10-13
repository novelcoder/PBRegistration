using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Microsoft.Win32;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RegistrationTables
{
	public class PartnerEmailReport
	{
		public PartnerEmailReport()
		{
        }

        public static List<IList<object>> PartnerEmail(List<Person> people, List<Payment> payments, string sheetId)
        {
            var result = new List<IList<object>>();
            var colData = new List<object>();

            var partnerEmailReader = new PartnerEmailReader();
            var savedEmails = partnerEmailReader.ReadSpreadsheet(sheetId);

            var added = new List<string>();

            result.Add(new List<object>() { "Player / Partner", "Division", "Player Due", "Partner Due", "Emails", "Player Paid", "Partner Paid", "Congrats EML", "Need Money EML", "Need Data EML" });
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
                                colData.Add(NeedMoneyMessage(salutation, div, partner.Name, PaidMsg(partner, partnerPaid),
                                                                              person.Name, PaidMsg(person, personPaid)));
                                colData.Add(NeedDataMessage(salutation, div));
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
                                colData.Add(NeedMoneyMessage(salutation, div, person.Name, PaidMsg(person, personPaid)
                                                                            , partner.Name, PaidMsg(partner, partnerPaid)));
                                colData.Add(NeedDataMessage(salutation, div));
                            }

                            if (! FindPartnerEmail(savedEmails, person.Name, partner.Name))
                            //if ( result.Count() < 5)
                                result.Add(colData);
                        }
                    }
                }
            }

            // put in generic message about missing registration
            colData = new List<object>();
            for (int ii = 0; ii < 7; ii++)
                colData.Add(string.Empty);
            var sb = new StringBuilder();
            sb.Append("Hi -\n\n");
            sb.Append("I am so happy to see that you paid for the spots in the tournament.\n\n");
            sb.Append("Would you please complete the registration, so that we can get you into the correct skills divisions and checked in as one of the teams!\n\n");
            sb.Append("Here is the link to register:  https://geni.us/MDM-Reg\n\n");
            sb.Append("We look forward to seeing you there!!!\n\n");
            sb.Append("Cindy");
            colData.Add(sb.ToString());
            result.Add(colData);

            return result;
        }

        private static string PaidMsg(Person person, string personPaid)
        {
            var sb = new StringBuilder();
            var due = Event.Due(person.Events);
            int amtDue = 0;
            int amtPaid = 0;
            string displayEvents = Event.DisplayList(person.Events);

            Int32.TryParse(due, NumberStyles.Currency, CultureInfo.CurrentCulture, out amtDue);
            Int32.TryParse(personPaid, NumberStyles.Currency, CultureInfo.CurrentCulture, out amtPaid);

            if (amtDue <= amtPaid)
                sb.Append($"I have you paid in full for your {person.Events.Count()} events ({displayEvents}).");
            else if (amtPaid == 0)
                sb.Append($"We have not received payment from you for events ({displayEvents}) total {due}. Please, submit payment as soon as possible.");
            else
            {
                sb.Append($"We have received partial payment of ${amtPaid} ");
                sb.Append($"and show {person.Events.Count()} events ({displayEvents}) ");
                sb.Append($"leaving ${amtDue - amtPaid} unpaid. Please, submit payment as soon as possible.");
            }

            return sb.ToString();
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

        private static object NeedMoneyMessage(string salutation, string div, string personName, string personPaidMsg, string partnerName, string partnerPaidMsg)
        {
            var sb = new StringBuilder();
            sb.Append(salutation);
            sb.Append($"I have you registered for {div}.\n\n");
            sb.Append($"The first event is $40 and 2nd events are $20.\n\n");
            sb.Append($"{personName} - {personPaidMsg}\n\n");
            sb.Append($"{partnerName} - {partnerPaidMsg}\n\n");
            sb.Append("Please see the links below for payment options.\n\n");
            sb.Append("Venmo: https://venmo.com/pickleball-lincoln\n");
            sb.Append("Paypal Email: PickleballLincoln @gmail.com\n");
            sb.Append("Paypal Link: https://paypal.com/paypalme/pickleballLincoln\n\n");
            sb.Append($"Please let me know if you have any questions.\n\n");
            sb.Append("Cindy");
            return sb.ToString();
        }

        private static object NeedDataMessage(string salutation, string div)
        {
            var sb = new StringBuilder();
            sb.Append(salutation);
            sb.Append($"I show you are registered for {div}.\n\n");
            sb.Append("Unfortunately, some of your information is missing and we cannot complete your registration. Please reply to this email with the following information.\n\n\n");
            sb.Append($"Please let me know if you have any questions.\n\n");
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

