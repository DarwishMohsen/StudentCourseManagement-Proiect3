namespace PAW_Project.Models
{
    public class Studies
    {

        public string SCNP { get; set; }
        public Student Student { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }


        public float Grade { get; set; }
    }
}