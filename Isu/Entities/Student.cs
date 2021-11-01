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
        public GroupName OgnpGroup1 { get; set; }
        public GroupName OgnpGroup2 { get; set; }
        public bool IsAssignedToOgnpGroup => OgnpGroup1 != null && OgnpGroup2 != null;

        public void FormatOgnpGroups()
        {
            if (OgnpGroup1 == null && OgnpGroup2 != null)
            {
                OgnpGroup1 = OgnpGroup2;
                OgnpGroup2 = null;
            }
        }
    }
}