using System;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;


namespace AccessingSQLDatabaseFromCsharp
{
    internal class Repository<T>
    {   
            private SqlConnection _connection;

            public Repository(SqlConnection connection)
            {
                _connection = connection;
            }

            public async Task<int> Create(T obj)
            {
                var type = typeof(T);
                var props = type.GetProperties();

                var sql = $"INSERT INTO {type.Name} ({GetParams(props)}) VALUES ({GetParams(props, true)})";


                return await _connection.ExecuteAsync(sql, obj);
            }

            private static string GetParams(PropertyInfo[] props, bool includeAt = false)
            {
                // LINQ versjonen av det under, + mulighet for å legge til en @
                return string.Join(',', props.Where(p => p.Name != "Id").Select(p => (includeAt ? "@" : "") + p.Name));

                //var paramsList = "";
                //foreach (var prop in props.Where(p => p.Name != "Id"))
                //{
                //    if (paramsList != "") paramsList += ", ";
                //    paramsList += prop.Name;
                //}
                //return paramsList;
            }

        public async Task<IEnumerable<Person>> ReadAll()
            {
                var sql = @"SELECT Id, FirstName, LastName, BirthYear 
                        FROM Person";
                return await _connection.QueryAsync<Person>(sql);
            }

            public async Task<IEnumerable<Person>> ReadYoungerThan(int birthYearMin)
            {
                var sql = @"SELECT Id, FirstName, LastName, BirthYear 
                        FROM Person
                        WHERE BirthYear > @BirthYearMin";
                return await _connection.QueryAsync<Person>(sql, new { BirthYearMin = birthYearMin });
            }

            public async Task<Person> ReadOneById(int id)
            {
                var sql = @"SELECT Id, FirstName, LastName, BirthYear
                        FROM Person
                        WHERE Id = @Id";
                return await _connection.QueryFirstOrDefaultAsync<Person>(sql, new { Id = id });
            }

            public async Task<Person> ReadOneByName(string firstName)
            {
                var sql = @"SELECT Id, FirstName, LastName, BirthYear
                        FROM Person
                        WHERE FirstName = @FirstName";
                return await _connection.QueryFirstOrDefaultAsync<Person>(sql, new { FirstName = firstName });
            }

            public async Task<int> Update(Person person)
            {
                var sql = @"UPDATE Person 
                       SET LastName = @LastName, FirstName = @FirstName, BirthYear = @BirthYear
                       WHERE Id = @Id";
                return await _connection.ExecuteAsync(sql, person);
            }

            public async Task<int> Update(int id, string firstName, string lastName, int birthYear)
            {
                var sql = @"UPDATE Person 
                       SET LastName = @LastName, FirstName = @FirstName, BirthYear = @BirthYear
                       WHERE Id = @Id";
                return await _connection.ExecuteAsync(sql, new { FirstName = firstName, LastName = lastName, BirthYear = birthYear, Id = id });
            }

            public async Task<int> DeleteAll()
            {
                var sql = @"DELETE FROM Person";
                return await _connection.ExecuteAsync(sql);
            }

            public async Task<int> DeleteOne(int id)
            {
                var sql = @"DELETE FROM Person WHERE Id = @Id";
                return await _connection.ExecuteAsync(sql, new { Id = id });
            }

            public async Task<int> DeleteOne(Person person)
            {
                var sql = @"DELETE FROM Person WHERE Id = @Id";
                return await _connection.ExecuteAsync(sql, person);
            }

            // Samler de metodene over inni en metode, så man slipper å repetere så mye kode
            public async Task<int> DeleteOne(Person person = null, int? id = null)
            {
                var sql = @"DELETE FROM Person WHERE Id = @Id";
                return await _connection.ExecuteAsync(sql, person ?? (object)new { Id = id.Value });
            }
        }
    }
