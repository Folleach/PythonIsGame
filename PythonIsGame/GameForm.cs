using PythonIsGame.Common;
using PythonIsGame.Common.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PythonIsGame
{
    public partial class GameForm : Form
    {
        public static GameForm Instance;
        private Timer timer;
        private SceneManager Scenes;
        private DrawPanel rootPanel;
        private Stopwatch deltaTime;

        public GameForm()
        {
            Instance = this;
            KeyPreview = true;
            rootPanel = new DrawPanel();
            rootPanel.BackColor = Color.Transparent;
            rootPanel.Paint += this.Painting;
            Controls.Add(rootPanel);
            timer = new Timer();
            timer.Interval = 16;
            timer.Tick += Update;
            Scenes = new SceneManager();
            Scenes.SceneReplaced += this.Scenes_SceneReplaced;
            Scenes.PushScene(new MainMenuScene(), null);
            InitializeComponent();
            timer.Start();
            deltaTime = Stopwatch.StartNew();
        }

        private void Scenes_SceneReplaced(SceneManager manager, Scene scene)
        {
            if (scene == null)
            {
                Application.Exit();
                return;
            }
            Controls.Remove(rootPanel);
            rootPanel.Paint -= Painting;
            Controls.Add(rootPanel = scene.Root);
            rootPanel.Paint += Painting;
            rootPanel.Focus();
            OnResize(null);
        }

        protected override void OnResize(EventArgs e)
        {
            rootPanel.Width = this.Width;
            rootPanel.Height = this.Height;
            Scenes.Current().Resize();
        }

        private void Update(object sender, EventArgs e)
        {
            deltaTime.Stop();
            Scenes.Current().Update(deltaTime.Elapsed);
            rootPanel.Invalidate();
            deltaTime.Restart();
        }

        private void Painting(object sender, PaintEventArgs e)
        {
            Scenes.Current().Draw(e.Graphics);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Scenes.Current().KeyDownHandlers.ContainsKey(e.KeyCode))
                Scenes.Current().KeyDownHandlers[e.KeyCode].Invoke(e);
        }
    }
}
