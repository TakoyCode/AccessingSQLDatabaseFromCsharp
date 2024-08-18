using System.Data.SqlClient;

namespace AccessingSQLDatabaseFromCsharp
{
    internal class CarRepository : Repository<Car>
    {
        public CarRepository(SqlConnection connection) : base(connection)
        {
        }
    }
}
