using System.Runtime.Serialization;

namespace CalcService
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public double Value { get; set; }
    }
}