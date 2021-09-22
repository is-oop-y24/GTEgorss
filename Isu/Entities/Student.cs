namespace Isu.MyClasses
{
    public class Student
    {
        public Student(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; }
        public int Id { get; }
    }
}