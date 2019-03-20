using FFMPEGWrapper;
using FFMPEGWrapper.Arguments;
using System.Linq;

internal class ArgumentHelper
{
    internal static string CreateArgumentString(InputFilePath inputArgument, OutputFilePath outputArgument, params IArgument[] arguments)
    {
        var input = inputArgument.ToStringRepresentation();
        var output = outputArgument.ToStringRepresentation();
        var stringArgs = arguments.Select(a => a.ToStringRepresentation()).ToArray();
        return input + " " + string.Join(" ", stringArgs) + " " + output;
    }
}
