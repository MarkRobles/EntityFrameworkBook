using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkBook
{
    class EntitySQL
    {
        #region Consulta EntitySQL y  Object Services
        public void QueryContacts()
        {
            using (var context = new PEF())
            {
                String query = "SELECT c.FirstName,c.LastName, c.Title " +
"FROM PEF.Contact AS c " +
"WHERE c.FirstName='Robert'";

                ObjectQuery<DbDataRecord> contacts = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord record in contacts)
                {
                    Console.WriteLine("{0} {1} {2}",
                    record["Title"].ToString().Trim(),
                    record["FirstName"].ToString().Trim(),
                    record["LastName"].ToString().Trim());
                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
            #endregion


        

        }


        #region Projección de objetos con  EntitySQL y  Object Services
        public void ProjectingObjects()
        {
            using (var context = new PEF())
            {
                String query = "SELECT c, c.Address " +
                             "FROM PEF.Contact AS c " +
                             "WHERE c.FirstName='Robert'";

                ObjectQuery<DbDataRecord> contacts = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contacts)
                {
                    var contact = c[0] as Contact;
                    Console.WriteLine("{0} {1} {2}",
                    contact.Title.Trim(),
                    contact.FirstName.Trim(),
                    contact.LastName);
                    foreach (var a in contact.Address)
                    {
                        Console.WriteLine(" {0}, {1}",
                        a.Street1.Trim(), a.City);
                    }
                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion


        #region Projección de objetos con  EntitySQL y  Query Builder Methods
        public void ProjectingWithQueryBuilderMethods()
        {
            //using (var context = new PEF())
            //{
            //    ObjectQuery<DbDataRecord> contacts = context.Contact
            //                 //        .Where("it.FirstName='Robert'")
            //                          //.Select("it.Title, it.FirstName, it.LastName");

            //    foreach (DbDataRecord c in contacts)
            //    {
            //        var contact = c[0] as Contact;
            //        Console.WriteLine("{0} {1} {2}",
            //        contact.Title.Trim(),
            //        contact.FirstName.Trim(),
            //        contact.LastName);
            //        foreach (var a in contact.Address)
            //        {
            //            Console.WriteLine(" {0}, {1}",
            //            a.Street1.Trim(), a.City);
            //        }
            //    }

            //    Console.Write("Press Enter...");
            //    Console.ReadLine();
            //}
        }
        #endregion

        #region Projecting into an EntityRef with Entity SQLs
        public void ProjectingEntityReference()
        {
            using (var context = new PEF())
            {
                String query = "SELECT a,a.Contact FROM PEF.Address AS a WHERE a.CountryRegion = 'UK'";

                ObjectQuery<DbDataRecord> contacts = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contacts)
                {
                    var contact = c[1] as Contact;
                    Console.WriteLine("{0} {1} {2}",
                    contact.Title.Trim(),
                    contact.FirstName.Trim(),
                    contact.LastName);
                    foreach (var a in contact.Address)
                    {
                        Console.WriteLine(" {0}, {1}",
                        a.Street1.Trim(), a.City);
                    }
                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion


        #region FilterSortAnEntityReference
        public void FilterSortAnEntityReference()
        {
            using (var context = new PEF())
            {
                String query = "SELECT a,a.Contact.LastName " +
                    "FROM PEF.Address AS a" +
                    " WHERE a.Contact.AddDate > DATETIME'2009-01-1 00:00' " +
                    "ORDER BY a.Contact.LastName";

                ObjectQuery<DbDataRecord> addres = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in addres)
                {
                    var a = c[0] as Address;
                    Console.WriteLine("{0} {1}",
                    a.Contact.LastName,a.Contact.FirstName);
                  
                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion


        #region Filtering and Sorting with EntityCollections
        public void FilterNavigation()
        {
            using (var context = new PEF())
            {
                String query = "Select VALUE c FROM PEF.Contact as c WHERE EXISTS(SELECT a from c.Address as a WHERE a.CountryRegion = 'UK')";

                ObjectQuery<DbDataRecord> contact = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contact)
                {
                    var contacto = c[0] as Contact;
                    Console.WriteLine("{0} {1}",
                    contacto.LastName, contacto.FirstName);

                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion

        #region Aggregating with EntityCollections
        //Using the Count aggregate function in Entity SQL
        public void CountAdresses()
        {
            using (var context = new PEF())
            {
                String query = "Select c, COUNT(Select VALUE a.AddressID FROM c.Address as a) FROM PEF.Contact as c";

                ObjectQuery<DbDataRecord> contact = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contact)
                {
                    var s = c[1];
                    var contacto = c[0] as Contact;
                    Console.WriteLine("{0} {1} {2}",
                    contacto.LastName, contacto.FirstName,s);

                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
       // Using the MAX aggregate function in Entity SQL
        public void MAX()
        {
            using (var context = new PEF())
            {
                String query = "SELECT c.LastName, MAX(SELECT VALUE a.PostalCode FROM c.Address AS a) FROM PEF.Contact AS c";

                ObjectQuery<DbDataRecord> contact = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contact)


                    Console.Write("{0}", c[1]);

            }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }

        #endregion

        #region SetOperators
        public void OperatorAnyElement()
        {
            using (var context = new PEF())
            {
                String query = "SELECT c.LastName, ANYELEMENT(SELECT VALUE max(a.PostalCode) FROM c.Address AS a) AS MaxPostal FROM PEF.Contact AS c";

                ObjectQuery<DbDataRecord> contact = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contact)
                {
                    var contacto = c[0] as Contact;
                    Console.WriteLine("{0} {1}",
                    contacto.LastName, contacto.FirstName);

                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion

        #region JOINS
        public void JOIN()
        {
            using (var context = new PEF())
            {
                String query = "SELECT c.Title,oa.FirstName, oa.LastName, oa.Street1, oa.City, oa.StateProvince" +
                    " FROM PEF.Contact as c " +
                    "JOIN PEF.vOfficeAddresses as oa ON c.ContactID = oa.ContactID";

                ObjectQuery<DbDataRecord> contact = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

                foreach (DbDataRecord c in contact)
                {

                    var contacto = c[0] as Contact;
                    Console.WriteLine("{0} {1}",
                    c["Title"].ToString().Trim(),c["LastName"].ToString().Trim() );


                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion

        #region ShapingData
        public void ShapingData()
        {
            using (var context = new PEF())
            {
                String query = "SELECT a,a.Contact FROM PEF.Address AS a WHERE a.CountryRegion = 'Canada'";

                ObjectQuery<DbDataRecord> addresses = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<DbDataRecord>(query);

            

            
                    foreach (DbDataRecord item in addresses)
{
                    var con = (Contact)item["Contact"]; //cast to Contact type
                            Console.WriteLine("LastName: {0} #Addresses: {1}",
                            con.LastName.Trim(), con.Address.Count());
                            foreach (Address a in con.Address)
                            {
                                Console.WriteLine("....." + a.City);
                            }
                            Console.WriteLine();
                        }

                        Console.Write("Press Enter...");
                Console.ReadLine();
            }
        }
        #endregion


    }
}