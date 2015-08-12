using System;

namespace W10_Logon_BG_Changer.Properties
{
    [Flags]
    public enum CollectionAccessType
    {
        /// <summary>Method does not use or modify content of the collection.</summary>
        None = 0,

        /// <summary>Method only reads content of the collection but does not modify it.</summary>
        Read = 1,

        /// <summary>Method can change content of the collection but does not add new elements.</summary>
        ModifyExistingContent = 2,

        /// <summary>Method can add new elements to the collection.</summary>
        UpdatedContent = ModifyExistingContent | 4
    }
}