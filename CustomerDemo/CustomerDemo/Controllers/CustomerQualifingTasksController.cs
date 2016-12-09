using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerDemo.DomainModel;
using CustomerDemo.Hypermedia;

namespace CustomerDemo.Controllers
{
    public class CustomerQualifingTasksController : ApiController
    {
        [Route("api/CustomerQualifingTasks/{id}", Name = RouteNames.CUSTOMERS_GETCUSTOMERQUALIFINGTASK)]
        public Siren Get(int id)
        {
            //ToDo Get Task
            if (true)
            {
                var siren = new Siren();
                siren.Class.Add("qualifingTask");

                siren.Links.Add(new Link
                {
                    Relation = new List<string>() { "self" },
                    Href = Url.Link(RouteNames.CUSTOMERS_GETCUSTOMERQUALIFINGTASK, new { id = id })
                });

                return siren;
            }
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound);
            msg.Content = new StringContent($"Kein Task mit der Id {id} vorhanden.");
            msg.ReasonPhrase = "No Task";
            throw new HttpResponseException(msg);
        }



        [HttpPost]
        [Route("api/CustomerQualifingTasks/{key:int}", Name = RouteNames.CUSTOMERS_CREATEQUALIFYTASK)]
        public HttpResponseMessage Qualifiy(int key)
        {
            Customer customer;
            if (CustomerRepository.TryGet(key, out customer))
            {
                //ToDO create Task
                customer.QualifingState = QualifingState.IsInProgress;
                var message = new HttpResponseMessage(HttpStatusCode.Created);
                message.Headers.Location = new System.Uri(Url.Link(RouteNames.CUSTOMERS_GETCUSTOMERQUALIFINGTASK, new { id = key }));
                return message;
            }
            //Ups. Customer nicht vorhanden. 404 zurück
            var msg = new HttpResponseMessage(HttpStatusCode.NotFound);
            msg.Content = new StringContent($"Kein Kunde mit der Id {key} vorhanden.");
            msg.ReasonPhrase = "No Customer";
            throw new HttpResponseException(msg);
        }
    }
}