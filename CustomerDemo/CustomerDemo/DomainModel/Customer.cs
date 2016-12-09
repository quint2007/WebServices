using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace CustomerDemo.DomainModel
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastContact { get; set; }
        public bool IsFavorite { get; set; }
        public QualifingState QualifingState { get; set; }

    }

    public enum QualifingState
    {
        NotQualified,
        IsInProgress,
        Qualified,
    }
}