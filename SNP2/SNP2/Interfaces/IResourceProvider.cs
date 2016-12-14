using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNP2
{
    public interface IResourceProvider
    {
        string ResourceFile1 { get; }
        string ResourceFile2 { get; }
    }
}
