using System.Data.Common;
using System.Data.SqlClient;
using Dapper;

namespace AccessingSQLDatabaseFromCsharp
{
    internal class PersonRepository2 : Repository<Person>
    {
        public PersonRepository2(SqlConnection connection) : base(connection)
        {
        }

        public async Task<IEnumerable<Person>> ReadYoungerThan(int birthYearMin)
        {
            var sql = @"SELECT Id, FirstName, LastName, BirthYear 
                        FROM Person
                        WHERE BirthYear > @BirthYearMin";
            return await _connection.QueryAsync<Person>(sql, new { BirthYearMin = birthYearMin });

            // Kan også gjøre dette med linq, men da tar du inn først all daten og så sorterer den i C#
            // Men da kan du tydeligvis støtte på problemer etterhvert, så bedre å bare gjøre det i SQL(som eksemplet over)   
            //var persons = await ReadAll();
            //return persons.Where(p => p.BirthYear > birthYearMin);
        }
    }
}
