using NUnit.Framework;
using PythonIsGame.Common;

namespace PythonIsGame.Tests
{
    [TestFixture]
    public class SceneManagerTests
    {
        [Test]
        public void PushSceneTest()
        {
            var manager = new SceneManager();
            var scene = new SampleScene();

            manager.PushScene(scene, null);

            Assert.AreEqual(1, manager.Count);
            Assert.AreEqual(scene, manager.Current());

            var newScene = new SampleScene();

            manager.PushScene(newScene, null);

            Assert.AreEqual(2, manager.Count);
            Assert.AreEqual(newScene, manager.Current());
        }

        [Test]
        public void ReplaceSceneTest()
        {
            var manager = new SceneManager();
            var scene = new SampleScene();
            var newScene = new SampleScene();

            manager.PushScene(scene, null);

            Assert.AreEqual(1, manager.Count);
            Assert.AreEqual(scene, manager.Current());

            manager.ReplaceScene(newScene, null);

            Assert.AreEqual(1, manager.Count);
            Assert.AreEqual(newScene, manager.Current());
        }

        [Test]
        public void RemoveSceneTest()
        {
            var manager = new SceneManager();
            var scene = new SampleScene();
            var newScene = new SampleScene();

            manager.PushScene(scene, null);
            manager.PushScene(newScene, null);

            Assert.AreEqual(2, manager.Count);
            Assert.AreEqual(newScene, manager.Current());

            manager.RemoveScene();

            Assert.AreEqual(1, manager.Count);
            Assert.AreEqual(scene, manager.Current());
        }

        [Test]
        public void LackScenes()
        {
            var manager = new SceneManager();

            Assert.AreEqual(0, manager.Count);
            Assert.AreEqual(null, manager.Current());
        }

        private class SampleScene : Scene
        {
        }
    }
}
