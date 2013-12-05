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
    class PlayerBall
    {
        private static Point _userBall;
        public PointF position;
        float radius = 0;
        private Brush brush;

        // NavBall constructor
        public PlayerBall(Point userBallPos)
        {
            _userBall.X = userBallPos.X;
            _userBall.Y = userBallPos.Y;

            // Player ball radius
            radius = 7;

            // Change player ball color
            brush = new SolidBrush(Color.PowderBlue);
        }

        public void NavigateBall(Graphics g, Point local)
        {
            // Move the ball with our cursor. WITHOUT CLICKING
            g.FillEllipse(brush, new RectangleF(local.X - radius, local.Y - radius, radius * 2, radius * 2));
        }

        public bool colliding(List<Ball> userBall, Point local)
        {
            float xd = local.X - userBall[8].position.X;
            float yd = local.Y - userBall[9].position.Y;

            float sumRadius = 20 + 20;
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
