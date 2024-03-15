using Explorer.BuildingBlocks.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class Bundle : Entity
    {
        public string Name { get; init; }
        
        public string Status { get; private set; }
        public ICollection<Tour>? Tours { get; set; }   

        public Bundle()
        {
            Tours = new List<Tour>();
        }

        public Bundle(long id, string name, string status)
        {
            Id = id;
            Name = name;
            
            Status = status;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Name)) throw new ArgumentException("Invalid Name");
           
            if (string.IsNullOrEmpty(Status)) throw new ArgumentException("Invalid status");

        }
        public Bundle Publish(Bundle bundle)
        {
            int count = 0;
            foreach (var item in bundle.Tours)
            {
                if (item.Status == Domain.Enum.TourStatus.PUBLISHED)
                    count++;
            }
            if (count >= 2)
            {
                bundle.Status = "PUBLISHED";
            }
            return bundle;
        }

        public Bundle Archive(Bundle bundle)
        {
            if (bundle.Status == "PUBLISHED")
                bundle.Status = "ARCHIVED";

            return bundle;
        }
    }
}
