namespace Optimum.Utilitis
{
    public class TypeUtil
    {
        public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType, IEnumerable<Type> types)
        {
            return from x in types
                   from z in x.GetInterfaces()
                   let y = x.BaseType
                   where
                   (y != null && y.IsGenericType &&
                   openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                   (z.IsGenericType &&
                   openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                   select x;
        }
    }
}
