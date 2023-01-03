using System;
namespace Tontonator.Core.Data
{
    public static class Extensions
    {
        public static ICollection<T> Clone<T>(this ICollection<T> listToClone)
        {
            var array = new T[listToClone.Count];

            listToClone.CopyTo(array, 0);
            return array.ToList();
        }
    }
}