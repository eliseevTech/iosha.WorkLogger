using System;

namespace iosha.WorkLogger.Data
{
    public class WorkLog
    {
        public int Id { get; set; }

        public DateTime Day { get; set; }

        public double WorkTimeMillisecond { get; set; }

        public double PCRunTimeMillisecond { get; set; }
    }
}
