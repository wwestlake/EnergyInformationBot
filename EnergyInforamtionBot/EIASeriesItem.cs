using System.ComponentModel.DataAnnotations;

namespace EnergyInforamtionBot
{
    /// <summary>
    /// Data entity for EIA Series
    /// </summary>
    public class EIASeriesItem
    {
        /// <summary>
        /// The Period of data for the entry is also the primary key
        /// </summary>
        [Key]
        public DateTime Period { get; set; }

        /// <summary>
        /// The price point of this entry
        /// </summary>
        public float Price { get; set; }
    }
}
