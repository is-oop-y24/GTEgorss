namespace Isu.Entities
{
    public abstract class GroupName
    {
        public string Name { get; protected set; }

        public abstract string GetCourseId();
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                GroupName courseNumber = (GroupName)obj;
                return Name == courseNumber.Name;
            }
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}