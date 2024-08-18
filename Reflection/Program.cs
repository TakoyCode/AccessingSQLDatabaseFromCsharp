using System.Reflection;
using Reflection.ClassLibrary;

namespace Reflection.Main
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var assemblyClassLibrary = Assembly.GetAssembly(typeof(Cola));
            var assemblyMain = Assembly.GetExecutingAssembly();

            var pepsiType = assemblyMain.GetType("Reflection.Main.Pepsi");

            var constructorInfo = pepsiType.GetConstructors()[0];
            var pepsiObject = constructorInfo.Invoke(null);

            var methods = pepsiType.GetMethods();

            foreach (var methodInfo in methods)
            {
                Console.Write(methodInfo.ReturnType.FullName + " " + methodInfo.Name + "( ");
                var parameterInfos = methodInfo.GetParameters();
                var parametersValues = new object[parameterInfos.Length];
                for (int index = 0; index < parameterInfos.Length; index++)
                {
                    var parameter = parameterInfos[index];
                    var name = parameter.ParameterType.FullName;
                    Console.Write((index > 0?", ":" ") + name + (index == parameterInfos.Length -1? " ":""));
                    if (name == "System.String")
                    {
                        parametersValues[index] = "Audun";
                    }
                    else if (name == "System.Object")
                    {
                        parametersValues[index] = new object();
                    }
                    else if (name == "System.Int32")
                    {
                        parametersValues[index] = 3;
                    }
                    else if (name == "System.Boolean")
                    {
                        parametersValues[index] = true;
                    }
                }

                var returnValue = methodInfo.Invoke(pepsiObject, parametersValues);
                Console.WriteLine(") => " + returnValue);
            }

            //ShowMethods(pepsiType);
            //ShowTypesInAssemblies(assemblyMain, assemblyClassLibrary);
        }

        private static void ShowMethods(Type? type)
        {
            var methods = type.GetMethods();

            foreach (var methodInfo in methods)
            {
                Console.WriteLine(methodInfo.Name);
            }
        }

        public static void ShowTypesInAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                ShowTypes(assembly);
            }
        }

        public static void ShowTypesInAssembly()
        {
            var assemblyClassLibrary = Assembly.GetAssembly(typeof(Cola));
            var assemblyMain = Assembly.GetExecutingAssembly();

            var colaType = assemblyClassLibrary.GetType("Reflection.ClassLibrary.Cola");
            var pepsiType = assemblyMain.GetType("Reflection.Main.Pepsi");

            ShowTypes(assemblyMain);
            ShowTypes(assemblyClassLibrary);
        }

        public static void ShowTypes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                Console.WriteLine(type.FullName);
            }
        }

    }
}
