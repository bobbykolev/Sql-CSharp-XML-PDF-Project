//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicStore.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Song
    {
        public int SongID { get; set; }
        public int AlbumID { get; set; }
        public string SongName { get; set; }
        public System.TimeSpan Duration { get; set; }
    
        public virtual Album Album { get; set; }
    }
}
