using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TrackR.Web.Helpers
{
    public static class AssemblyScanning
    {
        public static Assembly[] GetAssemblies(Type? entryType = null)
        {
            var checkedAssemblies = new HashSet<Assembly>();
            var entryAssembly = entryType?.Assembly ?? Assembly.GetEntryAssembly()!;
            var entryAssemblyFirstPrefix = entryAssembly.GetName().Name!.Split(".").First();
            var toCheck = new Queue<Assembly>(new []{entryAssembly});

            while (toCheck.Count > 0)
            {
                var assembly = toCheck.Dequeue();
                checkedAssemblies.Add(assembly);

                var loaded = assembly.GetReferencedAssemblies()
                                     .Where(x => x.Name?.StartsWith(entryAssemblyFirstPrefix) ?? false)
                                     .Where(x => !checkedAssemblies.Any(chx => AssemblyName.ReferenceMatchesDefinition(x, chx.GetName())))
                                     .Where(x => !toCheck.Any(chx => AssemblyName.ReferenceMatchesDefinition(x, chx.GetName())))
                                     .Select(Assembly.Load);
                foreach (var loadedAssembly in loaded)
                {
                    toCheck.Enqueue(loadedAssembly);
                }
            }

            var allReferencedAssemblies = checkedAssemblies.ToArray();
            if (allReferencedAssemblies.Length == 0) throw new InvalidOperationException("Entry Assembly not loaded properly");
            return allReferencedAssemblies;
        }
    }
}