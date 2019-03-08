using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkBook
{
    class LINQ
    {


        #region Consulta con  LINQ to Entities using the method-based syntax (lambda "=>")
        public   void QueryContactsLambda()
        {

            using (var context = new PEF())
            {

                //Seleccionar toda la entidad, propiedades, campos.
                //var contacts = 
                //    context.Contact.Where(c => c.FirstName == "Robert")
                //.OrderBy((foo) => foo.LastName);

                //Projection, seleccionar propiedades, campos especificos
                var contacts =
                  context.Contact
                  .Where(c => c.FirstName == "Robert")
                  .Select(c => new { c.Title, c.LastName, c.FirstName })
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

        #region Consulta con  LINQ to Entities
        public   void QueryContacts()
        {
            using (var context = new PEF())
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

        #region Agrupar
        public   void Grouping()
        {

            using (var context = new PEF())
            {



                //var contacts = from c in context.Contact group c by c.Title into mygroup select mygroup;

                //var contacts =
                //from c in context.Contact
                //group c by c.Title into MyGroup
                //orderby MyGroup.Key
                //select new { MyTitle = MyGroup.Key, MyGroup };

                //Chaining Agregates
                //var contacts =
                //from c in context.Contact
                //group c by c.Title into MyGroup
                //orderby MyGroup.Key
                //select new
                //{
                //    MyTitle = MyGroup.Key,
                //    MyGroup,
                //    Max = MyGroup.Max(c => c.AddDate),
                //    Count = MyGroup.Count()
                //};

                //filtering on a group property
                //var contacts =
                //from c in context.Contact
                //group c by c.Title into MyGroup
                //where MyGroup.Count() > 150
                //select new
                //{
                //    MyTitle = MyGroup.Key,
                //    MyGroup,
                //    Count = MyGroup.Count()
                //};

                var contacts =
                from a in context.Address
                let c = new
                {
                    a.Contact.FirstName,
                    a.Contact.LastName,
                    a.CountryRegion
                }
                group c by c.CountryRegion into MyGroup
                where (MyGroup.Count() > 150)
                select MyGroup;

                //Example 4 - 30.Filtering related data in a query using projections
                //var contactGraphs = from c in context.Contact
                //                  select new { c, c.Address.Where(a => a.CountryRegion = "UK") };

                foreach (var contact in contacts)
                {
                    //  Console.WriteLine("{0} {1} {2} ", contact.., contact.MyGroup,contact.Count);

                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }



        }
        #endregion


        #region Agrupar
        public   void LinqToEnttiesInclude()
        {

            using (var context = new PEF())
            {

                //var contacts =
                //from c in context.Contact.Include("Address")
                //where c.LastName.StartsWith("J")
                //select c;

                //Obtener los contactos que tengan cualquiera de sus domicilios en UK
                var contacts =
                from c in context.Contact.Include("Address")
                where c.Address.Any((a) => a.CountryRegion == "UK")
                select c;

                foreach (var contact in contacts)
                {
                    Console.WriteLine("{0} {1} {2} ", contact.FirstName, contact.LastName, contact.Address.Count());

                }

                Console.Write("Press Enter...");
                Console.ReadLine();
            }



        }
        #endregion


        #region Keeping Track of Entities
        public void UpdateContact()
        {

           
                using (PEF context = new PEF())
                {
                    var contact = context.Contact.First();
                    contact.FirstName = "Julia";
                    contact.ModifiedDate = DateTime.Now;
                    context.SaveChanges();
                }

           
            
            Console.Write("Press Enter...");
            Console.ReadLine();
        }

        public void UpdateContacts()
        {


            using (PEF context = new PEF())
            {
                var contacts = context.Contact.Include("Address")
                .Where(c => c.FirstName == "Bobby").ToList();
                var contact = contacts[1];
                contact.FirstName = "Marquitos";
                contact = contacts[2];
                var address = contact.Address.ToList()[0];
                address.Street1 = "Two Main Street";
                context.SaveChanges();
            }



            Console.Write("Press Enter...");
            Console.ReadLine();
        }
        #endregion


        public void InsertAdress()
        {
            using (PEF context = new PEF())
            {

                //  Example 6 - 3.Creating a new address in memory
                var contact = context.Contact.Where(c => c.FirstName == "Robert").First();
                var address = new Address();
                address.Street1 = "TRHEE Main Street";
                address.City = "Colon";

                address.StateProvince = "VH";
                address.AddressType = "HOME";
                address.ModifiedDate = DateTime.Now;
                //join the new address to the contact
                address.Contact = contact;
                context.SaveChanges();
            }
            Console.Write("Press Enter...");
            Console.ReadLine();
        }


        public void GetContactsbyState()
        {
            using (PEF context = new PEF())
            {
                //   Example 7 - 5.Testing the function mapping
                ObjectResult<Contact> results = context.GetContacsbyState("Washington");

             List<Contact> ListaContactos =   context.GetContacsbyState("Washington").ToList();

                 foreach (var contact in ListaContactos)
                {
                    Console.WriteLine("{0} {1} ", contact.FirstName, contact.LastName);

                }
            }

            
Console.Write("Press Enter...");
            Console.ReadLine();
        }





    }
}
