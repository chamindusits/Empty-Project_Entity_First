using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SITS.BNS.Entities.Interfaces
{
    public class CommonEntity
    {
        public int ID { get; set; }
        public string? CreatedByName { get; set; }
        [MaxLength(50)]
        public string? CreatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        [MaxLength(50)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
