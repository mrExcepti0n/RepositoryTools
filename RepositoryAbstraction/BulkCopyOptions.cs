using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryAbstraction
{
    [Flags]
    public enum BulkCopyOptions
    {
        Default = 0,
        KeepIdentity = 1,
        CheckConstraints = 2,
        TableLock = 4,
        KeepNulls = 8,
        FireTriggers = 16, // 0x00000010
        UseInternalTransaction = 32, // 0x00000020
        AllowEncryptedValueModifications = 64, // 0x00000040
    }
}
