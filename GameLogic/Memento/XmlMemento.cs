using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameLogic.Memento
{
    public class XmlMemento : IMemento
    {
        private readonly string _connectionString;

        public void Save(Field field)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Field));

            using (FileStream fs = new FileStream(_connectionString, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, field);
            }
        }

        public async Task<Field> GetLast()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Field));

            using (FileStream fs = new FileStream(_connectionString, FileMode.OpenOrCreate))
            {
                var field = await Task.Run(() => (Field) formatter.Deserialize(fs));
                return field;
            }
        }

        public XmlMemento(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}