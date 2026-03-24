using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Riddle_Shutova_PRI122.Managers;
using Riddle_Shutova_PRI122.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Riddle_Shutova_PRI122
{
    public partial class Form1 : Form
    {
        #region Константы персонажей
        private const float DOG_OFFSET_X = 2.5f;
        private const float PUPPY_OFFSET_X = 5.8f;
        private const float DAD_OFFSET_X = -3.0f;

        private const float DOG_BASE_Y = -0.1f;
        private const float PUPPY_BASE_Y = -1.0f;
        private const float DAUGHTER_BASE_Y = 0.2f;
        private const float DAD_BASE_Y = 1.4f;
        #endregion

        private float rotateX = 0;
        private float rotateY = 0;
        private float transX = -2.8f;
        private float transY = -1.0f;
        private float distance = -12.0f;
        private bool isMouseDown = false;
        private Point lastMousePos;
        private bool isInitialized = false;

        private Daughter daughter;
        private Dog dog;
        private Puppy puppy;
        private Dad dad;
        private JumpManager jumpManager;
        private bool spaceKeyPressed = false;
        private ObstacleManager obstacleManager;
        private CoinManager coinManager;
        private List<DragonCurve> dragonCurves;
        private FilterManager filterManager;
        private List<CloudCluster> clouds;

        private List<CoinExplosion> explosions = new List<CoinExplosion>();
        private Random particleRand = new Random();

        private SoundManager soundManager;

        private List<BushSprite> bushes = new List<BushSprite>();

        private float groundStartX = -16.0f;
        private float groundEndX = 16.0f;
        private float groundStartZ = -6.0f;
        private float groundEndZ = 6.0f;
        private float groundYLevel = -1.35f;
        private float groundThickness = 2.0f;

        private enum GameState
        {
            Menu,
            Playing,
            Paused,
            GameOver,
            Edit
        }

        private GameState previousState;
        private GameState currentState = GameState.Menu;
        private string[] filterOptions = { "Фильтр: нет", "Фильтр: акварелизация" };
        private int currentFilterIndex = 0;
        private bool isGameStarted = false;

        private enum ControlledObject { Dad, Daughter, Dog, Puppy }
        private ControlledObject currentControlled = ControlledObject.Dad;
        private bool tabKeyPressed = false;
        private float rotateSpeed = 5f;

        private Label selectionLabel;
        private Label scoreLabel;
        private int score = 0;
        private int bestScore = 0;

        private List<Label> titleLetters = new List<Label>();
        private Label gameOverScoreLabel;
        private Label gameOverBestScoreLabel;
        private Label gameOverTitleLabel;

        private int volumePercent = 100;
        private bool animationsEnabled = true;

        public Form1()
        {
            InitializeComponent();

            GLCtrl.Dock = DockStyle.Fill;

            daughter = new Daughter();
            dog = new Dog();
            puppy = new Puppy();
            dad = new Dad();

            jumpManager = new JumpManager();
            jumpManager.JumpUpdated += JumpManager_JumpUpdated;

            obstacleManager = new ObstacleManager(groundStartX, groundEndX, groundStartZ, groundEndZ);
            coinManager = new CoinManager(groundStartX, groundEndX, groundStartZ, groundEndZ);

            jumpTimer.Enabled = false;
            obstacleTimer.Enabled = false;

            InitializeScoreDisplay();
            InitializeSelectionLabel();
            InitializeDragonCurves();
            InitializeTitleLabel();
            InitializeGameOverLabels();

            clouds = new List<CloudCluster>();
            Random randCloud = new Random(123);
            int clusterCount = 40;

            for (int i = 0; i < clusterCount; i++)
            {
                float posX, posY, posZ;

                do
                {
                    posX = (float)(randCloud.NextDouble() * 120 - 60);
                    posY = (float)(randCloud.NextDouble() * 15 + 5);
                    posZ = (float)(randCloud.NextDouble() * 120 - 60);
                }
                while (Math.Abs(posX) < 30 && Math.Abs(posZ) < 30 && posY < 30);

                clouds.Add(new CloudCluster(new Vector3(posX, posY, posZ), randCloud));
            }

            ShowMainMenu();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.Resize += Form1_Resize;

            GLCtrl.KeyDown += GLCtrl_KeyDown;
            GLCtrl.KeyUp += GLCtrl_KeyUp;
            GLCtrl.MouseWheel += GLCtrl_MouseWheel;
            GLCtrl.TabStop = true;

            filterManager = new FilterManager();
            soundManager = new SoundManager("background_music.mp3", "coin.wav", "game_over.wav");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CenterMenuPanels();
            CenterGameOverLabels();
            CenterTitleLabel();
        }

        #region Инициализация интерфейса

        private void InitializeSelectionLabel()
        {
            selectionLabel = new Label
            {
                Text = "Управление: Dad",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Location = new Point(10, 40),
                AutoSize = true
            };
            Controls.Add(selectionLabel);
            selectionLabel.BringToFront();
        }

        private void InitializeScoreDisplay()
        {
            scoreLabel = new Label
            {
                Text = "Очки: 0",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Location = new Point(10, 10),
                AutoSize = true
            };
            Controls.Add(scoreLabel);
            scoreLabel.BringToFront();
        }

        private void InitializeTitleLabel()
        {
            string title = "ЗАГАДКА";
            Color[] colors = { Color.Red, Color.Orange, Color.Chocolate, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
            int letterWidth = 40;
            int totalWidth = title.Length * letterWidth;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int yPos = 20;

            for (int i = 0; i < title.Length; i++)
            {
                Label letterLabel = new Label
                {
                    Text = title[i].ToString(),
                    Font = new Font("Arial", 32, FontStyle.Bold),
                    ForeColor = colors[i % colors.Length],
                    BackColor = Color.Transparent,
                    Location = new Point(startX + i * letterWidth, yPos),
                    AutoSize = true
                };
                Controls.Add(letterLabel);
                letterLabel.BringToFront();
                titleLetters.Add(letterLabel);
            }
        }

        private void CenterTitleLabel()
        {
            int totalWidth = titleLetters.Count * 40;
            int startX = (this.ClientSize.Width - totalWidth) / 2;
            int yPos = 100;
            for (int i = 0; i < titleLetters.Count; i++)
            {
                titleLetters[i].Location = new Point(startX + i * 40, yPos);
            }
        }

        private void InitializeGameOverLabels()
        {
            gameOverTitleLabel = new Label
            {
                Text = "ИГРА ОКОНЧЕНА",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = true,
                Visible = false
            };
            Controls.Add(gameOverTitleLabel);
            gameOverTitleLabel.BringToFront();

            gameOverScoreLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = true,
                Visible = false
            };
            Controls.Add(gameOverScoreLabel);
            gameOverScoreLabel.BringToFront();

            gameOverBestScoreLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = true,
                Visible = false
            };
            Controls.Add(gameOverBestScoreLabel);
            gameOverBestScoreLabel.BringToFront();
        }

        private void InitializeDragonCurves()
        {
            dragonCurves = new List<DragonCurve>();
            Random rand = new Random(42);
            int rows = 9;
            int cols = 4;
            float spacingX = 3.5f;
            float spacingZ = 2.5f;
            float startX = -14f;
            float startZ = -3.7f;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    float centerX = startX + i * spacingX;
                    float centerZ = startZ + j * spacingZ;
                    float scale = 1.1f + (float)rand.NextDouble() * 0.5f;
                    float rotation = rand.Next(0, 360);
                    dragonCurves.Add(new DragonCurve(10, 0.15f, groundYLevel + 0.05f, centerX, centerZ, scale, rotation));
                }
            }
        }

        private void ShowMainMenu()
        {
            mainMenuPanel.Visible = true;
            settingsPanel.Visible = false;
            aboutPanel.Visible = false;

            if (selectionLabel != null) selectionLabel.Visible = false;

            if (currentState == GameState.GameOver)
            {
                foreach (var letter in titleLetters)
                    letter.Visible = false;
                gameOverTitleLabel.Visible = true;
                gameOverScoreLabel.Visible = true;
                gameOverBestScoreLabel.Visible = true;
                CenterGameOverLabels();
            }
            else
            {
                foreach (var letter in titleLetters)
                    letter.Visible = true;
                CenterTitleLabel();
                gameOverTitleLabel.Visible = false;
                gameOverScoreLabel.Visible = false;
                gameOverBestScoreLabel.Visible = false;
            }

            CenterMenuPanels();
        }

        private void CenterGameOverLabels()
        {
            int centerX = this.ClientSize.Width / 2;
            gameOverTitleLabel.Location = new Point(centerX - gameOverTitleLabel.Width / 2, 100);
            gameOverScoreLabel.Location = new Point(centerX - gameOverScoreLabel.Width / 2, 160);
            gameOverBestScoreLabel.Location = new Point(centerX - gameOverBestScoreLabel.Width / 2, 190);
        }

        private void CenterMenuPanels()
        {
            int centerX = (this.ClientSize.Width - mainMenuPanel.Width) / 2;
            int topOffset = 220;
            mainMenuPanel.Location = new Point(centerX, topOffset);
            settingsPanel.Location = new Point(centerX, topOffset);
            aboutPanel.Location = new Point(centerX, topOffset);
        }

        private void ShowSettingsPanel()
        {
            mainMenuPanel.Visible = false;
            settingsPanel.Visible = true;
            aboutPanel.Visible = false;
            foreach (var letter in titleLetters)
                letter.Visible = false;
            CenterMenuPanels();
        }

        private void ShowAboutPanel()
        {
            mainMenuPanel.Visible = false;
            settingsPanel.Visible = false;
            aboutPanel.Visible = true;
            foreach (var letter in titleLetters)
                letter.Visible = false;
            CenterMenuPanels();
        }

        #endregion

        #region Обработчики кнопок

        private void SettingsButton_Click(object sender, EventArgs e) => ShowSettingsPanel();
        private void AboutButton_Click(object sender, EventArgs e) => ShowAboutPanel();
        private void BackFromSettingsButton_Click(object sender, EventArgs e) => ShowMainMenu();
        private void BackFromAboutButton_Click(object sender, EventArgs e) => ShowMainMenu();

        private void ToggleAnimationsButton_Click(object sender, EventArgs e)
        {
            animationsEnabled = !animationsEnabled;
            toggleAnimationsButton.Text = animationsEnabled ? "Анимации: вкл" : "Анимации: выкл";
        }

        private void VolumeButton_Click(object sender, EventArgs e)
        {
            volumePercent -= 10;
            if (volumePercent < 0) volumePercent = 100;
            volumeButton.Text = $"Громкость: {volumePercent}%";
            soundManager.SetVolume(volumePercent / 100f);
        }

        #endregion

        #region Таймеры и игровая логика

        private void JumpTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                jumpManager.UpdateJump();
                if (!jumpManager.HasActiveSequences())
                    jumpTimer.Stop();
                GLCtrl.Invalidate();
            }
            catch (Exception ex)
            {
                jumpTimer.Stop();
            }
        }

        private void ObstacleTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (currentState == GameState.Playing)
                {
                    obstacleManager?.Update();
                    if (CheckCollision())
                    {
                        GameOver();
                        return;
                    }

                    coinManager?.Update();
                    CheckCoinCollection();
                    GLCtrl.Invalidate();
                }

                for (int i = explosions.Count - 1; i >= 0; i--)
                {
                    explosions[i].Update(0.016f);
                    if (!explosions[i].IsActive)
                        explosions.RemoveAt(i);
                }
                float deltaTime = 0.016f;
                if (animationsEnabled)
                {
                    foreach (var bush in bushes)
                        bush.Update(deltaTime);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void JumpManager_JumpUpdated(object sender, EventArgs e)
        {
            puppy.JumpOffset = jumpManager.GetCharacterOffset("Puppy");
            dog.JumpOffset = jumpManager.GetCharacterOffset("Dog");
            daughter.JumpOffset = jumpManager.GetCharacterOffset("Daughter");
            dad.JumpOffset = jumpManager.GetCharacterOffset("Dad");
            GLCtrl.Invalidate();
        }

        #endregion

        #region Обработка клавиш

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((mainMenuPanel.Visible || settingsPanel.Visible || aboutPanel.Visible) &&
                (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space || e.KeyCode == Keys.Escape))
                return;

            float moveStep = 0.2f;

            switch (e.KeyCode)
            {
                case Keys.Left: transX += moveStep; break;
                case Keys.Right: transX -= moveStep; break;
                case Keys.Up: transY -= moveStep; break;
                case Keys.Down: transY += moveStep; break;

                case Keys.Space:
                    if (currentState == GameState.Playing)
                    {
                        spaceKeyPressed = true;
                        e.Handled = true;
                    }
                    break;

                case Keys.R:
                    transX = -2.2f;
                    transY = -1.0f;
                    distance = -12.0f;
                    rotateX = 0;
                    rotateY = 0;
                    break;

                case Keys.Escape:
                    if (currentState == GameState.Playing)
                        PauseGame();
                    else if (currentState == GameState.Paused)
                    {
                        currentState = GameState.Menu;
                        ShowMainMenu();
                        soundManager.StopBackgroundMusic();
                        startButton.Text = "Начать";
                    }
                    else if (currentState == GameState.Edit)
                    {
                        currentState = previousState;
                        ShowMainMenu();
                        UpdateStartButtonText();
                    }
                    else if (currentState == GameState.GameOver)
                    {
                        ShowMainMenu();
                        startButton.Text = "Начать";
                    }
                    else if (currentState == GameState.Menu)
                    {
                        Application.Exit();
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Tab:
                    if (!tabKeyPressed)
                    {
                        tabKeyPressed = true;
                        currentControlled = (ControlledObject)(((int)currentControlled + 1) % 4);
                        UpdateSelectionLabel();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.Q:
                    RotateCurrentObject(-rotateSpeed);
                    break;
                case Keys.E:
                    RotateCurrentObject(rotateSpeed);
                    break;
            }

            GLCtrl.Invalidate();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && spaceKeyPressed)
            {
                spaceKeyPressed = false;
                if (currentState == GameState.Playing)
                {
                    bool isDoubleJump = jumpManager.IsPuppyInAir();
                    jumpManager.StartJumpSequence(isDoubleJump);
                    if (!jumpTimer.Enabled)
                        jumpTimer.Start();
                    e.Handled = true;
                }
            }
            if (e.KeyCode == Keys.Tab)
            {
                tabKeyPressed = false;
                e.Handled = true;
            }
        }

        private void GLCtrl_KeyDown(object sender, KeyEventArgs e) => Form1_KeyDown(sender, e);
        private void GLCtrl_KeyUp(object sender, KeyEventArgs e) => Form1_KeyUp(sender, e);

        #endregion

        #region Управление игрой

        private void StartGame()
        {
            currentState = GameState.Playing;
            mainMenuPanel.Visible = false;
            settingsPanel.Visible = false;
            aboutPanel.Visible = false;
            if (selectionLabel != null) selectionLabel.Visible = false;
            foreach (var letter in titleLetters)
                letter.Visible = false;
            gameOverTitleLabel.Visible = false;
            gameOverScoreLabel.Visible = false;
            gameOverBestScoreLabel.Visible = false;

            jumpTimer.Start();
            obstacleTimer.Start();
            isGameStarted = true;
            soundManager.PlayBackgroundMusic(true);
            GLCtrl.Focus();
        }

        private void PauseGame()
        {
            if (currentState != GameState.Playing) return;
            currentState = GameState.Paused;
            jumpTimer.Stop();
            obstacleTimer.Stop();
            startButton.Text = "Продолжить";
            ShowMainMenu();
            soundManager.StopBackgroundMusic();
            foreach (var letter in titleLetters)
                letter.Visible = true;
            CenterTitleLabel();
            gameOverTitleLabel.Visible = false;
            gameOverScoreLabel.Visible = false;
            gameOverBestScoreLabel.Visible = false;
        }

        private void ResumeGame()
        {
            if (currentState != GameState.Paused) return;
            currentState = GameState.Playing;
            mainMenuPanel.Visible = false;
            settingsPanel.Visible = false;
            aboutPanel.Visible = false;
            if (selectionLabel != null) selectionLabel.Visible = false;
            foreach (var letter in titleLetters)
                letter.Visible = false;
            gameOverTitleLabel.Visible = false;
            gameOverScoreLabel.Visible = false;
            gameOverBestScoreLabel.Visible = false;

            jumpTimer.Start();
            obstacleTimer.Start();
            soundManager.PlayBackgroundMusic(true);
            GLCtrl.Focus();
        }

        private void GameOver()
        {
            if (currentState != GameState.Playing) return;
            currentState = GameState.GameOver;
            jumpTimer.Stop();
            obstacleTimer.Stop();
            startButton.Text = "Начать заново";
            soundManager.StopBackgroundMusic();
            soundManager.PlayGameOverSound();

            if (score > bestScore)
                bestScore = score;

            gameOverScoreLabel.Text = $"Счёт: {score}";
            gameOverBestScoreLabel.Text = $"Лучший счёт: {bestScore}";

            ShowMainMenu();
            CenterMenuPanels();
            CenterGameOverLabels();
        }

        private void ResetGame()
        {
            currentState = GameState.Playing;
            puppy.JumpOffset = 0;
            dog.JumpOffset = 0;
            daughter.JumpOffset = 0;
            dad.JumpOffset = 0;
            ResetScore();

            obstacleManager = new ObstacleManager(groundStartX, groundEndX, groundStartZ, groundEndZ);
            coinManager = new CoinManager(groundStartX, groundEndX, groundStartZ, groundEndZ);
            jumpManager = new JumpManager();
            jumpManager.JumpUpdated += JumpManager_JumpUpdated;

            mainMenuPanel.Visible = false;
            settingsPanel.Visible = false;
            aboutPanel.Visible = false;
            gameOverScoreLabel.Visible = false;
            gameOverBestScoreLabel.Visible = false;
            if (selectionLabel != null) selectionLabel.Visible = false;
            foreach (var letter in titleLetters)
                letter.Visible = false;

            jumpTimer.Start();
            obstacleTimer.Start();
            soundManager.PlayBackgroundMusic(true);
            GLCtrl.Invalidate();
            GLCtrl.Focus();
        }

        private void UpdateStartButtonText()
        {
            if (currentState == GameState.Paused)
                startButton.Text = "Продолжить";
            else if (currentState == GameState.GameOver)
                startButton.Text = "Начать заново";
            else if (currentState == GameState.Menu)
                startButton.Text = "Начать";
        }

        private void UpdateSelectionLabel()
        {
            selectionLabel.Text = $"Управление: {currentControlled}";
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (currentState == GameState.Menu)
                StartGame();
            else if (currentState == GameState.Paused)
                ResumeGame();
            else if (currentState == GameState.GameOver)
                ResetGame();
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            currentFilterIndex = (currentFilterIndex + 1) % filterOptions.Length;
            filterButton.Text = filterOptions[currentFilterIndex];
            GLCtrl.Invalidate();
        }

        private void ExitButton_Click(object sender, EventArgs e) => Application.Exit();

        private void EditButton_Click(object sender, EventArgs e)
        {
            previousState = currentState;
            if (currentState == GameState.Menu || currentState == GameState.Paused || currentState == GameState.GameOver)
            {
                currentState = GameState.Edit;
                mainMenuPanel.Visible = false;
                settingsPanel.Visible = false;
                aboutPanel.Visible = false;
                if (selectionLabel != null) selectionLabel.Visible = true;
                jumpTimer.Stop();
                obstacleTimer.Stop();
                soundManager.StopBackgroundMusic();
                GLCtrl.Focus();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            puppy.RotationY = 0;
            dog.RotationY = 0;
            daughter.RotationY = 0;
            dad.RotationY = 0;
            GLCtrl.Invalidate();
        }

        #endregion

        #region Отрисовка

        private void GLCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                GLCtrl.MakeCurrent();

                GL.ClearColor(Color.LightSkyBlue);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
                GL.Enable(EnableCap.ColorMaterial);
                GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);

                float[] lightPos = { 5.0f, 10.0f, 5.0f, 1.0f };
                GL.Light(LightName.Light0, LightParameter.Position, lightPos);
                float[] whiteLight = { 1.0f, 1.0f, 1.0f, 1.0f };
                GL.Light(LightName.Light0, LightParameter.Diffuse, whiteLight);
                float[] ambientLight = { 0.3f, 0.3f, 0.3f, 1.0f };
                GL.Light(LightName.Light0, LightParameter.Ambient, ambientLight);

                filterManager.Initialize(GLCtrl.Width, GLCtrl.Height);
                Obstacle.LoadSharedTexture();

                string spritePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bush.png");
                BushSprite.LoadTexture(spritePath);

                float groundMinX = -15f;
                float groundMaxX = 15f;
                float groundMinZ = -5f;
                float groundMaxZ = 5f;
                float bushY = groundYLevel + 0.8f;
                float bushScale = 4.0f;
                float bushSpacing = 2.5f;

                bushes.Clear();

                for (float z = groundMinZ + 0.5f; z <= groundMaxZ - 0.5f; z += bushSpacing)
                {
                    bushes.Add(new BushSprite(new Vector3(groundMinX + 0.2f, bushY, z), bushScale));
                    bushes.Add(new BushSprite(new Vector3(groundMaxX - 0.2f, bushY, z), bushScale));
                }
                for (float x = groundMinX + 0.5f; x <= groundMaxX - 0.5f; x += bushSpacing)
                {
                    bushes.Add(new BushSprite(new Vector3(x, bushY, groundMinZ + 0.2f), bushScale));
                    bushes.Add(new BushSprite(new Vector3(x, bushY, groundMaxZ - 0.2f), bushScale));
                }

                isInitialized = true;
                GLCtrl_Resize(null, EventArgs.Empty);
                GLCtrl.Invalidate();
            }
            catch (Exception ex)
            {
            }
        }

        private void GLCtrl_Paint(object sender, PaintEventArgs e)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            if (!isInitialized)
            {
                return;
            }

            try
            {
                GLCtrl.MakeCurrent();

                if (currentFilterIndex == 1 && filterManager != null)
                {
                    filterManager.BeginRenderToTexture();
                    RenderScene3D();
                    filterManager.EndRenderAndApplyWatercolor();
                }
                else
                {
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    RenderScene3D();
                }

                GLCtrl.SwapBuffers();
            }
            catch (Exception ex) { }
        }

        private void RenderScene3D()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(transX, transY, distance);
            GL.Rotate(rotateX, 1, 0, 0);
            GL.Rotate(rotateY, 0, 1, 0);

            DrawGround();
            DrawDragonCurves();

            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            DrawShadow(PUPPY_OFFSET_X, 0, 0.35f);
            DrawShadow(DOG_OFFSET_X, 0, 1.0f);
            DrawShadow(0, 0, 0.4f);
            DrawShadow(DAD_OFFSET_X, 0, 0.45f);

            foreach (var bush in bushes)
                DrawShadow(bush.Position.X, bush.Position.Z, 0.9f);

            if (obstacleManager != null)
            {
                foreach (var obs in obstacleManager.Obstacles)
                    if (obs.IsActive)
                        DrawShadow(obs.PositionX, 0, obs.Width * 0.6f);
            }

            if (coinManager != null)
            {
                foreach (var coin in coinManager.Coins)
                    if (coin.IsActive)
                        DrawShadow(coin.PositionX, 0, 0.2f);
            }

            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);

            if (clouds != null)
            {
                foreach (var cloud in clouds)
                    cloud.Draw();
            }

            obstacleManager?.Draw();
            coinManager?.Draw();
            dad.Draw();
            daughter.Draw();
            dog.Draw();
            puppy.Draw();

            GL.DepthMask(false);
            float[] viewMatrix = new float[16];
            GL.GetFloat(GetPName.ModelviewMatrix, viewMatrix);
            Vector3 cameraRight = new Vector3(viewMatrix[0], viewMatrix[4], viewMatrix[8]);
            Vector3 cameraUp = new Vector3(viewMatrix[1], viewMatrix[5], viewMatrix[9]);
            GL.Disable(EnableCap.Lighting);
            foreach (var bush in bushes)
                bush.Draw(cameraRight, cameraUp);
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);

            if (explosions.Count > 0)
            {
                GL.Disable(EnableCap.Lighting);
                GL.Disable(EnableCap.Normalize);
                foreach (var exp in explosions)
                    exp.Draw();
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Normalize);
            }
        }

        private void DrawGround()
        {
            GL.Enable(EnableCap.Normalize);
            float yLevel = groundYLevel;

            GL.Color3(0.2f, 0.6f, 0.2f);
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(groundStartX, yLevel, groundStartZ);
            GL.Vertex3(groundEndX, yLevel, groundStartZ);
            GL.Vertex3(groundEndX, yLevel, groundEndZ);
            GL.Vertex3(groundStartX, yLevel, groundEndZ);
            GL.End();

            GL.Color3(0.1f, 0.4f, 0.1f);
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 0, -1);
            GL.Vertex3(groundStartX, yLevel, groundStartZ);
            GL.Vertex3(groundStartX, yLevel - groundThickness, groundStartZ);
            GL.Vertex3(groundEndX, yLevel - groundThickness, groundStartZ);
            GL.Vertex3(groundEndX, yLevel, groundStartZ);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(groundStartX, yLevel, groundEndZ);
            GL.Vertex3(groundEndX, yLevel, groundEndZ);
            GL.Vertex3(groundEndX, yLevel - groundThickness, groundEndZ);
            GL.Vertex3(groundStartX, yLevel - groundThickness, groundEndZ);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(groundStartX, yLevel, groundStartZ);
            GL.Vertex3(groundStartX, yLevel, groundEndZ);
            GL.Vertex3(groundStartX, yLevel - groundThickness, groundEndZ);
            GL.Vertex3(groundStartX, yLevel - groundThickness, groundStartZ);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(1, 0, 0);
            GL.Vertex3(groundEndX, yLevel, groundStartZ);
            GL.Vertex3(groundEndX, yLevel - groundThickness, groundStartZ);
            GL.Vertex3(groundEndX, yLevel - groundThickness, groundEndZ);
            GL.Vertex3(groundEndX, yLevel, groundEndZ);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, -1, 0);
            GL.Color3(0.3f, 0.2f, 0.1f);
            GL.Vertex3(groundStartX, yLevel - groundThickness, groundStartZ);
            GL.Vertex3(groundStartX, yLevel - groundThickness, groundEndZ);
            GL.Vertex3(groundEndX, yLevel - groundThickness, groundEndZ);
            GL.Vertex3(groundEndX, yLevel - groundThickness, groundStartZ);
            GL.End();

            GL.Disable(EnableCap.Normalize);
        }

        private void DrawDragonCurves()
        {
            if (dragonCurves == null) return;
            foreach (var curve in dragonCurves)
                curve.Draw();
        }

        private void DrawShadow(float x, float z, float radius, float opacity = 0.4f)
        {
            float y = groundYLevel + 0.02f;
            int segments = 16;

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(0f, 0f, 0f, opacity);
            GL.Vertex3(x, y, z);
            for (int i = 0; i <= segments; i++)
            {
                float angle = 2 * MathF.PI * i / segments;
                float dx = radius * MathF.Cos(angle);
                float dz = radius * MathF.Sin(angle);
                GL.Vertex3(x + dx, y, z + dz);
            }
            GL.End(); 
        }

        #endregion

        #region Обработка мыши и размера

        private void GLCtrl_Resize(object sender, EventArgs e)
        {
            if (!isInitialized || GLCtrl.Width == 0 || GLCtrl.Height == 0) return;

            GL.Viewport(0, 0, GLCtrl.Width, GLCtrl.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspect = (float)GLCtrl.Width / GLCtrl.Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), aspect, 0.1f, 100.0f);
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);

            filterManager?.Resize(GLCtrl.Width, GLCtrl.Height);
            CenterGameOverLabels();
            CenterTitleLabel();
        }

        private void GLCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            lastMousePos = e.Location;
            GLCtrl.Focus();
        }

        private void GLCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                rotateY += (e.X - lastMousePos.X) * 0.5f;
                rotateX += (e.Y - lastMousePos.Y) * 0.5f;
                lastMousePos = e.Location;
                GLCtrl.Invalidate();
            }
        }

        private void GLCtrl_MouseUp(object sender, MouseEventArgs e) => isMouseDown = false;

        private void GLCtrl_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomStep = 0.5f;
            if (e.Delta > 0)
            {
                distance += zoomStep;
                if (distance > -1.0f) distance = -1.0f;
            }
            else
            {
                distance -= zoomStep;
                if (distance < -20.0f) distance = -20.0f;
            }
            GLCtrl.Invalidate();
        }

        #endregion

        #region Столкновения и физика

        private bool CheckCollision()
        {
            if (obstacleManager == null) return false;

            var characters = new[]
            {
                new { Name = "Puppy", X = PUPPY_OFFSET_X, Y = PUPPY_BASE_Y + puppy.JumpOffset, W = 0.6f, H = 0.8f, D = 0.5f },
                new { Name = "Dog", X = DOG_OFFSET_X, Y = DOG_BASE_Y + dog.JumpOffset, W = 0.8f, H = 1.0f, D = 0.6f },
                new { Name = "Daughter", X = 0f, Y = DAUGHTER_BASE_Y + daughter.JumpOffset, W = 0.5f, H = 1.2f, D = 0.4f },
                new { Name = "Dad", X = DAD_OFFSET_X, Y = DAD_BASE_Y + dad.JumpOffset, W = 0.6f, H = 1.5f, D = 0.5f }
            };

            foreach (var ch in characters)
            {
                float chMinX = ch.X - ch.W / 2, chMaxX = ch.X + ch.W / 2;
                float chMinY = ch.Y - ch.H / 2, chMaxY = ch.Y + ch.H / 2;
                float chMinZ = -ch.D / 2, chMaxZ = ch.D / 2;

                foreach (var obs in obstacleManager.Obstacles)
                {
                    if (!obs.IsActive) continue;
                    float obsMinX = obs.PositionX - obs.Width / 2, obsMaxX = obs.PositionX + obs.Width / 2;
                    float obsMinY = obs.PositionY, obsMaxY = obs.PositionY + obs.Height;
                    float obsMinZ = -obs.Depth / 2, obsMaxZ = obs.Depth / 2;

                    bool collisionX = chMaxX > obsMinX && chMinX < obsMaxX;
                    bool collisionY = chMaxY > obsMinY && chMinY < obsMaxY;
                    bool collisionZ = chMaxZ > obsMinZ && chMinZ < obsMaxZ;

                    if (collisionX && collisionY && collisionZ)
                        return true;
                }
            }
            return false;
        }

        private void CheckCoinCollection()
        {
            if (coinManager == null) return;

            var characters = new[]
            {
                new { Name = "Puppy", X = PUPPY_OFFSET_X, Y = PUPPY_BASE_Y + puppy.JumpOffset, Z = 0f },
                new { Name = "Dog", X = DOG_OFFSET_X, Y = DOG_BASE_Y + dog.JumpOffset, Z = 0f },
                new { Name = "Daughter", X = 0f, Y = DAUGHTER_BASE_Y + daughter.JumpOffset, Z = 0f },
                new { Name = "Dad", X = DAD_OFFSET_X, Y = DAD_BASE_Y + dad.JumpOffset, Z = 0f }
            };

            float radius = 0.7f;
            for (int i = coinManager.Coins.Count - 1; i >= 0; i--)
            {
                var coin = coinManager.Coins[i];
                if (!coin.IsActive) continue;

                bool collected = false;
                foreach (var ch in characters)
                {
                    float dx = ch.X - coin.PositionX;
                    float dy = ch.Y - coin.PositionY;
                    float dz = ch.Z - coin.PositionZ;
                    if (Math.Sqrt(dx * dx + dy * dy + dz * dz) < radius)
                    {
                        collected = true;
                        break;
                    }
                }

                if (collected)
                {
                    coin.IsActive = false;
                    coinManager.Coins.RemoveAt(i);
                    AddScore(10);
                    soundManager.PlayCoinSound();
                    Vector3 explosionPos = new Vector3(coin.PositionX, coin.PositionY, coin.PositionZ);
                    explosions.Add(new CoinExplosion(explosionPos, particleRand, 25, 1.2f, 3.5f));
                }
            }
        }

        #endregion

        #region Система очков

        private void AddScore(int points)
        {
            score += points;
            scoreLabel.Text = $"Очки: {score}";
        }

        private void ResetScore()
        {
            score = 0;
            scoreLabel.Text = "Очки: 0";
        }

        #endregion

        #region Управление объектами

        private void RotateCurrentObject(float deltaAngle)
        {
            switch (currentControlled)
            {
                case ControlledObject.Puppy: puppy.RotationY += deltaAngle; break;
                case ControlledObject.Dog: dog.RotationY += deltaAngle; break;
                case ControlledObject.Daughter: daughter.RotationY += deltaAngle; break;
                case ControlledObject.Dad: dad.RotationY += deltaAngle; break;
            }
            GLCtrl.Invalidate();
        }

        #endregion

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            soundManager?.Dispose();
            base.OnFormClosed(e);
        }
    }
}