using PAW_Project.Models;

namespace PAW_Project.Services
{
    public interface IStudentService
    {
        Task EnrollStudentAsync(string cnp, string nume, string prenume, int courseId);
    }
}