namespace Optimum.Contracts;

public interface IIdentifiable<TId>
{
    public TId Id { get; set; }
}