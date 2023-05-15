using System;
using System.Text;

namespace RegistrationTables
{
	public class Event
    {
        public EventType EventType = EventType.none;
        public DivisionLevel DivisionLevel = DivisionLevel.none;
        public string PartnerName = string.Empty;

        public static List<Tuple<EventType, DivisionLevel>> DayOneDivisions()
        {
            return new List<Tuple<EventType, DivisionLevel>>()
            {
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l25),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l30),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.s25_30),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.s35plus),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l25),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l30),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.s25_30),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.s35plus),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l25),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.s25_30),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l30),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.s35plus)

            };
        }

        public static List<Tuple<EventType, DivisionLevel>> DayTwoDivisions()
        {
            return new List<Tuple<EventType, DivisionLevel>>()
            {
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l35),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l40),
                new Tuple<EventType, DivisionLevel>(EventType.womens, DivisionLevel.l45plus),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l35),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l40),
                new Tuple<EventType, DivisionLevel>(EventType.mens, DivisionLevel.l45),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l35),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l40),
                new Tuple<EventType, DivisionLevel>(EventType.mixed, DivisionLevel.l45plus)

            };
        }

        public static bool IsDayOne(DivisionLevel divisionLevel)
        {
            if ( divisionLevel == DivisionLevel.s25_30
              || divisionLevel == DivisionLevel.s35plus
              || divisionLevel == DivisionLevel.l25
              || divisionLevel == DivisionLevel.l30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDayOne()
        {
            return IsDayOne(DivisionLevel);
        }

        public bool IsDayTwo()
        {
            return !IsDayOne();
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

        internal static string Due(List<Event> events)
        {
            if (events.Count() == 1)
                return "$40";
            else if (events.Count() == 2)
                return "$60";
            else if (events.Count() >= 3)
                return "$80";

            return "$0";
        }

        internal static bool AnyDayOne(List<Event> events)
        {
            bool result = false;
            foreach (var evt in events)
            {
                if (evt.IsDayOne())
                    result = true;
            }
            return result;
        }

        internal static bool AnyDayTwo(List<Event> events)
        {
            bool result = false;
            foreach (var evt in events)
            {
                if (evt.IsDayTwo())
                    result = true;
            }
            return result;
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
                case DivisionLevel.l45plus:
                    sb.Append("4.5+");
                    break;
                case DivisionLevel.s25_30:
                    sb.Append("55+ 2.5-3.0");
                    break;
                case DivisionLevel.s35plus:
                    sb.Append("55+ 3.5+");
                    break;
            }

            return sb.ToString();
        }
    }
}



