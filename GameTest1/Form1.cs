using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

using Timer = System.Windows.Forms.Timer;

namespace GameTest1
{
    public partial class Form1 : Form
    {

        List<Ball> balls;
        const int fps = 60;
        private Point local; // New point variable for mouse
        private Boolean _isStop = false;
        private Timer GameTimer;
        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private Boolean _STOP = false;
        private Boolean playSound = true;
        private SoundPlayer sound;
        private SoundPlayer soundGameOver;

        public Form1()
        {
            InitializeComponent();

            InitializeGame();
            var task = new Task(Run);
            task.Start();
        }

        protected void InitializeGame()
        {
            // SOLVE SOME THREADING ERROR
            Control.CheckForIllegalCrossThreadCalls = false;

            balls = new List<Ball>();
            Random r = new Random();
            label4.Visible = false;

            for (int i = 0; i < 120; i++)
            {
                balls.Add(new Ball(Width, Height, r));
            }

            _STOP = true;
            _isStop = false;

            if (playSound)
            {
                sound = new SoundPlayer(Properties.Resources.song1);
                sound.PlayLooping();
            }
        }

        protected void Run()
        {
            while (!_isStop)
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    balls[i].Move(this.ClientSize.Width, this.ClientSize.Height);
                    if (balls[i].colliding(local))
                    {
                        // If player ball hit other balls
                        // Stop the timer and the game when ball hits
                        HandleGameOver();
                        _STOP = false;
                        //break;
                    }
                }
                this.Invalidate();
                Thread.Sleep(1000 / fps);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            this.local = PointToClient(Cursor.Position);

            PlayerBall player = new PlayerBall(local);

            // Make the ball smooth!
            g.SmoothingMode = SmoothingMode.AntiAlias;

            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].Draw(g);
            }
            // This function lets user navigate the ball
            player.NavigateBall(g, local);
        }

        public void HandleGameOver()
        {
            // Stop game and then check score
            // Break the thread to jump out of loop
            label4.Visible = true;
            _isStop = true;
            _STOP = true;
            GameTimer.Stop();

            if (playSound)
            {
                soundGameOver = new SoundPlayer(Properties.Resources.beep);
                soundGameOver.Play();
            }

            CheckHighscore();
        }

        // Set up a timer and fire the Tick event once per second (1000 ms)
        private void SetTimer()
        {
            GameTimer = new Timer();
            // Set the timer ticks to milisecond (10) or second(1000)
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameTimer_Tick);
            // Assign time when mouse enter form
            _startTime = DateTime.Now;
        }

        // Game start once the mouse enters
        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            // IF game is running 
            if (!_isStop)
            {
                label3.Visible = false;
                label2.Visible = false;
                //label4.Visible = false;
                // Set the timer
                SetTimer();
                // and, start the timer
                GameTimer.Start();
            }
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            // Stop timer when mouse leave the form
            if (!_isStop && _STOP) // true == stop
            {
                HandleGameOver();
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _currentElapsedTime = DateTime.Now - _startTime;

            // Print the timer to label with Minutes:Second:Milisecond format
            label1.Text = _currentElapsedTime.ToString(@"mm\:ss\.fff");
        }

        protected void CheckHighscore()
        {
            HighScore test = new HighScore(this);
            InputName userName = new InputName(this);
            List<NameAndScore> abc = test.GetScoreList();

            // Check with the highest three score
            if (abc[2].Score < _currentElapsedTime)
            {
                // Open new input name form
                userName.ShowDialog();
                // Pass the scores and name to be saved into XML
                test.AppendScore(_currentElapsedTime, userName.GetName());
            }
        }

        private void restartToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InitializeGame();
            SetTimer();
            var task = new Task(Run);
            task.Start();
        }

        private void HighScoreTollStripMenu_Click(object sender, EventArgs e)
        {
            // Show form when clicked
            if (!_isStop)
            {
                _STOP = true;
                _isStop = true;
            }
            HighScore high = new HighScore(this);
            high.Show();
        }

        private void soundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playSound)
            {
                this.sound.Stop();
                this.soundGameOver.Stop();
                playSound = false;
                soundToolStripMenuItem.Text = "Sound On";
            }
            else
            {
                //this.sound.Play();
                playSound = true;
                soundToolStripMenuItem.Text = "Sound Off";
            }            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }        
    }
}
