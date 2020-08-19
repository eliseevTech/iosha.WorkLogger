using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using iosha.WorkLogger.Data;

namespace iosha.WorkLogger.Data.XmlStorage
{
    public class WorkLogXmlManager : IWorkLogManager
    {
        private readonly string _path;

        public WorkLogXmlManager()
        {
            _path = AppDomain.CurrentDomain.BaseDirectory + "DATA.XML";
        }

        public WorkLog GetLogs(GetLogsRequest getLogsRequest)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WorkLog));
            
            try
            {
                TextReader reader = new StreamReader(_path);

                WorkLog workLog = (WorkLog)serializer.Deserialize(reader);

                reader.Close();
                return workLog;

            }
            catch
            {
                return new WorkLog()
                {
                    Day = DateTime.Now,
                    Id = 1,
                    PCRunTimeMillisecond = 0,
                    WorkTimeMillisecond = 0
                };
            }
        }

        public void SaveLog(WorkLog workLog)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WorkLog));
            // Create a new StreamWriter
             TextWriter writer = new StreamWriter(_path);

            // Serialize the file
            serializer.Serialize(writer, workLog);

            // Close the writer
            writer.Close();
        }
    }
}
