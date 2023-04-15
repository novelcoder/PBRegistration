using CsvHelper;
using System.Globalization;
using RegistrationTables;
using System.IO;
using System.Text;

Program program = new();
program.Start();


partial class Program
{
    public void Start()
    {
        Console.WriteLine("start reader");


        using (var writer = new StreamWriter("/users/jamesgreenwood/OneDrive/Pickleball/data/MayDayMeleeReports.csv", false))
        //using (var rdr = new StreamReader("/users/jamesgreenwood/OneDrive/Pickleball/data/Form Responses.csv"))
        {
            //using (var rdr = new StreamReader("/users/jamesgreenwood/OneDrive/Pickleball/data/KellyMacForm.csv"))
            //using (var csv = new CsvReader(rdr, CultureInfo.InvariantCulture))
            //{
            //    csv.Context.RegisterClassMap<RegistrationMap>();

            //    var records = csv.GetRecords<Registration>();
            //var people = Registration.Parse(records);
            var rr = new ResponseReader();
            var sheetWriter = new SheetWriter();

            var records = rr.ReadSheet();
            var people = Registration.Parse(records);

            // Division reports
            IndividualDivisionReports(sheetWriter, people);

            // Shirts Report
            ShirtReport(sheetWriter, people);

            //All Division Reports
            var dataToWrite = AllDivisionReport(people);
            sheetWriter.EraseSheetData("ALL Divisions!A1:Y");
            sheetWriter.BulkWriteRange("ALL Divisions!A1:Y", dataToWrite);

            //Check In Report
            dataToWrite = CheckInReport(people);
            sheetWriter.EraseSheetData("Check In!A1:Y");
            sheetWriter.BulkWriteRange("Check In!A1:Y", dataToWrite);

            //Partner Email
            dataToWrite = PartnerEmail(people);
            sheetWriter.EraseSheetData("PartnerEmail!A1:Y");
            sheetWriter.BulkWriteRange("PartnerEmail!A1:Y", dataToWrite);

            //MailChimp Upload
            dataToWrite = MailChimpUpload(people);
            sheetWriter.EraseSheetData("MailChimp!A1:Y");
            sheetWriter.BulkWriteRange("MailChimp!A1:Y", dataToWrite);

            //Payments
            var paymentReader = new ReadPayments();
            var payments = paymentReader.ReadSpreadsheet();
            var sheetName = $"Payments!A{payments.Count() + 1}";
            var missingPeople = new List<Person>();
            foreach ( var person in people)
            {
                if (payments.FirstOrDefault( x => x.Name == person.Name) == null )
                {
                    missingPeople.Add(person);
                }
            }
            dataToWrite = MissingPeopleData(missingPeople);
            sheetWriter.BulkWriteRange(sheetName, dataToWrite);
        }
    }

    private static List<IList<object>> MailChimpUpload(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();

        result.Add( new List<object>() { "email","first","last","phone","tags" } );
        foreach (var person in people)
        {
            string[] names = person.Name.Split();
            string firstName=string.Empty;
            string last=string.Empty;
            if (names.Count()>=1)
                firstName = names[0]; 
            if (names.Count() >= 2)
                last = names[1];

            var sb = new StringBuilder();
            var first = true;
            foreach (var evt in person.Events)
            {
                if (first)
                    first = false;
                else
                    sb.Append(",");
                sb.AppendFormat($"MDM23_{evt.DivisionLevel.ToString()}_{evt.EventType.ToString()}");
            }

            colData = new List<object>();
            colData.Add(person.Email);
            colData.Add(firstName);
            colData.Add(last);
            colData.Add($"'{person.PhoneNumber}");
            colData.Add($"\"MDM23,{sb.ToString()}\"");
            result.Add(colData);
        }

        return result;
    }

    private static List<IList<object>> MissingPeopleData(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();

        foreach ( var person in people)
        {
            colData = new List<object>();
            colData.Add(person.Name);
            colData.Add(Event.DisplayList(person.Events));
            colData.Add(Event.Due(person.Events));
            colData.Add(string.Empty);
            colData.Add(string.Empty);
            colData.Add(string.Empty);
            colData.Add(person.ShirtSize);
            colData.Add(person.Email);
            result.Add(colData);
        }

        return result;
    }

    private static List<IList<object>> PartnerEmail(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();

        result.Add(new List<object>() { "Player / Partner", "Division", "Player Due", "Partner Due", "Emails" });
        foreach (var person in people)
        {
            foreach (var evt in person.Events)
            {
                var partner = people.FirstOrDefault(x => x.Name == evt.PartnerName);
                if (partner != null)
                {
                    colData = new List<object>();
                    colData.Add($"{person.Name} / {partner.Name}");
                    colData.Add(Registration.DivisionName(evt));
                    colData.Add(Event.Due(person.Events));
                    colData.Add(Event.Due(partner.Events));
                    colData.Add($"{person.Email}; {partner.Email}");
                    result.Add(colData);
                }
            }
        }
        return result;
    }
    private static List<IList<object>> CheckInReport(List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();
        //
        // Check In Report
        //
        people.Sort((x, y) => x.Name.CompareTo(y.Name));
        // day one
        colData.Add("Check In Report - Day One");
        result.Add(colData);
        colData = new List<object>() { string.Empty };
        result.Add(colData);

        foreach (var person in people)
        {
            if (Event.AnyDayOne(person.Events))
            {
                colData = new List<object>();
                colData.Add($"{person.Name}");
                colData.Add(Event.DisplayList(person.Events));
                colData.Add(Event.Due(person.Events));
                colData.Add(person.Email);
                colData.Add(person.ShirtSize);
                result.Add(colData);
            }
        }

        // day two
        colData = new List<object>() { string.Empty };
        result.Add(colData);

        colData = new List<object>();
        colData.Add("Check In Report - Day Two");
        result.Add(colData);

        colData = new List<object>() { string.Empty };
        result.Add(colData);

        foreach (var person in people)
        {
            if (Event.AnyDayTwo(person.Events))
            {
                colData = new List<object>();
                colData.Add($"{person.Name}");
                colData.Add(Event.DisplayList(person.Events));
                colData.Add(Event.Due(person.Events));
                colData.Add(person.Email);
                colData.Add(person.ShirtSize);
                result.Add(colData);
            }
        }
        return result;
    }

    private static void ShirtReport(SheetWriter writer, List<Person> people)
    {
        var result = new List<IList<object>>();
        var colData = new List<object>();
        var shirts = new Dictionary<string, int>();

        colData.Add("Shirt Size");
        colData.Add("Count");
        result.Add(colData);

        foreach ( var person in people)
        {
            if (shirts.ContainsKey(person.ShirtSize))
                shirts[person.ShirtSize]++;
            else
                shirts[person.ShirtSize] = 1;
        }
        foreach ( var key in shirts.Keys)
        {
            colData = new List<object>();
            colData.Add(key);
            colData.Add(shirts[key].ToString());
            result.Add(colData);
        }
        colData = new List<object>();
        colData.Add(string.Empty);
        colData.Add($"=sum(B1:B{result.Count()})");
        result.Add(colData);
        writer.EraseSheetData("Shirts!A1:Y");
        writer.BulkWriteRange("Shirts!A1:Y", result);
    }

    private static void IndividualDivisionReports(SheetWriter writer, List<Person> people)
    {
        foreach (int evtType in Enum.GetValues(typeof(EventType)))
        {
            foreach (int blvl in Enum.GetValues(typeof(DivisionLevel)))
            {
                var list = Person.DivisionList(people, (EventType)evtType, (DivisionLevel)blvl);
                if (list.Count > 0)
                {
                    var eType = (EventType)evtType;
                    var divisionLevel = (DivisionLevel)blvl;
                    var divisionData = Person.PrintDivision(list, eType, divisionLevel);
                    var tabName = Event.DivisionTabName(eType, divisionLevel);
                    writer.BulkWriteRange(tabName, divisionData);
                }
            }
        }
    }

    //
    // AllDivisionReport
    //
    private static List<IList<object>> AllDivisionReport( List<Person> people)
    {
        var result = new List<IList<object>>();
        var divisionListsOne = new List<Tuple<string, List<Person>>>();
        var divisionListsTwo = new List<Tuple<string, List<Person>>>();
        foreach (int evtType in Enum.GetValues(typeof(EventType)))
            foreach (int dLvl in Enum.GetValues(typeof(DivisionLevel)))
            {
                var eventType = (EventType)evtType;
                var divisionLevel = (DivisionLevel)dLvl;
                
                var list = Person.DivisionList(people, eventType, (DivisionLevel)dLvl);
                if (list.Count > 0)
                {
                    var division = Registration.DivisionName((EventType)evtType, (DivisionLevel)dLvl);

                    if (Event.IsDayOne(divisionLevel))
                        divisionListsOne.Add(new Tuple<string, List<Person>>(division, list));
                    else
                        divisionListsTwo.Add(new Tuple<string, List<Person>>(division, list));
                }
            }

        AllDivisionDetails(result, divisionListsOne, "One");
        AllDivisionDetails(result, divisionListsTwo, "Two");

        return result;
    }

    private static void AllDivisionDetails(List<IList<object>> dataToWrite, List<Tuple<string, List<Person>>> divisionLists, string day)
    {
        bool written = true;

        dataToWrite.Add( new List<object>() { $"Division List - Day {day}" } );
        dataToWrite.Add(new List<object>() { "" });

        // write headings
        var values = new List<object>();
        foreach (var divisionList in divisionLists)
        {
            values.Add(divisionList.Item1);
        }
        dataToWrite.Add(values);

        int row = 0;
        while (written)
        {
            written = false;
            values = new List<object>();
            for (int col = 0; col < divisionLists.Count; col++)
            {
                if (divisionLists[col].Item2.Count > row)
                {
                    values.Add(divisionLists[col].Item2[row].Name);
                    written = true;
                }
            }
            row++;
            dataToWrite.Add(values);
        }
    }
}

