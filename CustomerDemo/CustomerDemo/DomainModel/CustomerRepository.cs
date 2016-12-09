using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerDemo.DomainModel
{
    public static class CustomerRepository
    {
        private static Dictionary<int, Customer> s_Repository = new Dictionary<int, Customer>();

        static CustomerRepository()
        {
            var customer1 = new Customer
            {
                Id = 1,
                Name = "Conrad Müller",
                LastContact = new DateTime(2015, 3, 24, 11, 34, 21)
            };
            s_Repository.Add(customer1.Id, customer1);
            var customer2 = new Customer
            {
                Id = 2,
                Name = "Peter Aurin",
                LastContact = new DateTime(1998, 9, 21, 15, 44, 21),
            };
            s_Repository.Add(customer2.Id, customer2);
            var customer3 = new Customer
            {
                Id = 3,
                Name = "Uwe Baumann",
                LastContact = new DateTime(2016, 1, 2, 9, 30, 41)
            };
            s_Repository.Add(customer3.Id, customer3);
        }

        public static List<Customer> GetAll()
        {
            return s_Repository.Values.ToList();
        }

        public static bool TryGet(int id, out Customer customer)
        {
            return s_Repository.TryGetValue(id, out customer);
        }

        public static void Add(Customer customer)
        {
            s_Repository.Add(customer.Id, customer);
        }

        public static bool Exists(int customerId)
        {
            return s_Repository.ContainsKey(customerId);
        }

        public static void Update(Customer customer)
        {
            if (!s_Repository.ContainsKey(customer.Id))
            {
                throw new Exception("Customer does not exists");
            }
            s_Repository[customer.Id] = customer;
        }

        public static void Delete(int id)
        {
            s_Repository.Remove(id);
        }
    }
}