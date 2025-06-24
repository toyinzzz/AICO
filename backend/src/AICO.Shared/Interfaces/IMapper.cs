namespace AICO.Shared.Interfaces
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource Map(TDestination destination); // For reverse mapping
    }
}