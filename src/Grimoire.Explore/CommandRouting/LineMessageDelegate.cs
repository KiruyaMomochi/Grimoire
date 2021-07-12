using System.Threading.Tasks;

#nullable enable
namespace Grimoire.Explore.CommandRouting
{
    /// <summary>
    /// A function that can handle LINE message event.
    /// </summary>
    /// <param name="grimoireContext">The <see cref="GrimoireContext"/>for the event.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    /// <example>
    /// async (context) => Console.WriteLine("Hello World!");
    /// </example>
    public delegate Task<Line.Api.Message.BaseMessage?> LineMessageDelegate(GrimoireContext grimoireContext);
}
#nullable restore
