using System.ComponentModel.DataAnnotations;

namespace EngMonarchApi.Models
{
    /// <summary>
    /// Represents a single engmonarch data to be saved.
    /// </summary>
    public class EngMonarchInput
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the release year.
        /// </summary>
        public string Years { get; set; }
        
        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the EngMonarch Geek URL.
        /// </summary>
        [Required]
        public string House { get; set; }
        
        /// <summary>
        /// Copy data to <see cref="EngMonarch"/> instance.
        /// </summary>
        /// <param name="engmonarch">EngMonarch to copy the input data to.</param>
        public void MapToEngMonarch(EngMonarch engmonarch)
        {
            engmonarch.Name = Name;
            engmonarch.Years = Years;
            engmonarch.Country = Country;
            engmonarch.House = House;            
        }
    }
}
