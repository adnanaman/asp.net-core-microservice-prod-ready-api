namespace EngMonarchApi.Models
{
    /// <summary>
    /// Represents a single engmonarch.
    /// </summary>
    public class EngMonarch
    {
        /// <summary>
        /// Gets or sets the engmonarch id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the years.
        /// </summary>
        public string Years { get; set; }

        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the EngMonarch House.
        /// </summary>
        public string House { get; set; }        
    }
}
