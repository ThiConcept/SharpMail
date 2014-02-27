using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Tibo.fr.SharpMail.Util
{
    public class XmlDataRecorder<T>
    {
        private XmlSerializer serializer = new XmlSerializer(typeof(T));


        public void StoreData(string name, T data)
        {
            string settingsFile = GetSettingsFile(name);
            Stream fileStream = File.Create(settingsFile);
            serializer.Serialize(fileStream, data);
            fileStream.Close();
        }

        public T LoadData(string name)
        {
            string settingsFile = GetSettingsFile( name);
            Stream fileStream;
            if (File.Exists(settingsFile))
            {
                fileStream = File.OpenRead(settingsFile);
                try
                {
                    if (fileStream.Length > 0)
                    {
                        object data = serializer.Deserialize(fileStream);
                        if (data != null)
                        {
                            if (data.GetType().IsAssignableFrom(typeof(T)))
                            {
                                return (T)data;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return Activator.CreateInstance<T>();
        }

        private static readonly string HOME_PATH = Environment.GetEnvironmentVariable("LocalAppData");
        private static readonly string ST_PATH = "/Tibo/SharpMail/";

        static XmlDataRecorder()
        {
            string fileF = HOME_PATH + "/Tibo";
            if (!Directory.Exists(fileF)) Directory.CreateDirectory(fileF);
            fileF = HOME_PATH + ST_PATH;
            if (!Directory.Exists(fileF)) Directory.CreateDirectory(fileF);
        }

        public string GetSettingsFile(string namePart)
        {
            return HOME_PATH + ST_PATH + namePart + ".xml";
        }  
    }
}
