using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerDemo.DomainModel;
using CustomerDemo.Hypermedia;

namespace CustomerDemo.Controllers
{
    public class FavoriteCustomersController : ApiController
    {
        [Route("api/FavoriteCustomers", Name = RouteNames.CUSTOMERS_GETALLFAVORITECUSTOMERS)]
        public Siren Get()
        {
            var siren = new Siren();
            siren.Class.Add("customers");
            siren.Class.Add("collection");
            foreach (var customer in CustomerRepository.GetAll())
            {
                if (customer.IsFavorite)
                {
                    var entity = new EmbeddedRepresentation();
                    entity.Class.Add("customer");
                    entity.Properties.Add(new Property {Name = "Name", Value = customer.Name});
                    entity.Properties.Add(new Property {Name = "Id", Value = customer.Id});
                    entity.Properties.Add(new Property {Name = "LastContact", Value = customer.LastContact});
                    entity.Relation.Add("CustomerOverview");
                    entity.Links.Add(new Link
                    {
                        Relation = new List<string>() {"self"},
                        Href = Url.Link(RouteNames.CUSTOMERS_GETCUSTOMER, new {id = customer.Id})
                    });
                    siren.Entities.Add(entity);
                }
            }
            siren.Links.Add(new Link
            {
                Relation = new List<string>() { "self" },
                Href = Url.Link(RouteNames.CUSTOMERS_GETALLCUSTOMERS, null)
            });

            return siren;
        }       

        [HttpPost]
        [Route("api/FavoriteCustomers/{key:int}", Name = RouteNames.CUSTOMERS_MARKASFAVORITE)]
        public void MarkAsFavorit(int key)
        {
            Customer customer;
            if (CustomerRepository.TryGet(key, out customer))
            {
                customer.IsFavorite = true;
                return;
            }
            //Ups. Customer nicht vorhanden. 404 zurück
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound);
            msg.Content = new StringContent($"Kein Kunde mit der Id {key} vorhanden.");
            msg.ReasonPhrase = "No Customer";
            throw new HttpResponseException(msg);
        }

        [HttpDelete]
        [Route("api/FavoriteCustomers/{key:int}", Name = RouteNames.CUSTOMERS_UNMARKASFAVORITE)]
        public void UnmarkAsFavorit(int key)
        {
            Customer customer;
            if (CustomerRepository.TryGet(key, out customer))
            {
                customer.IsFavorite = false;
                return;
            }
            //Ups. Customer nicht vorhanden. 404 zurück
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound);
            msg.Content = new StringContent($"Kein Kunde mit der Id {key} vorhanden.");
            msg.ReasonPhrase = "No Customer";
            throw new HttpResponseException(msg);
        }
    }
}