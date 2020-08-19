using iosha.WorkLogger.Data;
using System.Threading.Tasks;

namespace iosha.WorkLogger.CloudSender
{
    public interface ICloudSender
    {
        string Send(WorkLog workLog);
    }
}