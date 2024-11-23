[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
class SolverAttribute : Attribute
{
    public SolverAttribute(int year, int day, Part part) {}
}