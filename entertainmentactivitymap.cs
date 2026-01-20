using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace PART2OOPPROJECTUI_DANIELMOURAD
{




    public sealed class EntertainmentActivityMap : ClassMap<EntertainmentActivity>
    {
        public EntertainmentActivityMap()
        {
            Map(m => m.DateStartTime).Name("DateStartTime");
            Map(m => m.Title).Name("Title");
            Map(m => m.Cost).Name("Cost");
            Map(m => m.MinParticipants).Name("MinParticipants");

        }
    }
}
