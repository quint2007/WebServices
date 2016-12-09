using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace CustomerDemo.Hypermedia
{
    public class SirenFormatter : BufferedMediaTypeFormatter
    {
        public SirenFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.siren+json"));
        }
        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Siren);
        }
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var s = value as Siren;
            if (s == null)
            {
                base.WriteToStream(type, value, writeStream, content);
                return;
            }

            var sb = new StringBuilder();
            sb.Append("{");
            SerializeClass(sb, s.Class);
            sb.Append(",");
            SerializeProperties(sb, s.Properties);
            sb.Append(",");
            SerializeEntities(sb, s.Entities);
            sb.Append(",");
            SerializeActions(sb, s.Actions);
            sb.Append(",");
            SerializeLinks(sb, s.Links);
            sb.Append("}");

            byte[] buf = Encoding.UTF8.GetBytes(sb.ToString());
            writeStream.Write(buf, 0, buf.Length);

        }

        private void SerializeLinks(StringBuilder sb, List<Link> links)
        {
            sb.Append("\"links\":[");
            for (int i = 0; i < links.Count; i++)
            {
                var link = links[i];
                sb.Append("{");
                sb.Append("\"rel\"");
                sb.Append(":");

                SerializeRelation(sb, link.Relation);
                sb.Append(",");
                sb.Append("\"href\":\"").Append(link.Href).Append("\"");
                if (i < links.Count - 1)
                {
                    sb.Append(",");
                }
                sb.Append("}");
            }
            sb.Append("]");
        }

        private static void SerializeRelation(StringBuilder sb, List<string> relation)
        {
            sb.Append("[");
            for (int j = 0; j < relation.Count; j++)
            {
                var rel = relation[j];
                sb.Append("\"");
                sb.Append(rel);
                sb.Append("\"");
                if (j < relation.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
        }

        private void SerializeActions(StringBuilder sb, List<Action> actions)
        {
            sb.Append("\"actions\":[");
            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                sb.Append("{");
                sb.Append("\"name\":\"").Append(action.Name).Append("\",");
                sb.Append("\"title\":\"").Append(action.Title).Append("\",");
                sb.Append("\"method\":\"").Append(action.Method).Append("\",");
                sb.Append("\"href\":\"").Append(action.Href).Append("\",");
                sb.Append("\"type\":\"").Append(action.Type).Append("\",");
                sb.Append("\"fields\":").Append(Newtonsoft.Json.JsonConvert.SerializeObject(action.Fields));

                sb.Append("}");
                if (i < actions.Count - 1)
                {
                    sb.Append(",");
                }
                
            }
            sb.Append("]");
        }

        private void SerializeEntities(StringBuilder sb, List<Entity> entities)
        {
            sb.Append("\"entities\":[");
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                sb.Append("{");
                if (entity is EmbeddedRepresentation)
                {
                    SerializeEmbededEntity(sb, entity as EmbeddedRepresentation);
                }
                else if (entity is EmbeddedLink)
                {
                    SerializeEmbededLink(sb, entity as EmbeddedLink);
                }
                sb.Append("}");
                if (i < entities.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
        }

        private void SerializeEmbededLink(StringBuilder sb, EmbeddedLink entity)
        {
            throw new NotImplementedException();
        }

        private void SerializeEmbededEntity(StringBuilder sb, EmbeddedRepresentation entity)
        {
            SerializeClass(sb, entity.Class);
            sb.Append(",");
            sb.Append("\"rel\"");
            sb.Append(":");

            SerializeRelation(sb, entity.Relation);
            sb.Append(",");
            SerializeProperties(sb, entity.Properties);
            sb.Append(",");
            SerializeLinks(sb, entity.Links);
        }

        private void SerializeProperties(StringBuilder sb, List<Property> properties)
        {
            sb.Append("\"properties\":{");
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                sb.Append("\"");
                sb.Append(property.Name);
                sb.Append("\"");
                sb.Append(":");
                var serializedValue = Newtonsoft.Json.JsonConvert.SerializeObject(property.Value);
                sb.Append(serializedValue);
                if (i < properties.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");
        }

        private void SerializeClass(StringBuilder sb, List<string> classes)
        {
            sb.Append("\"class\":[");
            for (int i = 0; i < classes.Count; i++)
            {
                sb.Append("\"").Append(classes[i]).Append("\"");
                if (i < classes.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
        }
    }
}