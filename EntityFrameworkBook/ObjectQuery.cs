using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkBook
{
    class ObjectQuery
    {


        #region Consulta con ObjectQuery class
        public  void QueryContacts()
        {
            using (var context = new PEF())
            {
                //using System.Data.Objects;
                //ObjectSet<Contact> contacts = context.Contacts;
                var contacts = context.Contact;
                foreach (var contact in contacts)
                {
                    Console.WriteLine("{0} {1}",
                    contact.FirstName.Trim(),
                    contact.LastName);
                }
            }
            Console.Write("Press Enter...");
            Console.ReadLine();
        }
        #endregion

        #region Consulta con ObjectQuery class2
        public  void QueryContacts2()
        {

            using (var context = new PEF())
            {

                //Querying with Object Services and Entity SQL


                var sqlString = "SELECT VALUE c " +
                "FROM PEF.Contact AS c " +
                "WHERE c.FirstName = 'Robert'";

                var objctx = (context as IObjectContextAdapter).ObjectContext;

                ObjectQuery<Contact> contact = objctx.CreateQuery<Contact>(sqlString);
                Contact newContact = contact.First<Contact>();
                Console.WriteLine("{0} {1}",
                   newContact.FirstName.Trim(),
                   newContact.LastName);
                Console.Write("Press Enter...");
                Console.ReadLine();

            }
        }
        #endregion

        #region Consulta con ObjectQuery y entityConecction
        public  void QueryContactsEntityConecction()
        {

            using (var con = new EntityConnection("name=PEF"))
            {
                con.Open();
                EntityCommand cmd = con.CreateCommand();

                cmd.CommandText = "SELECT VALUE c " +
                "FROM PEF.Contact AS c " +
                "WHERE c.FirstName = 'Robert'";

                Dictionary<int, string> dict = new Dictionary<int, string>();
                using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
                {
                    while (rdr.Read())
                    {
                        int a = rdr.GetInt32(0);
                        var b = rdr.GetString(1);
                        dict.Add(a, b);
                        Console.WriteLine("{0} {1}",
               a,
               b);
                    }
                }
                Console.Write("Press Enter...");
                Console.ReadLine();



            }
            #endregion
        }

        #region Consulta con ObjectQuery y entityConecction
        public  void NativeSQL()
        {

            using (var context = new PEF())
            {

                var contact = context.Contact.SqlQuery("SELECT * FROM Contact  WHERE  FirstName = 'Robert'").FirstOrDefault<Contact>();


                Console.WriteLine("{0} {1}",
                contact.FirstName.Trim(),
                contact.LastName);


                Console.Write("Press Enter...");
                Console.ReadLine();



            }



        }
        #endregion

    }
}
