using System;
namespace RegistrationTables
{
    public class TwoPeople : IComparable<TwoPeople>
	{
		public string Person1 { get; private set; } = string.Empty;
		public string Person2 { get; private set; } = string.Empty;


        public TwoPeople(string name1, string name2)
        {
            AssignNames(name1, name2);
        }

        public TwoPeople(Person person1, Person? person2)
        {
            if (person2 is null)
                throw new Exception("[TwoPeople:Constructor] null second person.");
            else
                AssignNames(person1.Name, person2.Name);
        }

        private void AssignNames(string name1, string name2)
        {
            if (name1.CompareTo(name2) < 0)
            {
                Person1 = name1;
                Person2 = name2;
            }
            else
            {
                Person1 = name2;
                Person2 = name1;
            }
        }

        public override string ToString()
        {
            return $"{Person1}, {Person2}";
        }

        public int CompareTo(TwoPeople? right)
        {
            if (ReferenceEquals(right, null))
                return 1;

            return this.ToString().CompareTo(right?.ToString());
        }

        public static bool operator ==(TwoPeople obj1, TwoPeople? obj2)
        {
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
			return obj1.Person1 == obj2.Person1 && obj1.Person2 == obj2.Person2;
        }

        public static bool operator !=(TwoPeople obj1, TwoPeople obj2)
        {
            if (ReferenceEquals(obj1, null))
                return true;
            if (ReferenceEquals(obj2, null))
                return true;
            return obj1.Person1 != obj2.Person1 || obj1.Person2 != obj2.Person2;
        }

        public bool Equals(TwoPeople? other)
        {
            return this == other;
        }
        public override bool Equals(object? obj) => Equals(obj as TwoPeople);

        public override int GetHashCode()
        {
            return Person1.GetHashCode() + Person2.GetHashCode();
        }
    }
}

