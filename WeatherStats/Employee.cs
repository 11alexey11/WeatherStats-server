public class Employee
{
    public string Name { get; set; }
    public double Salary { get; set; }
    public bool ExpensiveComputation()
    {
        Thread.Sleep(2);
        return (Salary > 2000 && Salary < 3000);
    }
    public bool NonExpensiveComputation()
    {
        return (Salary > 2000 && Salary < 3000);
    }
}
