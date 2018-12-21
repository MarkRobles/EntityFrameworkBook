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

            
            LINQ objLinqToEntities = new LINQ();
            ObjectQuery objObjectQuery = new ObjectQuery();
            EntityClient objEntityClient = new EntityClient();
            ObjectServices objObjectServices = new ObjectServices();
            EntitySQL objEntitySL = new EntitySQL();

            //Ejemplos LINQ TO ENTITIES
            //objLinqToEntities.QueryContactsLambda();
            //objLinqToEntities.QueryContacts();
            //objLinqToEntities.Grouping();
            //objLinqToEntities.LinqToEnttiesInclude();


            //Ejemplos ObjectQuery
            //objObjectQuery.QueryContacts();
            //objObjectQuery.QueryContacts2();
            //objObjectQuery.QueryContactsEntityConecction();
            //objObjectQuery.NativeSQL();


            //Ejemplos Entity Client
           objEntityClient.QueryContacts();


            //Ejemplos ObjectServices
            //objObjectServices.QueryContacts();









        }

      
 


       

       
    }
}
