using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkBook
{
    class EntityClient
    {
        //Querying with EntityClient  to return Streamed Data
        public void QueryContacts()
        {
            using (EntityConnection conn = new EntityConnection("name=PEF"))
            {
                conn.Open();
                var queryString = "SELECT VALUE c " +
                "FROM PEF.Contact AS c " +
                "WHERE c.FirstName = 'Robert'";

                EntityCommand cmd = conn.CreateCommand();
                cmd.CommandText = queryString;

                using (EntityDataReader rdr =
                    cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess |
                                           System.Data.CommandBehavior.CloseConnection))
                {
                    while (rdr.Read())
                    {
                        var firsName = rdr.GetString(1);
                        var lastName = rdr.GetString(2);
                        var tittle = rdr.GetString(3);
                        Console.WriteLine("{0} {1} {2}", tittle.Trim(), firsName.Trim(), lastName);
                    }
                }
                conn.Close();
                Console.Write("Presiona Enter");
                Console.ReadLine();

            }
        }

    }
}
