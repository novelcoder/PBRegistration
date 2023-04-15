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
        public List<Event> Events = new List<Event>();

        internal bool ParticipatesIn(EventType type, DivisionLevel division)
        {
            return Events.FirstOrDefault(x => x.DivisionLevel == division && x.EventType == type) != null;
        }

        internal static List<Person> DivisionList(List<Person> persons, EventType eventType, DivisionLevel level)
        {
            List<Person> result = new List<Person>();

            foreach (var person in persons)
            {
                if (person.ParticipatesIn(eventType, level))
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

