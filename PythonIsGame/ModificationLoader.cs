using PythonIsGame.Common;
using PythonIsGame.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame
{
    public class ModificationLoader : IEnumerable<IGameMode>
    {
        private List<IGameMode> modifications;
        private string modificationFolder;

        public ModificationLoader(string path)
        {
            modifications = new List<IGameMode>();
            modificationFolder = Path.GetFullPath(path);
            Load();
        }

        public ModificationLoader() : this("Modifications")
        {
        }

        private void Load()
        {
            foreach (var file in AvaliableModifications())
            {
                if (!LoadModification(file))
                    throw new Exception($"Modification '{file}' can't be loaded.");
            }
        }

        internal IEnumerable<string> AvaliableModifications()
        {
            var files = Directory.GetFiles(modificationFolder, "*.dll");
            foreach (var file in files)
                yield return Path.GetFileNameWithoutExtension(file);
        }

        private bool LoadModification(string fileName)
        {
            var path = Path.Combine(modificationFolder, $"{fileName}.dll");
            if (!File.Exists(path))
                return false;
            var modificationAssembly = LoadAssembly(path);
            if (modificationAssembly == null)
                return false;
            var gameModes = GetGameModes(modificationAssembly);
            if (gameModes == null)
                return false;
            modifications.AddRange(gameModes);
            return true;
        }

        private List<IGameMode> GetGameModes(Assembly assembly)
        {
            var gameModes = new List<IGameMode>();
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(IGameMode)))
                        gameModes.Add(type.CreateInstance<IGameMode>());
                }
            }
            catch
            {
                return null;
            }
            return gameModes;
        }

        private Assembly LoadAssembly(string fileName)
        {
            try
            {
                return Assembly.LoadFile(fileName);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerator<IGameMode> GetEnumerator()
        {
            foreach (var mod in modifications)
                yield return mod;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
