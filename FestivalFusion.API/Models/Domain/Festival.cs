﻿namespace FestivalFusion.API.Modals.Domain
{
    public class Festival
    {
        public int festivalId { get; set; }
        public string FestivalName { get; set; }
        public string FestivalDescription { get; set; }
        public string Theme { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Sponsor { get; set; }
    }
}
