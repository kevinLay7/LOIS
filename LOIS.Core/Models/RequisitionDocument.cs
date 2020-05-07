using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Models
{
    public class RequisitionDocument
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Index]
        public int RequisitionId { get; set; }

        public bool markedDeleted { get; set; }

        public DateTime? markedDeletedDate { get; set; }

        public string markedDeletedUser { get; set; }

        public byte[] Document { get; set; }
        
    }
}
