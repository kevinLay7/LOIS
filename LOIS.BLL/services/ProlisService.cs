using LOIS.Core.Interfaces;
using System;
using System.Linq;
using AutoMapper;
using LOIS.Core.Exceptions;
using LOIS.DAL.Prolis;
using Patient = LOIS.Core.Models.Patient;

namespace LOIS.BLL.services
{
    /// <summary>
    /// Service responsible for getting information from the Prolis database
    /// </summary>
    public class ProlisService : ILisService
    {
        private readonly IMapper _mapper;

        public ProlisService()
        {
            _mapper = mapperConfig.CreateConfig().CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="LOIS.Core.Exceptions.ProlisException"></exception>
        /// <returns></returns>
        public LOIS.Core.Models.Requisition GetRequisitionById(int id)
        {
            try
            {
                using (var db = new LabaseEntities())
                {
                    var req = db.Requisitions.FirstOrDefault(x => x.ID == id);

                    if (req == null)
                    {
                        throw new LOIS.Core.Exceptions.ProlisException("Unable to locate Requisition: " + id);
                    }
                    else
                    {
                        var output = _mapper.Map<Core.Models.Requisition>(req);
                        if (output.CollectedDate == null)
                        {
                            //Get the specimine
                            var specimine = db.Specimens.FirstOrDefault(x => x.Accession_ID == output.RequisitionNo);

                            if (specimine != null)
                            {
                                output.CollectedDate = specimine.SourceDate;
                            }
                        }

                        return output;
                    }
                }
            }
            catch (AutoMapperMappingException e)
            {
                throw new ProlisException("Unable to convert Prolis entity to model", e);
            }
            catch (Exception e)
            {
                throw new LOIS.Core.Exceptions.ProlisException("Unable to contact the datatbase", e.InnerException);
            }
        }

        public Patient GetPatientById(int id)
        {
            try
            {
                using (var db = new LabaseEntities())
                {
                    var patient = db.Patients.FirstOrDefault(x => x.ID == id);

                    if(patient == null)
                        throw new ProlisException("Unable to location Patient: " + id);

                    var output = _mapper.Map<Patient>(patient);
                    return output;
                }
            }
            catch (Exception e)
            {
                throw new Core.Exceptions.ProlisException("Error in ProlisService", e);
            }
        }
    }
}
