using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTest1
{
    class Ball
    {
        public PointF position;
        float xvel, yvel;
        float radius;
        public Brush brush;

        public Ball(int gameWidth, int gameHeight, Random r)
        {
            position.X = r.Next(gameWidth) + 10;
            position.Y = r.Next(gameHeight + 40) - 10;

            xvel = r.Next(2) + 1;
            yvel = r.Next(2) + 1;

            // Set ball radius
            radius = 5;

            // Generate balls with random color
            //brush = new SolidBrush(Color.FromArgb(r.Next(127)+127,r.Next(127)+127,r.Next(127)+127));
            brush = new SolidBrush(Color.Red);
        }

        public void Move(int gameWidth, int gameHeight)
        {
            if (position.X - radius < 0 || position.X + radius > gameWidth)
            {
                xvel = -xvel;
            }
            if (position.Y - radius < 25 || position.Y + radius > gameHeight)
            {
                yvel = -yvel;
            }
           
            position.X += xvel;
            position.Y += yvel;
        }

        public void Draw(Graphics g)
        {
            
            g.FillEllipse(brush, new RectangleF(position.X - radius, position.Y - radius, radius * 2, radius * 2));
        }

        public bool colliding(Point ball)
        {
            float xd = position.X - ball.X;
            float yd = position.Y - ball.Y;

            float sumRadius = radius + radius;
            float sqrRadius = sumRadius * sumRadius;

            float distSqr = (xd * xd) + (yd * yd);
            
            if (distSqr <= sqrRadius)
            {
                //miss
                return true;
            }
            //collide
            return false;
        }

    }
}
