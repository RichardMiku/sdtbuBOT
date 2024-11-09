using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace sdtbuhelperLib
{
    internal class des
    {
        public static string desjscontent()
        {
            string resourceName = "sdtbuhelperLib.des.js"; // 确保使用正确的命名空间和文件名
            string scriptContent = GetEmbeddedResource(resourceName);
            return scriptContent;
        }

        static string GetEmbeddedResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
