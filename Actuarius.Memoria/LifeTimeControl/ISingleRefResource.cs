namespace Actuarius.Memoria
{
    /// <summary>
    /// Объект у которого может быть только один владелец
    /// </summary>
    public interface ISingleRefResource : IReleasableResource
    {
    }
}