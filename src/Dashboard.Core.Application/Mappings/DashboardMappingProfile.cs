using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Application.DTOs;
using AutoMapper;
using Dashboard.Core.Domain.Entities;

namespace Dashboard.Core.Application.Mappings
{
    public class DashboardMappingProfile : Profile
    {
        public DashboardMappingProfile()
        {
            CreateMap<EmployeeEvent, EmployeeEventDto>()
                .ForMember(d => d.CameraName, opt => opt.MapFrom(s => s.Camera.Name))
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name));

            CreateMap<EmployeeAlert, EmployeeAlertDto>()
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name));

            CreateMap<EmployeeCurrentStatus, EmployeeCurrentStatusDto>()
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name))
                .ForMember(d => d.AwayMinutes, opt => opt.MapFrom(s =>
                    s.AwayStartTime.HasValue
                        ? (int)(DateTime.UtcNow - s.AwayStartTime.Value).TotalMinutes
                        : (int?)null));

            CreateMap<DashboardCamera, DashboardCameraDto>()
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name));

            CreateMap<DashboardLocation, DashboardLocationDto>();

            CreateMap<FloorPlan, FloorPlanDto>()
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name));

            CreateMap<WatchlistConfiguration, WatchlistConfigurationDto>()
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name));

            CreateMap<DailyStatistic, DailyStatisticDto>()
                .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location.Name));
        }
    }
}
