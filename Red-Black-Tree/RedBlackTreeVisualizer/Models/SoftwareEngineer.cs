namespace RedBlackTreeVisualizer.Models
{
    public class SoftwareEngineer
    {
        public string Name;
        public string Surname;

        public SoftwareEngineer(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public void Modify(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        public override string ToString()
        {
            return $"{Name}, {Surname}";
        }
    }
}
