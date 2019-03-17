namespace CsvOutput.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_persons
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string personId { get; set; }

        [Required]
        [StringLength(100)]
        public string personName { get; set; }

        public string mailAddress { get; set; }

        [StringLength(20)]
        public string phoneNumber { get; set; }

        [StringLength(200)]
        public string detail { get; set; }

        public DateTime createdDatetime { get; set; }

        public DateTime updatedDatetime { get; set; }
    }
}
