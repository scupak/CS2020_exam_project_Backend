using System.Collections.Generic;

namespace Core.Entities.Entities.Filter
{
    /// <summary>
    /// A list of filtered items that includes information about the total number of objects
    /// and the filter used to filter them
    /// </summary>
    /// <remarks>
    /// This list is a generic and therefore can be used with any time of object 
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class FilteredList<T>
    {
        /// <summary>
        /// Describes which filter was used to filter the list
        /// </summary>
        public Entities.Filter.Filter FilterUsed { get; set; }

        /// <summary>
        /// The total number of unfiltered objects 
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The generic list of objects
        /// </summary>
        public List<T> List { get; set; }

    }
}