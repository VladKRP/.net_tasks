using System;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    // In this case ScienceFiction rise exception cause value has space
    //[Flags] 
    //[Serializable]
    //public enum Genre
    //{
    //    None = 1,
    //    [XmlEnum]
    //    Computer = 2,
    //    [XmlEnum]
    //    Fantasy = 4,
    //    [XmlEnum]
    //    Romance = 6,
    //    [XmlEnum]
    //    Horror = 8,
    //    [XmlEnum("Science Fiction")]
    //    ScienceFiction = 16
    //}

    [Flags]
    [Serializable]
    public enum Genre
    {
        None = 1,
        Computer = 2,
        Fantasy = 4,
        Romance = 6,
        Horror = 8,
        ScienceFiction = 16
    }

}