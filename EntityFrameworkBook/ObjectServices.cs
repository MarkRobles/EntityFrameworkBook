using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkBook
{
    class ObjectServices
    {

        #region Consulta con  Entity   Framework’s Object Services
       public void QueryContacts()
        {
            using (var context = new PEF())
            {


                var queryString = "SELECT VALUE c " +
 "FROM PEF.Contact AS c " +
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
    }
}
