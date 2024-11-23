[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
class SolverAttribute : Attribute
{
    public SolverAttribute(int year, int day, Part part) {}
}