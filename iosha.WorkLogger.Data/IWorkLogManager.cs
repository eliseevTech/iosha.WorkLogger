using System;
using System.Collections.Generic;
using System.Text;

namespace iosha.WorkLogger.Data
{
    public interface IWorkLogManager
    {
        void SaveLog(WorkLog workLog);

        WorkLog GetLogs(GetLogsRequest getLogsRequest);
    }
}
