namespace AccessingSQLDatabaseFromCsharp
{
    internal class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }

        public Person()
        {
            
        }

        public Person(string firstName, string lastName, int birthYear, int id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }
    }
}
