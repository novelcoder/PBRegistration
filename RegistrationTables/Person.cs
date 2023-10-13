using System;
using System.Text;

namespace RegistrationTables
{
    public class Person
    {
        public string Name = string.Empty;
        public string Email = string.Empty;
        public string ShirtSize = string.Empty;
        public string PhoneNumber = string.Empty;
        public int SelfReportedNumEvents = 0;
        public bool IsRegistered = false;
        public List<Event> Events = new List<Event>();

        internal static Person? FindPerson(List<Person> persons, string name, string phoneNumber)
        {
            var result = persons.FirstOrDefault(x =>
                string.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (result == null)
                result = persons.FirstOrDefault(x => x.PhoneNumber == phoneNumber);

            return result;
        }

        internal static int CountPerTournament(List<Person> persons, Tournaments tournament)
        {
            return persons.Count(x => x.Events.Exists(y => y.Tournament == tournament));
        }

        internal bool ParticipatesIn(Tournaments tournament, EventType type, DivisionLevel division)
        {
            return Events.FirstOrDefault(x => x.DivisionLevel == division && x.EventType == type && x.Tournament == tournament) != null;
        }

        internal static List<TwoPeople> DivisionPartners(List<Person> persons, EventType eventType, DivisionLevel divisionLevel, Tournaments tournament)
        {
            var result = new List<TwoPeople>();
            foreach (var person in persons)
            {
                var evt = person.Events.FirstOrDefault(x => x.EventType == eventType
                                                         && x.Tournament == tournament
                                                         && x.DivisionLevel == divisionLevel);
                if (evt != null)
                {
                    var partner = Person.FindPerson(persons, evt.PartnerName, evt.PartnerPhoneNumber);
                    var twoPeople = new TwoPeople(person, partner);
                    if (result.FirstOrDefault( x => x == twoPeople) is null)
                    {
                        result.Add(twoPeople);
                    }
                }
            }

            result.Sort();

            return result;
        }

        internal static List<Person> DivisionList(List<Person> persons, EventType eventType, DivisionLevel level, Tournaments tournament)
        {
            List<Person> result = new List<Person>();

            foreach (var person in persons)
            {
                if (person.ParticipatesIn(tournament, eventType, level))
                    result.Add(person);
            }

            return result;
        }

        internal static List<IList<object>> PrintDivision(List<Person> list, EventType evtType, DivisionLevel divisionLevel)
        {
            var result = new List<IList<object>>();
            var colData = new List<object>() { "Person", "Email", "ShirtSize", "PartnerName", "Due", "Events" };

            result.Add( colData ); // add headings

            foreach (var person in list)
            {
                var evt = person.Events
                    .FirstOrDefault(x => x.DivisionLevel == divisionLevel && x.EventType == evtType) ?? new Event();

                colData = new List<object>();
                colData.Add(person.Name);
                colData.Add(person.Email);
                colData.Add(person.ShirtSize);
                colData.Add(evt.PartnerName);
                colData.Add(Event.Due(person.Events));
                colData.Add(Event.DisplayList(person.Events));
                result.Add(colData);
            }
            return result;
        }
    }
}

