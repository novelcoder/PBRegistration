using System;
using System.Text;

namespace RegistrationTables
{
	public class Event
    {
        public EventType EventType = EventType.none;
        public DivisionLevel DivisionLevel = DivisionLevel.none;
        public string PartnerName = string.Empty;
        public string PartnerPhoneNumber = string.Empty;
        public Tournaments Tournament = Tournaments.none;
        bool PartnerIsRegistered = false;

        internal static Event? FindEvent(List<Event> events, Tournaments tournament, EventType eventType, DivisionLevel divisionLevel, string partnerName, string partnerPhoneNumber)
        {
            var evt = events.FirstOrDefault(x => x.EventType == eventType
                                                     && x.Tournament == tournament
                                                     && x.DivisionLevel == divisionLevel
                                                     && string.Compare(x.PartnerName, partnerName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (evt == null)
            {
                evt = events.FirstOrDefault(x => x.EventType == eventType
                                         && x.Tournament == tournament
                                         && x.DivisionLevel == divisionLevel
                                         && string.Compare(x.PartnerPhoneNumber, partnerPhoneNumber, StringComparison.InvariantCultureIgnoreCase) == 0);
            }

            return evt;
        }

        internal static void AddEvents(List<Person> persons, Person person, string skillAndEvent, string partnerName, string partnerPhoneNumber, Tournaments tournament)
        {
            var partner = Person.FindPerson(persons, partnerName, partnerPhoneNumber);

            if (partner == null)
            {
                throw new Exception($"Partner not found {partnerName} - {partnerPhoneNumber}");
            }
            else
            {
                var possibleEvent = BuildEvent(tournament, skillAndEvent, partnerName, partnerPhoneNumber, partner.IsRegistered);
                var partnerEvent = BuildEvent(tournament, skillAndEvent, person.Name, person.PhoneNumber, person.IsRegistered);
                var existingPartnerEvent = Event.FindEvent(partner.Events, tournament, partnerEvent.EventType, partnerEvent.DivisionLevel, person.Name, person.PhoneNumber);
                if (existingPartnerEvent == null)
                {
                    partner.Events.Add(partnerEvent);
                }

                var existingEvent = Event.FindEvent(person.Events, possibleEvent.Tournament, possibleEvent.EventType, possibleEvent.DivisionLevel, partnerName, partnerPhoneNumber);
                if (existingEvent == null)
                {
                    person.Events.Add(possibleEvent);
                }
            }
        }
        private static Event BuildEvent(Tournaments tournament, string eventType, string partnerName, string partnerPhoneNumber, bool partnerIsRegistered)
        {
            Event result = new Event
            {
                Tournament = tournament,
                PartnerPhoneNumber = partnerPhoneNumber,
                PartnerName = partnerName,
                PartnerIsRegistered = partnerIsRegistered
            };
            switch (eventType)
            {
                case "2.5 - Men's":
                    result.DivisionLevel = DivisionLevel.l25;
                    result.EventType = EventType.mens;
                    break;
                case "3.0 - Men's":
                    result.DivisionLevel = DivisionLevel.l30;
                    result.EventType = EventType.mens;
                    break;
                case "3.5 - Men's":
                    result.DivisionLevel = DivisionLevel.l35;
                    result.EventType = EventType.mens;
                    break;
                case "4.0 - Men's":
                    result.DivisionLevel = DivisionLevel.l40;
                    result.EventType = EventType.mens;
                    break;
                case "4.5 - Men's":
                    result.DivisionLevel = DivisionLevel.l45;
                    result.EventType = EventType.mens;
                    break;

                case "2.5 - Women's":
                    result.DivisionLevel = DivisionLevel.l25;
                    result.EventType = EventType.womens;
                    break;
                case "3.0 - Women's":
                    result.DivisionLevel = DivisionLevel.l30;
                    result.EventType = EventType.womens;
                    break;
                case "3.5 - Women's":
                    result.DivisionLevel = DivisionLevel.l35;
                    result.EventType = EventType.womens;
                    break;
                case "4.0 - Women's":
                    result.DivisionLevel = DivisionLevel.l40;
                    result.EventType = EventType.womens;
                    break;
                case "4.5 - Women's":
                    result.DivisionLevel = DivisionLevel.l45;
                    result.EventType = EventType.womens;
                    break;

                case "2.5 - Mixed":
                    result.DivisionLevel = DivisionLevel.l25;
                    result.EventType = EventType.mixed;
                    break;
                case "3.0 - Mixed":
                    result.DivisionLevel = DivisionLevel.l30;
                    result.EventType = EventType.mixed;
                    break;
                case "3.5 - Mixed":
                    result.DivisionLevel = DivisionLevel.l35;
                    result.EventType = EventType.mixed;
                    break;
                case "4.0 - Mixed":
                    result.DivisionLevel = DivisionLevel.l40;
                    result.EventType = EventType.mixed;
                    break;
                case "4.5 - Mixed":
                    result.DivisionLevel = DivisionLevel.l45;
                    result.EventType = EventType.mixed;
                    break;
                default:
                    throw new Exception($"Build Event - bad input {eventType}");
            }

            return result;
        }
        public static List<Tuple<EventType, DivisionLevel>> Divisions()
        {
            return new List<Tuple<EventType, DivisionLevel>>()
            {
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l25),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l30),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l35),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l40),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l45),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l25),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l30),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l35),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l40),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l45),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l25),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l30),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l35),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l40),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l45)
            };
        }   

        internal static string DisplayList(List<Event> events)
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach ( var evt in events)
            {
                if (first)
                    first = false;
                else
                    sb.Append("; ");
                sb.AppendFormat($"{evt.EventType.ToString()} - {evt.DivisionLevel.ToString()}");
            }
            return sb.ToString();
        }

        internal static string DivisionTabName(EventType evtType, DivisionLevel divisionLevel)
        {
            var sb = new StringBuilder();

            switch (evtType)
            {
                case EventType.mens:
                    sb.Append("Mens ");
                    break;
                case EventType.womens:
                    sb.Append("Womens ");
                    break;
                case EventType.mixed:
                    sb.Append("Mixed ");
                    break;
            }

            switch (divisionLevel)
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

        internal static string Due(List<Event> events)
        {
            return $"{events.Count * 30}";
        }
    }
}



