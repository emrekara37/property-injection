namespace DotnetPropertyInjection.Lib;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class InjectAttribute : Attribute
{
}