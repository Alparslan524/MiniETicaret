using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class Configuration
    {
       static public string ConnectionString 
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/WebAPI"));
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("MsSql");
                //WebAPI'da bulunan appsettings.json içerisinde connections stringsi tanımladık ve  Microsoft.Extensions.Configuration kullanarak
                //Json dosyasındaki MsSql connection stringini okuduk. Artık el ile yazmamıza gerek kalmıyor. Direk bu sınıfı kullanarak
                //connection stringimizi alabiliyoruz. Kısaca appsettings.json içindeki connection stringi getiriyoruz.
            }
        }
    }
}
