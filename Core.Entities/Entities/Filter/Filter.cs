using System;

namespace Core.Entities.Entities.Filter
{
    /// <summary>
    /// A class that is used to store filtering information. 
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Describes the direction in which the objects should be ordered
        /// can be ASC for ascending or DSC for descending
        /// </summary>
        public string OrderDirection { get; set; }

        /// <summary>
        /// Describes the order in witch the objects are listed
        /// </summary>
        public string OrderProperty { get; set; }

        /// <summary>
        /// describes the value of the field on witch the search should be performed
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Describes which field of the object the search should be based on 
        /// </summary>
        public string SearchField { get; set; }

        /// <summary>
        /// describes the value of the second field on witch the search should be performed
        /// </summary>
        public string SearchText2 { get; set; }

        /// <summary>
        /// Describes which field of the object the second search should be based on 
        /// </summary>
        public string SearchField2 { get; set; }

        /// <summary>
        /// Describes how many objects should be displayed per page
        /// </summary>
        public int ItemsPrPage { get; set; }

        /// <summary>
        /// Describes what page the filter is on
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Describes the date interval between which the filtered entities datetime should be.
        /// </summary>
        public DateTime OrderStartDateTime { get; set; }

        public DateTime OrderStopDateTime { get; set; }
    }
}