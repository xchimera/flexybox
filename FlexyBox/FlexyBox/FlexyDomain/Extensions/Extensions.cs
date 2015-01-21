using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Extensions
{
    //lavet af Vijeeth og Søren
    public static class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> list)
        {
            if(list == null)            
                return;
            //lav et tjek på om den listen findes, og iterer igennem den og tilføj elementerne til den liste metoden blev kaldt på
            foreach(var item in list)
            {
                source.Add(item);
            }
      
        }
        public static void AddRange<T>(this BindingList<T> source, IEnumerable<T> list)
        {
            if (list == null)
                return;
            //lav et tjek på om den listen findes, og iterer igennem den og tilføj elementerne til den liste metoden blev kaldt på
            foreach (var item in list)
            {
                source.Add(item);
            }

        }

        public static void ForEach<T>(this BindingList<T> source, Action<T> action)
        {
            //iterer igennem listen som metoden blev kaldt på og udfør en Action
            foreach (T item in source)
                action(item);
        }

        public static void ForEach<T>(this ObservableCollection<T> source, Action<T> action)
        {
            //iterer igennem listen som metoden blev kaldt på og udfør en Action
            foreach (T item in source)
                action(item);
        }
        public static void ForEach<T>(this IList<T> source, Action<T> action)
        {
            //iterer igennem listen som metoden blev kaldt på og udfør en Action
            foreach (T item in source)
                action(item);
        }
        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> source)
        {
            //skab en ny binding list med indholdet af den liste som metoden blev kaldt på og returner den.
            return new BindingList<T>(source.ToList());
        }


    }
}
