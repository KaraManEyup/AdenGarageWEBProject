using Core.Models;

namespace Business.Services
{
    public interface IArabaService
    {
        Araba GetById(int id);
        IEnumerable<Araba> GetAll();
        // Diğer işlemler burada tanımlanabilir (örneğin, ekleme, silme, güncelleme)
    }
}
