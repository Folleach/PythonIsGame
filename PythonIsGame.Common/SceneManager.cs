using PythonIsGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PythonIsGame.Common
{
    public class SceneManager
    {
        private Stack<Scene> scenes = new Stack<Scene>();
        public event Action<SceneManager, Scene> SceneReplaced;
        public event Action<SceneManager, Scene> SceneInitializing;

        public Scene Current()
        {
            return scenes.Peek();
        }

        public void PushScene(Scene scene, object data)
        {
            SceneInitializing?.Invoke(this, scene);
            scene.Create(this, data);
            scenes.Push(scene);
            SceneReplaced?.Invoke(this, scene);
        }

        public void ReplaceScene(Scene scene, object data)
        {
            RemoveScene();
            PushScene(scene, data);
        }

        public void RemoveScene()
        {
            var old = scenes.Pop();
            var next = scenes.Count > 0 ? Current() : null;
            SceneReplaced?.Invoke(this, next);
            old.Destroy();
        }
    }
}
