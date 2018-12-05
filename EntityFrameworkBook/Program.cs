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
    class Program
    {

        static void Main(string[] args)
        {
             QueryContacts();
              //QueryContactsLambda();
            //EntityCientQueryContacts();
            //QueryContactsObjectQuery2();
            // QueryContactsObjectQuery3();
           // QueryContactsObjectQuery();
           // NativeSQL();
        }

        //Querying with EntityClient  to return Streamed Data
        static void EntityCientQueryContacts()
        {
            using (EntityConnection conn = new EntityConnection("name=SampleEntities"))
            {
                conn.Open();
                var queryString = "SELECT VALUE c " +
                "FROM SampleEntities.Contact AS c " +
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

        #region Consulta con  LINQ to Entities using the method-based syntax (lambda "=>")
        private static void QueryContactsLambda()
        {

            using (var context = new SampleEntities())
            {

                //Seleccionar toda la entidad, propiedades, campos.
                //var contacts = 
                //    context.Contact.Where(c => c.FirstName == "Robert")
                //.OrderBy((foo) => foo.LastName);

                //Projection, seleccionar propiedades, campos especificos
                var contacts =
                  context.Contact
                  .Where(c => c.FirstName == "Robert")
                  .Select(c => new  {c.Title, c.LastName,c.FirstName })
              .OrderBy((foo) => foo.LastName);



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

        #region Consulta con  Entity   Framework’s Object Services
        private static void QueryContactsObjectServices()
        {
            using (var context = new SampleEntities())
            {


                var queryString = "SELECT VALUE c " +
 "FROM SampleEntities.Contact AS c " +
 "WHERE c.FirstName='Robert'";

                //ObjectQuery<Contact> contacts = context.CreateQuery<Contact>(queryString);

                //foreach (var contact in contacts)
                //{
                //    Console.WriteLine("{0} {1}",
                //    contact.FirstName.Trim(),
                //    contact.LastName);
                //}
            }
            Console.Write("Press Enter...");
            Console.ReadLine();
        }
        #endregion

        #region Consulta con  LINQ to Entities
        private static void QueryContacts()
        {
            using (var context = new SampleEntities())
            {

                //Retornar un objetos, entidades enteras donde el nombre sea Robert
                //var contacts = from c in context.Contact
                //               where c.FirstName == "Robert"
                //               select c;

                //Projection, obtener propiedades o campos especificos
                //var contacts = from c in context.Contact
                //               where c.FirstName == "Robert"
                //               select new { c.Title, c.FirstName, c.LastName };


                //foreach (var contact in contacts)
                //{

                //    Console.WriteLine("{0} {1} {2}",
                //    contact.Title.Trim(),
                //    contact.FirstName.Trim(),
                //    contact.LastName.Trim()
                //    );
                //}


                //Projection, tipos anonimos
                //            var contacts =
                //from c in context.Contact
                //where c.FirstName == "Robert"
                //let foo = new
                //{
                //    ContactName = new { c.Title, c.LastName, c.FirstName },
                //    c.Address
                //}
                //orderby foo.ContactName.LastName
                //select foo;

                //            foreach (var contact in contacts)
                //            {
                //                var name = contact.ContactName;
                //                Console.WriteLine("{0} {1} {2}: # Addresses {3}",
                //                name.Title.Trim(), name.FirstName.Trim(),
                //                name.LastName.Trim(), contact.Address.Count());
                //            }


                //Projecting into an EntityRef with LINQ to Entities
                //var addresses = from a in context.Address
                //                where a.CountryRegion == "UK"
                //                select new { a, a.Contact };


                //var addresses = from a in context.Address
                //                where a.Contact.AddDate > new System.DateTime(2009,1,1)
                //                orderby a.Contact.LastName
                //                select new
                //                {
                //                   Address = a, a.Contact
                //                };

                ////Accessing the properties of an anonymous type
                //foreach (var address in addresses)
                //{
                //    Console.WriteLine("{0} {1} {2}",
                //    address.Contact.LastName, address.Address.Street1,
                //    address.Address.City);
                //}



                //var contacts = from c in context.Contact
                //               select new { c, Foos = c.Address};

                //foreach (var contact in contacts)
                //{
                //    Console.WriteLine("{0}: Address Count {1} ",
                //    contact.c.LastName.Trim(), contact.Foos.Count);
                //    foreach (var foo in contact.Foos)
                //    {
                //        Console.WriteLine(" City= {0}", foo.City);
                //    }
                //}


                //shaped results
                //var contacts = from c in context.Contact
                //               select new
                //               {
                //                   c.FirstName,
                //                   c.LastName,
                //                   StreetsCities = from a in c.Address
                //                                   select new { a.Street1, a.City }
                //               };

                //foreach (var contact in contacts)
                //{
                //    Console.WriteLine("{0} {1} ",
                //    contact.LastName.Trim(), contact.FirstName);
                //    foreach (var foo in contact.StreetsCities)
                //    {
                //        Console.WriteLine("{0} {1}",foo.Street1 ,foo.City);
                //    }
                //}

                //flatened results, turn the query, look more simple, but the contacts data are duplicated when they have multiple adresses
                var contacts =
    from a in context.Address
    orderby a.Contact.LastName
    select new { a.Contact.LastName, a.Contact.FirstName, a.Street1, a.City };


                foreach (var contact in contacts)
                {
                    Console.WriteLine("{0}{1}{2}{3}", contact.LastName, contact.FirstName, contact.Street1, contact.City);
                }

            }
            Console.Write("Press Enter...");
            Console.ReadLine();
        }
        #endregion

        #region Consulta con ObjectQuery class
        private static void QueryContactsObjectQuery()
        {
            using (var context = new SampleEntities())
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
        private static void QueryContactsObjectQuery2()
        {

            using (var context = new SampleEntities())
            {

                //Querying with Object Services and Entity SQL


                var sqlString = "SELECT VALUE c " +
                "FROM SampleEntities.Contact AS c " +
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
        private static void QueryContactsObjectQuery3()
        {

            using (var con = new EntityConnection("name=SampleEntities"))
            {
                con.Open();
                EntityCommand cmd = con.CreateCommand();

                cmd.CommandText = "SELECT VALUE c " +
                "FROM SampleEntities.Contact AS c " +
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
        private static void NativeSQL()
        {

            using (var context = new SampleEntities())
            {

                var contact = context.Contact.SqlQuery("SELECT * FROM Contact  WHERE  FirstName = 'Robert'").FirstOrDefault<Contact>();


                Console.WriteLine("{0} {1}",
                contact.FirstName.Trim(),
                contact.LastName);


                Console.Write("Press Enter...");
                Console.ReadLine();



            }
            #endregion
        }
    }
}
