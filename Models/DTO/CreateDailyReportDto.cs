using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp.Api.Models.DTO
{
    public class CreateDailyReportDto
    {
        public int UserId { get; set; }
        public DateTime DailyReportDate { get; set; }
    }
}
