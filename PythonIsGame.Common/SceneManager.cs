using System;
using System.Collections.Generic;

namespace PythonIsGame.Common
{
    public class SceneManager
    {
        private Stack<Scene> scenes = new Stack<Scene>();
        public event Action<SceneManager, Scene> SceneReplaced;
        public event Action<SceneManager, Scene> SceneInitializing;
        public int Count => scenes.Count;

        public Scene Current()
        {
            if (scenes.Count == 0)
                return null;
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
