using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlesAzureSearch.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class RentingStats
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Code { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public string WeeklyRentPaidByHousehold { get; set; }
        [IsFilterable, IsSortable, IsFacetable]
        public string HouseholdsInRentedOccupiedPrivateDwellings { get; set; }
    }
}
