namespace Riddle_Shutova_PRI122
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            GLCtrl = new OpenTK.GLControl.GLControl();
            jumpTimer = new System.Windows.Forms.Timer(components);
            obstacleTimer = new System.Windows.Forms.Timer(components);
            mainMenuPanel = new Panel();
            startButton = new Button();
            settingsButton = new Button();
            aboutButton = new Button();
            exitButton = new Button();
            settingsPanel = new Panel();
            editButton = new Button();
            resetButton = new Button();
            toggleAnimationsButton = new Button();
            volumeButton = new Button();
            filterButton = new Button();
            backFromSettingsButton = new Button();
            aboutPanel = new Panel();
            aboutLabel = new Label();
            backFromAboutButton = new Button();
            mainMenuPanel.SuspendLayout();
            settingsPanel.SuspendLayout();
            aboutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // GLCtrl
            // 
            GLCtrl.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            GLCtrl.APIVersion = new Version(3, 3, 0, 0);
            GLCtrl.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            GLCtrl.IsEventDriven = false;
            GLCtrl.Location = new Point(0, 1);
            GLCtrl.Name = "GLCtrl";
            GLCtrl.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            GLCtrl.SharedContext = null;
            GLCtrl.Size = new Size(1036, 710);
            GLCtrl.TabIndex = 0;
            GLCtrl.Load += GLCtrl_Load;
            GLCtrl.Paint += GLCtrl_Paint;
            GLCtrl.MouseDown += GLCtrl_MouseDown;
            GLCtrl.MouseMove += GLCtrl_MouseMove;
            GLCtrl.MouseUp += GLCtrl_MouseUp;
            GLCtrl.MouseWheel += GLCtrl_MouseWheel;
            GLCtrl.Resize += GLCtrl_Resize;
            // 
            // jumpTimer
            // 
            jumpTimer.Interval = 16;
            jumpTimer.Tick += JumpTimer_Tick;
            // 
            // obstacleTimer
            // 
            obstacleTimer.Interval = 16;
            obstacleTimer.Tick += ObstacleTimer_Tick;
            // 
            // mainMenuPanel
            // 
            mainMenuPanel.BackColor = Color.FromArgb(180, 30, 30, 30);
            mainMenuPanel.Controls.Add(startButton);
            mainMenuPanel.Controls.Add(settingsButton);
            mainMenuPanel.Controls.Add(aboutButton);
            mainMenuPanel.Controls.Add(exitButton);
            mainMenuPanel.Location = new Point(372, 200);
            mainMenuPanel.Name = "mainMenuPanel";
            mainMenuPanel.Size = new Size(300, 350);
            mainMenuPanel.TabIndex = 1;
            // 
            // startButton
            // 
            startButton.BackColor = Color.DodgerBlue;
            startButton.FlatAppearance.BorderSize = 0;
            startButton.FlatStyle = FlatStyle.Flat;
            startButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            startButton.ForeColor = Color.White;
            startButton.Location = new Point(50, 30);
            startButton.Name = "startButton";
            startButton.Size = new Size(200, 50);
            startButton.TabIndex = 0;
            startButton.TabStop = false;
            startButton.Text = "Начать";
            startButton.UseVisualStyleBackColor = false;
            startButton.Click += StartButton_Click;
            // 
            // settingsButton
            // 
            settingsButton.BackColor = Color.Gray;
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.FlatStyle = FlatStyle.Flat;
            settingsButton.Font = new Font("Segoe UI", 12F);
            settingsButton.ForeColor = Color.White;
            settingsButton.Location = new Point(50, 100);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(200, 50);
            settingsButton.TabIndex = 1;
            settingsButton.TabStop = false;
            settingsButton.Text = "Настройки";
            settingsButton.UseVisualStyleBackColor = false;
            settingsButton.Click += SettingsButton_Click;
            // 
            // aboutButton
            // 
            aboutButton.BackColor = Color.Gray;
            aboutButton.FlatAppearance.BorderSize = 0;
            aboutButton.FlatStyle = FlatStyle.Flat;
            aboutButton.Font = new Font("Segoe UI", 12F);
            aboutButton.ForeColor = Color.White;
            aboutButton.Location = new Point(50, 170);
            aboutButton.Name = "aboutButton";
            aboutButton.Size = new Size(200, 50);
            aboutButton.TabIndex = 2;
            aboutButton.TabStop = false;
            aboutButton.Text = "О программе";
            aboutButton.UseVisualStyleBackColor = false;
            aboutButton.Click += AboutButton_Click;
            // 
            // exitButton
            // 
            exitButton.BackColor = Color.Crimson;
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            exitButton.ForeColor = Color.White;
            exitButton.Location = new Point(50, 240);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(200, 50);
            exitButton.TabIndex = 3;
            exitButton.TabStop = false;
            exitButton.Text = "Выйти";
            exitButton.UseVisualStyleBackColor = false;
            exitButton.Click += ExitButton_Click;
            // 
            // settingsPanel
            // 
            settingsPanel.BackColor = Color.FromArgb(180, 30, 30, 30);
            settingsPanel.Controls.Add(editButton);
            settingsPanel.Controls.Add(resetButton);
            settingsPanel.Controls.Add(toggleAnimationsButton);
            settingsPanel.Controls.Add(volumeButton);
            settingsPanel.Controls.Add(filterButton);
            settingsPanel.Controls.Add(backFromSettingsButton);
            settingsPanel.Location = new Point(372, 200);
            settingsPanel.Name = "settingsPanel";
            settingsPanel.Size = new Size(300, 450);
            settingsPanel.TabIndex = 2;
            settingsPanel.Visible = false;
            // 
            // editButton
            // 
            editButton.BackColor = Color.Gray;
            editButton.FlatAppearance.BorderSize = 0;
            editButton.FlatStyle = FlatStyle.Flat;
            editButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            editButton.ForeColor = Color.White;
            editButton.Location = new Point(50, 30);
            editButton.Name = "editButton";
            editButton.Size = new Size(200, 50);
            editButton.TabIndex = 0;
            editButton.TabStop = false;
            editButton.Text = "Редактировать сцену";
            editButton.UseVisualStyleBackColor = false;
            editButton.Click += EditButton_Click;
            // 
            // resetButton
            // 
            resetButton.BackColor = Color.Gray;
            resetButton.FlatAppearance.BorderSize = 0;
            resetButton.FlatStyle = FlatStyle.Flat;
            resetButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            resetButton.ForeColor = Color.White;
            resetButton.Location = new Point(50, 100);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(200, 50);
            resetButton.TabIndex = 1;
            resetButton.TabStop = false;
            resetButton.Text = "Сбросить сцену";
            resetButton.UseVisualStyleBackColor = false;
            resetButton.Click += ResetButton_Click;
            // 
            // toggleAnimationsButton
            // 
            toggleAnimationsButton.BackColor = Color.Gray;
            toggleAnimationsButton.FlatAppearance.BorderSize = 0;
            toggleAnimationsButton.FlatStyle = FlatStyle.Flat;
            toggleAnimationsButton.Font = new Font("Segoe UI", 12F);
            toggleAnimationsButton.ForeColor = Color.White;
            toggleAnimationsButton.Location = new Point(50, 170);
            toggleAnimationsButton.Name = "toggleAnimationsButton";
            toggleAnimationsButton.Size = new Size(200, 50);
            toggleAnimationsButton.TabIndex = 2;
            toggleAnimationsButton.TabStop = false;
            toggleAnimationsButton.Text = "Анимации: вкл";
            toggleAnimationsButton.UseVisualStyleBackColor = false;
            toggleAnimationsButton.Click += ToggleAnimationsButton_Click;
            // 
            // volumeButton
            // 
            volumeButton.BackColor = Color.Gray;
            volumeButton.FlatAppearance.BorderSize = 0;
            volumeButton.FlatStyle = FlatStyle.Flat;
            volumeButton.Font = new Font("Segoe UI", 12F);
            volumeButton.ForeColor = Color.White;
            volumeButton.Location = new Point(50, 240);
            volumeButton.Name = "volumeButton";
            volumeButton.Size = new Size(200, 50);
            volumeButton.TabIndex = 3;
            volumeButton.TabStop = false;
            volumeButton.Text = "Громкость: 100%";
            volumeButton.UseVisualStyleBackColor = false;
            volumeButton.Click += VolumeButton_Click;
            // 
            // filterButton
            // 
            filterButton.BackColor = Color.Gray;
            filterButton.FlatAppearance.BorderSize = 0;
            filterButton.FlatStyle = FlatStyle.Flat;
            filterButton.Font = new Font("Segoe UI", 12F);
            filterButton.ForeColor = Color.White;
            filterButton.Location = new Point(50, 310);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(200, 50);
            filterButton.TabIndex = 4;
            filterButton.TabStop = false;
            filterButton.Text = "Фильтр: нет";
            filterButton.UseVisualStyleBackColor = false;
            filterButton.Click += FilterButton_Click;
            // 
            // backFromSettingsButton
            // 
            backFromSettingsButton.BackColor = Color.Gray;
            backFromSettingsButton.FlatAppearance.BorderSize = 0;
            backFromSettingsButton.FlatStyle = FlatStyle.Flat;
            backFromSettingsButton.Font = new Font("Segoe UI", 12F);
            backFromSettingsButton.ForeColor = Color.White;
            backFromSettingsButton.Location = new Point(50, 380);
            backFromSettingsButton.Name = "backFromSettingsButton";
            backFromSettingsButton.Size = new Size(200, 50);
            backFromSettingsButton.TabIndex = 5;
            backFromSettingsButton.TabStop = false;
            backFromSettingsButton.Text = "Назад";
            backFromSettingsButton.UseVisualStyleBackColor = false;
            backFromSettingsButton.Click += BackFromSettingsButton_Click;
            // 
            // aboutPanel
            // 
            aboutPanel.BackColor = Color.FromArgb(180, 30, 30, 30);
            aboutPanel.Controls.Add(aboutLabel);
            aboutPanel.Controls.Add(backFromAboutButton);
            aboutPanel.Location = new Point(372, 200);
            aboutPanel.Name = "aboutPanel";
            aboutPanel.Size = new Size(300, 396);
            aboutPanel.TabIndex = 3;
            aboutPanel.Visible = false;
            // 
            // aboutLabel
            // 
            aboutLabel.BackColor = Color.Transparent;
            aboutLabel.Font = new Font("Segoe UI", 10F);
            aboutLabel.ForeColor = Color.White;
            aboutLabel.Location = new Point(10, 10);
            aboutLabel.Name = "aboutLabel";
            aboutLabel.Size = new Size(280, 280);
            aboutLabel.TabIndex = 0;
            aboutLabel.Text = "Игра 'Загадка' — бесконечный раннер, в котором вы управляете группой персонажей, прыгающих через препятствия и собирающих монеты.\r\n\r\nРазработчик: Шутова Таисия, группа ПРИ-122\r\n\r\nУправление:\r\n- Пробел: прыжок\r\n- Q/E: вращение выбранного персонажа\r\n- Ctrl+Tab: переключение персонажа\r\n- Ctrl+Стрелки: перемещение камеры\r\n- Мышь (правая кнопка + движение): вращение камеры\r\n- Колесо мыши: масштабирование\r\n- R: сброс камеры\r\n- Esc: меню/пауза";
            // 
            // backFromAboutButton
            // 
            backFromAboutButton.BackColor = Color.Gray;
            backFromAboutButton.FlatAppearance.BorderSize = 0;
            backFromAboutButton.FlatStyle = FlatStyle.Flat;
            backFromAboutButton.Font = new Font("Segoe UI", 12F);
            backFromAboutButton.ForeColor = Color.White;
            backFromAboutButton.Location = new Point(50, 310);
            backFromAboutButton.Name = "backFromAboutButton";
            backFromAboutButton.Size = new Size(200, 50);
            backFromAboutButton.TabIndex = 0;
            backFromAboutButton.TabStop = false;
            backFromAboutButton.Text = "Назад";
            backFromAboutButton.UseVisualStyleBackColor = false;
            backFromAboutButton.Click += BackFromAboutButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1036, 708);
            Controls.Add(mainMenuPanel);
            Controls.Add(settingsPanel);
            Controls.Add(aboutPanel);
            Controls.Add(GLCtrl);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Загадка";
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            mainMenuPanel.ResumeLayout(false);
            settingsPanel.ResumeLayout(false);
            aboutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private OpenTK.GLControl.GLControl GLCtrl;
        private System.Windows.Forms.Timer jumpTimer;
        private System.Windows.Forms.Timer obstacleTimer;
        private Panel mainMenuPanel;
        private Button startButton;
        private Button exitButton;
        private Button settingsButton;
        private Button aboutButton;
        private Panel settingsPanel;
        private Button editButton;
        private Button resetButton;
        private Button toggleAnimationsButton;
        private Button volumeButton;
        private Button backFromSettingsButton;
        private Button filterButton;
        private Panel aboutPanel;
        private Label aboutLabel;
        private Button backFromAboutButton;
    }
}