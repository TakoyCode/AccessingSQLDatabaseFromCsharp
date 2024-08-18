using Dapper;
using System.Data.SqlClient;
using System.Reflection;


namespace AccessingSQLDatabaseFromCsharp
{
    internal class Repository<T> where T : class
    {
        protected SqlConnection _connection;

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
        private static string GetSetters(PropertyInfo[] props)
        {
            return string.Join(',', props.Where(p => p.Name != "Id").Select(p => p.Name + " = @" + p.Name));
        }

        public async Task<IEnumerable<T>> ReadAll()
        {
            var type = typeof(T);
            var props = type.GetProperties();

            var sql = $"SELECT Id, {GetParams(props)} FROM {type.Name}";
            return await _connection.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<Person>> ReadYoungerThan(int birthYearMin)
        {
            var sql = @"SELECT Id, FirstName, LastName, BirthYear 
                    FROM Person
                    WHERE BirthYear > @BirthYearMin";
            return await _connection.QueryAsync<Person>(sql, new { BirthYearMin = birthYearMin });
        }

        public async Task<T> ReadOneById(int id)
        {
            var type = typeof(T);
            var props = type.GetProperties();

            var sql = $"SELECT Id, {GetParams(props)} FROM {type} WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<Person> ReadOneByName(string firstName)
        {
            var sql = @"SELECT Id, FirstName, LastName, BirthYear
                    FROM Person
                    WHERE FirstName = @FirstName";
            return await _connection.QueryFirstOrDefaultAsync<Person>(sql, new { FirstName = firstName });
        }

        public async Task<int> Update(T obj)
        {
            var type = typeof(T);
            var props = type.GetProperties();

            var sql = $"UPDATE {type.Name} SET {GetSetters(props)} WHERE Id = @Id";
            return await _connection.ExecuteAsync(sql, obj);
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
            var type = typeof(T);
            var sql = $"DELETE FROM {type.Name}";
            return await _connection.ExecuteAsync(sql);
        }

        public async Task<int> DeleteOne(T? obj = null, int? id = null)
        {
            var type = typeof(T);
            var sql = $"DELETE FROM {type.Name} WHERE Id = @Id";
            return await _connection.ExecuteAsync(sql, obj ?? (object)new { Id = id.Value });
        }
    }
}
