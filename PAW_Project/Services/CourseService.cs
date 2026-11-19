using PAW_Project.Models;
using PAW_Project.Repositories;

namespace PAW_Project.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IProfessorRepository _professorRepo;
        private readonly IRoomRepository _roomRepo;

        public CourseService(ICourseRepository repo, IProfessorRepository professorRepo, IRoomRepository roomRepo)
        {
            _repo = repo;
            _professorRepo = professorRepo;
            _roomRepo = roomRepo;
        }

        public async Task<IEnumerable<Course>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync() => await _repo.GetAllWithDetailsAsync();

        public async Task<Course> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<Course> GetCourseWithDetailsAsync(int id) => await _repo.GetCourseWithDetailsAsync(id);

        public async Task CreateAsync(Course course, string[] professorIds, int? roomId)
        {
            if (professorIds != null)
            {
                foreach (var pcnp in professorIds)
                {
                    var prof = await _professorRepo.GetByIdAsync(pcnp);
                    if (prof != null) course.Professors.Add(prof);
                }
            }
            if (roomId.HasValue)
            {
                var room = await _roomRepo.GetByIdAsync(roomId.Value);
                if (room != null)
                {
                    var newSchedule = new Schedule
                    {
                        Date = DateTime.UtcNow,
                        Hour = "TBD",
                        Rooms = new List<Room> { room }
                    };
                    course.Schedules.Add(newSchedule);
                }
            }

            await _repo.AddAsync(course);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Course course, string[] professorIds, int? roomId)
        {
            var existingCourse = await _repo.GetCourseWithDetailsAsync(course.CourseID);
            if (existingCourse == null) return;

            existingCourse.Name = course.Name;
            existingCourse.Year = course.Year;
            existingCourse.Semester = course.Semester;

            existingCourse.Professors.Clear();
            if (professorIds != null)
            {
                foreach (var pcnp in professorIds)
                {
                    var prof = await _professorRepo.GetByIdAsync(pcnp);
                    if (prof != null) existingCourse.Professors.Add(prof);
                }
            }

            if (roomId.HasValue)
            {
                var room = await _roomRepo.GetByIdAsync(roomId.Value);
                if (room != null)
                {
                    if (!existingCourse.Schedules.Any())
                    {
                        existingCourse.Schedules.Add(new Schedule
                        {
                            Date = DateTime.UtcNow,
                            Hour = "TBD",
                            Rooms = new List<Room> { room }
                        });
                    }
                    else
                    {
                        var firstSched = existingCourse.Schedules.First();
                        firstSched.Rooms.Clear();
                        firstSched.Rooms.Add(room);
                    }
                }
            }
            else
            {
                if (existingCourse.Schedules.Any())
                {
                    existingCourse.Schedules.First().Rooms.Clear();
                }
            }

            _repo.Update(existingCourse);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course != null)
            {
                _repo.Delete(course);
                await _repo.SaveAsync();
            }
        }
    }
}