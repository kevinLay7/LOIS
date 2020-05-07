using AutoMapper;
using LOIS.Core.Models;
using LOIS.DAL;

namespace LOIS.BLL.services
{
    public  class mapperConfig
    {
        public static MapperConfiguration CreateConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Requisition, DAL.Lois.Requisition>().ReverseMap();

                cfg.CreateMap<DAL.Lois.Patient, Patient>().ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.Dob)).ReverseMap();

                cfg.CreateMap<DAL.Lois.RequisitionDocument, RequisitionDocument>().ReverseMap();

                cfg.CreateMap<LOIS.DAL.Prolis.Requisition, Core.Models.Requisition>()
                    .ForMember(d => d.RequisitionNo, o => o.MapFrom(s => s.RequisitionNo))
                    .ForMember(d => d.AccessionDate, o => o.MapFrom(s => s.AccessionDate))
                    .ForMember(d => d.CollectedDate, o => o.MapFrom(s => s.DrawnDate))
                    .ForMember(d => d.EmrNo, o => o.MapFrom(s => s.EMRNo))
                    .ForMember(d => d.PatientId, o => o.MapFrom(s => s.Patient_ID))
                    .ForMember(d => d.PrimePayerId, o => o.MapFrom(s => s.PrimePayer_ID))
                    .ForMember(d => d.ReceivedDate, o => o.MapFrom(s => s.ReceivedTime))
                    .ForMember(d => d.SecondaryPayerId, o => o.MapFrom(s => s.SecondPayer_ID))
                    .ForMember(d => d.SpecimineType, o => o.MapFrom(s => s.SpecimenType))
                    .ForMember(d => d.ProviderId, o => o.MapFrom(s => s.OrderingProvider_ID))
                    .ForMember(d => d.ScannedDate, o => o.Ignore())
                    .ForMember(d => d.Patient, o => o.Ignore())
                    .ForMember(d => d.Documents, o => o.Ignore())
                    .ForMember(d => d.ProviderName, o => o.Ignore())
                    .ReverseMap();

                cfg.CreateMap<DAL.Prolis.Patient, Core.Models.Patient>()
                    .ForMember(d => d.PatientId, o => o.MapFrom(s => s.ID))
                    .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                    .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                    .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DOB))
                    .ForMember(d => d.SSN, o => o.ResolveUsing(s =>
                        {
                            int parsed;
                            int.TryParse(s.SSN, out parsed);
                            return parsed;
                        }))
                    .ForMember(d => d.Address, o => o.Ignore())
                    .ReverseMap();

                cfg.CreateMap<DAL.Lois.Authentication, User>().ReverseMap();

            });
        }
    }
}
