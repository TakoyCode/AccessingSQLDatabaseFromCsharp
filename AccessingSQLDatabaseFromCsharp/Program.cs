using System.Data.SqlClient;
using Dapper;

namespace AccessingSQLDatabaseFromCsharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestQueries().Wait();
            //TestQueriesWithRepository().Wait();
        }

        static async Task TestQueriesWithRepository()
        {
            var connStr = @"Data Source=(localdb)\local;Initial Catalog=AccessingSQLDatabaseFromCsharp;Integrated Security=True;";
            var conn = new SqlConnection(connStr);

            var repo = new PersonRepository(conn);

            int rowsDeleted = await repo.DeleteAll();

            int rowsInserted1 = await repo.Create(new Person("Audun", "Nicolaisen", 2001));
            int rowsInserted2 = await repo.Create(new Person("Per", null, 1980));
            int rowsInserted3 = await repo.Create(new Person("Pål", null, 1981));

            IEnumerable<Person> persons = await repo.ReadAll();

            Person audun = await repo.ReadOneByName("Audun");

            audun.FirstName = "Petter";
            audun.LastName = "Pettersen";
            int rowsAffected2 = await repo.Update(audun);
            //int rowsAffected2 = await repo.Update(new Person("Petter", "Pettersen", 2001, audun.Id));

            persons = await repo.ReadAll();

            int rowsAffected3 = await repo.DeleteOne(audun);
            //int rowsAffected3 = await repo.DeleteOne(audun.Id);

            persons = await repo.ReadAll();
        }

        static async Task TestQueries()
        {
            var connStr = @"Data Source=(localdb)\local;Initial Catalog=AccessingSQLDatabaseFromCsharp;Integrated Security=True;";
            var conn = new SqlConnection(connStr);

            var readMany = @"SELECT Id, FirstName, LastName, BirthYear 
                             FROM Person";

            // DECLARE @Id int = 1;
            var readOne = @"SELECT Id, FirstName, LastName, BirthYear
                            FROM Person
                            WHERE Id = @Id";

            var readOneByName = @"SELECT Id, FirstName, LastName, BirthYear
                                  FROM Person
                                  WHERE FirstName = @FirstName";

            var create = @"INSERT INTO Person (FirstName, LastName, BirthYear)
                           VALUES (@FirstName, @LastName, @BirthYear)";

            var delete = @"DELETE FROM Person WHERE Id = @Id";

            var deleteAll = @"DELETE FROM Person";

            var update = @"UPDATE Person 
                           SET LastName = @LastName, FirstName = @FirstName
                           WHERE Id = @Id";

            int rowsDeleted = await conn.ExecuteAsync(deleteAll);

            int rowsInserted1 = await conn.ExecuteAsync(create, new { FirstName = "Audun", LastName = "Nicolaisen", BirthYear = 2001 });
            int rowsInserted2 = await conn.ExecuteAsync(create, new { FirstName = "Per", LastName = (string)null, BirthYear = 1980 });
            int rowsInserted3 = await conn.ExecuteAsync(create, new { FirstName = "Pål", LastName = (string)null, BirthYear = 1981 });

            IEnumerable<Person> persons = await conn.QueryAsync<Person>(readMany);

            Person audun = await conn.QueryFirstOrDefaultAsync<Person>(readOneByName, new { FirstName = "Audun" });

            int rowsAffected2 = await conn.ExecuteAsync(update, new { FirstName = "Petter", LastName = "Pettersen", Id = audun.Id});
            persons = await conn.QueryAsync<Person>(readMany);

            int rowsAffected3 = await conn.ExecuteAsync(delete, new { Id = audun.Id });
            persons = await conn.QueryAsync<Person>(readMany);

        }


    }
}
