using LOIS.Core.Models;
using System.Linq;
using LOIS.DAL.Lois;

namespace LOIS.BLL.services
{
    internal class RequisitionSearchService
    {
        public static IQueryable<DAL.Lois.Requisition> Search(RequisitionSearch searchModel, loisEntities1 context)
        {
            var reqs = context.Requisitions.AsQueryable();

            if (searchModel.RequisitionId.HasValue)
                reqs = reqs.Where(x => x.RequisitionNo == searchModel.RequisitionId);

            if (!string.IsNullOrEmpty(searchModel.EMRNo))
                reqs = reqs.Where(x => x.EmrNo.ToLower().Contains(searchModel.EMRNo));

            if (searchModel.AccessionDateStart.HasValue)
                reqs = reqs.Where(x => x.AccessionDate >= searchModel.AccessionDateStart);

            if (searchModel.AccessionDateEnd.HasValue)
                reqs = reqs.Where(x => x.AccessionDate <= searchModel.AccessionDateEnd);

            if (searchModel.CollectedDateStart.HasValue)
                reqs = reqs.Where(x => x.CollectedDate >= searchModel.CollectedDateStart);

            if (searchModel.CollectedDateEnd.HasValue)
                reqs = reqs.Where(x => x.CollectedDate <= searchModel.CollectedDateEnd);

            if (searchModel.ScannedDateStart.HasValue)
                reqs = reqs.Where(x => x.ScannedDate >= searchModel.ScannedDateStart);

            if (searchModel.ScannedDateEnd.HasValue)
                reqs = reqs.Where(x => x.ScannedDate <= searchModel.ScannedDateEnd);

            //Patient stuff
            if (searchModel.PatientId.HasValue)
                reqs = reqs.Where(x => x.PatientId == searchModel.PatientId);

            if(!string.IsNullOrEmpty(searchModel.PatientFirstName) || !string.IsNullOrEmpty(searchModel.PatientLastName) || searchModel.PatientSSN.HasValue)
            {
                //Get patient
                var patients = context.Patients.AsQueryable();

                if (!string.IsNullOrEmpty(searchModel.PatientFirstName))
                    patients = context.Patients.Where(p => p.FirstName.ToLower().Contains(searchModel.PatientFirstName));

                if (!string.IsNullOrEmpty(searchModel.PatientLastName))
                    patients = context.Patients.Where(p => p.LastName.ToLower().Contains(searchModel.PatientLastName));

                if (searchModel.PatientSSN.HasValue)
                    patients = context.Patients.Where(p => p.SSN == searchModel.PatientSSN);

                var patIds = patients.Select(x => x.PatientId).ToList();

                reqs = reqs.Where(r => r.PatientId != null && patIds.Any(p => p == r.PatientId.Value));
            }

            //Insurance
            //todo add insurance search

            return reqs;
        }

    }
}
