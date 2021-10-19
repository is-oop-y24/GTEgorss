namespace Isu.Entities
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

        public bool IsAssignedToOgnpGroup { get; set; }
        public OgnpGroupName OgnpGroup1 { get; set; }
        public OgnpGroupName OgnpGroup2 { get; set; }
    }
}