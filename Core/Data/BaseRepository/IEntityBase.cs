using System;
namespace Tontonator.Core.Data.BaseRepository
{
    public interface IEntityBase
    {
        /// <summary>
        /// This property represents the generic Id property for all models. This is used as identifier in the database. 
        /// </summary>
        string Id { get; set; }

        Dictionary<string, object> ToDictionary();
    }
}

