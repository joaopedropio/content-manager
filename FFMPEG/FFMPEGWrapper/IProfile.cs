using System.Collections.Generic;

namespace FFMPEGWrapper
{
    public interface IProfile
    {
        IList<IArgument> GetArguments();
    }
}
