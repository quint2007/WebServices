using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CustomerDemo.DomainModel;
using CustomerDemo.Hypermedia;

namespace CustomerDemo.Controllers
{
    public class CustomersController : ApiController
    {
        [Route("api/Customers", Name = RouteNames.CUSTOMERS_GETALLCUSTOMERS)]
        public Siren Get()
        {
            var siren = new Siren();
            siren.Class.Add("customers");
            siren.Class.Add("collection");
            foreach (var customer in CustomerRepository.GetAll())
            {
                var entity = new EmbeddedRepresentation();
                entity.Class.Add("customer");
                entity.Properties.Add(new Property { Name = "Name", Value = customer.Name });
                entity.Properties.Add(new Property { Name = "Id", Value = customer.Id });
                entity.Properties.Add(new Property { Name = "LastContact", Value = customer.LastContact });
                entity.Relation.Add("CustomerOverview");
                entity.Links.Add(new Link
                {
                    Relation = new List<string>() {"self"},
                    Href = Url.Link(RouteNames.CUSTOMERS_GETCUSTOMER, new {id = customer.Id})
                });
                siren.Entities.Add(entity);
            }
            siren.Links.Add(new Link
            {
                Relation = new List<string>() { "self" },
                Href = Url.Link(RouteNames.CUSTOMERS_GETALLCUSTOMERS, null)
            });

            return siren;
        }

        [Route("api/Customers/{id}", Name = RouteNames.CUSTOMERS_GETCUSTOMER)]
        public Siren Get(int id)
        {
            Customer customer;
            if (CustomerRepository.TryGet(id, out customer))
            {
                var siren = new Siren();
                siren.Class.Add("customer");
                siren.Properties.Add(new Property { Name = "Name", Value = customer.Name });
                siren.Properties.Add(new Property { Name = "Id", Value = customer.Id });
                siren.Properties.Add(new Property { Name = "LastContact", Value = customer.LastContact });
                siren.Properties.Add(new Property { Name = "IsFavorite", Value = customer.IsFavorite });
                if (!customer.IsFavorite)
                {
                    //ToDo Add Action
                    siren.Actions.Add(new Hypermedia.Action
                    {
                        Name = "MarkAsFavorite",
                        Title = "Fügt den Customer als Favorit hinzu",
                        Method = "POST",
                        Href = Url.Link(RouteNames.CUSTOMERS_MARKASFAVORITE, new { key = id })
                    });
                }
                if (customer.IsFavorite)
                {
                    siren.Actions.Add(new Hypermedia.Action
                    {
                        Name = "UnmarkAsFavorite",
                        Title = "Entfernt den Customer von den Favoriten",
                        Method = "DELETE",
                        Href = Url.Link(RouteNames.CUSTOMERS_UNMARKASFAVORITE, new  { key = id })
                    });
                }
                siren.Links.Add(new Link
                {
                    Relation = new List<string>() { "self" },
                    Href = Url.Link(RouteNames.CUSTOMERS_GETCUSTOMER, new { id = id })
                });

                return siren;
            }
            //Ups. Customer nicht vorhanden. 404 zurück
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound);
            msg.Content = new StringContent($"Kein Kunde mit der Id {id} vorhanden.");
            msg.ReasonPhrase = "No Customer";
            throw new HttpResponseException(msg);
        }
        
        
    }
}
