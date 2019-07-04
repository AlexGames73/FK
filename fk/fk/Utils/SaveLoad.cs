using fk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fk.Utils
{
    class SaveLoad
    {
        public static void Save(User user)
        {
            using (StreamWriter writer = new StreamWriter(File.Create(Directory.GetCurrentDirectory() + "/save.bin")))
            using (JsonTextWriter fout = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(fout, user);
            }
        }

        public static User Load()
        {
            try
            {
                User user;
                using (StreamReader reader = new StreamReader(File.OpenRead(Directory.GetCurrentDirectory() + "/save.bin")))
                using (JsonTextReader fin = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    user = serializer.Deserialize<User>(fin);
                }
                return user;
            }
            catch
            {
                return new User();
            }
        }
    }
}
