using System;
using AutoMapper;
using LOIS.BLL.services;
using LOIS.Core.Interfaces;
using LOIS.Core.Models;
using LOIS.DAL;

namespace LOIS.BLL.Factories
{
    public class RequisitionFactory
    {
        private static IMapper _mapper = services.mapperConfig.CreateConfig().CreateMapper();

        public static Requisition CreateRequisition(int accId, ILisService service)
        {
            //Get the information from the Prolis db
            var req = service.GetRequisitionById(accId);
            req.ScannedDate = DateTime.Now;

            // Get Patient
            if (req.PatientId.HasValue)
            {
                var pat = service.GetPatientById(req.PatientId.Value);
                if (pat != null)
                {
                    var patModel = _mapper.Map<Core.Models.Patient>(pat);
                    req.Patient = patModel;
                }
            }

            return req;
        }

    }
}
