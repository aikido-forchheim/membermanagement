using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using PCLStorage;

namespace AVF.MemberManagement.Console
{
    internal class JsonDumper
    {
        public async Task Main()
        {
            // ReSharper disable once RedundantNameQualifier => We need to get the FullName of the Tbo namespace, with typeof(....Test).Namespace we get the correct ns even if we ever rename it.
            var tboTypes = GetTypesInNamespace(typeof(Beitragsklasse).GetTypeInfo().Assembly, typeof(AVF.MemberManagement.StandardLibrary.Tbo.Test).Namespace);

            foreach (var tboType in tboTypes)
            {
                System.Console.WriteLine(tboType.Name);

                var typeOfIProxy = typeof(IProxy<>);

                Type[] typeArgsForGenericIProxy = { tboType };

                var genericIProxyType = typeOfIProxy.MakeGenericType(typeArgsForGenericIProxy);

                dynamic proxy = Program.Container.Resolve(genericIProxyType);

                var data = await proxy.GetAsync();

                var json = (string) Newtonsoft.Json.JsonConvert.SerializeObject(data);

                var localStorage = FileSystem.Current.LocalStorage;

                var avfFolder = await localStorage.CreateFolderAsync("AVF",
                    CreationCollisionOption.OpenIfExists);

                var jsonFile = await avfFolder.CreateFileAsync($"List{tboType.Name}.json",
                    CreationCollisionOption.ReplaceExisting);

                await jsonFile.WriteAllTextAsync(json);
            }
        }
        
        private static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }
    }
}
