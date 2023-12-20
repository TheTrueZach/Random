using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager
{
    internal class GameObject
    {
        private string name;
        private string mainPng;
        private string exePath;
        private bool favorite;
        public string Name {  get { return name; } }
        public string MainPng { get { return mainPng; } }
        public string ExePath { get { return exePath; } }
        public bool Favorite { get { return favorite; } }
        public GameObject(string filepath, bool fav)
        {
            favorite = fav;
            string[] list = Directory.GetFiles(filepath);
            if (list[0].Contains(".exe"))
            {
                exePath = list[0];
                name = exePath.Substring(exePath.LastIndexOf('\\') + 1, exePath.LastIndexOf('.') - exePath.LastIndexOf('\\') - 1);
                mainPng = list[1];
            }
            else
            {
                exePath = list[1];
                name = exePath.Substring(exePath.LastIndexOf('\\') + 1, exePath.LastIndexOf('.') - exePath.LastIndexOf('\\') - 1);
                mainPng = list[0];
            }
        }
    }
}
