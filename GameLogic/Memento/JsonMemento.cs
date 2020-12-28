using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameLogic.Memento
{
    public class JsonMemento : IMemento
    {
        private readonly string _connectionString;

        public async void Save(Field field)
        {
            using (FileStream fs = new FileStream(_connectionString, FileMode.OpenOrCreate))
            { 
                await JsonSerializer.SerializeAsync<Field>(fs, field);
            }
        }

        public async Task<Field> GetLast()
        {
            using (FileStream fs = new FileStream(_connectionString, FileMode.OpenOrCreate))
            {
                var person = await JsonSerializer.DeserializeAsync<Field>(fs);
                return person;
            }

        }

        public JsonMemento(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}