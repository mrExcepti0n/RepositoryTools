using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryAbstraction
{
    public interface IIdentityProvider
    {
        string User { get; set; }
    }
}
