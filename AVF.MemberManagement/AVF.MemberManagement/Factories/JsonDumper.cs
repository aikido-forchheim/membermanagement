using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using PCLStorage;

namespace AVF.MemberManagement.Factories
{
    public class JsonDumper
    {
        private readonly IUnityContainer _container;

        public JsonDumper(IUnityContainer container)
        {
            _container = container;
        }

        public async Task Main()
        {
            // ReSharper disable once RedundantNameQualifier => We need to get the FullName of the Tbo namespace, with typeof(....Test).Namespace we get the correct ns even if we ever rename it.
            var tboTypes = GetTypesInNamespace(typeof(Beitragsklasse).GetTypeInfo().Assembly, typeof(AVF.MemberManagement.StandardLibrary.Tbo.Test).Namespace);

            foreach (var tboType in tboTypes)
            {
                if (tboType.Name == "Setting") continue;

                Debug.WriteLine(tboType.Name);

                var typeOfIProxy = typeof(IProxy<>);

                Type[] typeArgsForGenericIProxy = { tboType };

                var genericIProxyType = typeOfIProxy.MakeGenericType(typeArgsForGenericIProxy);

                dynamic proxy = _container.Resolve(genericIProxyType);

                var data = await proxy.GetAsync();

                var json = (string)Newtonsoft.Json.JsonConvert.SerializeObject(data);

                var tboTypeName = tboType.Name;

                await WriteJson(json, tboTypeName);
            }

            var settingsProxy = _container.Resolve<IProxyBase<Setting, string>>();

            var settingsJson = Newtonsoft.Json.JsonConvert.SerializeObject(await settingsProxy.GetAsync());

            await WriteJson(settingsJson, nameof(Setting));
        }

        private static async Task WriteJson(string json, string tboTypeName)
        {
            var localStorage = FileSystem.Current.LocalStorage;

            var avfFolder = await localStorage.CreateFolderAsync("AVF",
                CreationCollisionOption.OpenIfExists);

            var jsonFile = await avfFolder.CreateFileAsync($"List{tboTypeName}.json",
                CreationCollisionOption.ReplaceExisting);

            await jsonFile.WriteAllTextAsync(json);
        }

        private static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.ExportedTypes.Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }
    }
}
