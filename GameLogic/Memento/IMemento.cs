using System.Threading.Tasks;

namespace GameLogic.Memento
{
    public interface IMemento
    {
        void Save(Field field);
        Task<Field> GetLast();
    }
}