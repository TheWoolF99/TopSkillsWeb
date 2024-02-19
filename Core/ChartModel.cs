using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ChartModel
    {
        public DateTime CurrentDate { get; set; } = DateTime.Now;

        public int TotalAll { get
            {
                return chartElems.Select(x=>x.Total).Sum();
            } 
        } 
        public int Growth {
            get
            {
                if (chartElems.Any())
                {
                    var Today = chartElems.Where(x => x.Time.Date == CurrentDate.Date).FirstOrDefault()?.Total;
                    var Yesterday = chartElems.Where(x => x.Time.Date == CurrentDate.AddDays(-1).Date).FirstOrDefault()?.Total;
                    Today ??= 0;
                    Yesterday ??= 0;
                    var Growth = Today - Yesterday;
                    return Growth ??= 0;
                }
                return 0;
            }
        }

        public string ClassName {
            get
            {
                switch (Growth)
                {
                    case var value when value > 0:
                        return "Profit";
                    case var value when value < 0:
                        return "Decline";
                    default:
                        return "Neutral";
                }
            }
        }


        public List<ChartElem> chartElems = new List<ChartElem>();
        public int[] DataArray { get { return chartElems.Select(x => x.Total).ToArray(); } }
        public string[] DateStringArray { get { return chartElems.Select(x => x.Time.ToString("yyy-MM-dd")).ToArray(); } }
    }


    public class ChartElem
    {
        public int Total { get; set; }
        public DateTime Time { get; set; }
    }
}
