//Local
using Forum.Models.Entities;
using Forum.Models.Response.Report;
//Public
using System;
using System.Linq;

namespace Forum.Service.Common.Extensions
{
    public static class ReportQueries
    {
        public static Func<IQueryable<Report>, IQueryable<ReportResponseModel>> GetAllReports
        => (IQueryable<Report> reports) =>
            reports.Select(r => new ReportResponseModel
            (
                r.Sender.Username,
                r.Receiver.Username,
                r.ReportType.Name,
                r.Description
            ));
    }
}