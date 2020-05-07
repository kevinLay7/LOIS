using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Interfaces
{
    public interface ILisService
    {
        Core.Models.Requisition GetRequisitionById(int id);

        Core.Models.Patient GetPatientById(int id);
    }
}
