using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Extensions
{
    public static class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> list)
        {
            if(list == null)            
                return;

            foreach(var item in list)
            {
                source.Add(item);
            }
      
        }
        public static void AddRange<T>(this BindingList<T> source, IEnumerable<T> list)
        {
            if (list == null)
                return;

            foreach (var item in list)
            {
                source.Add(item);
            }

        }

        public static void ForEach<T>(this BindingList<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

    }
}
