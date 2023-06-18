using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace PMU;

public partial class Cheval : Panel {
    public float x;
    public float y;
    public float width = 20;
    public float height = 20;
    public float speed;
    public float endurance;
    public float angle = 0;
    public float distance = 0;
    public float time = 0;
    public float totalTime = 0;
    public Color color;
    public Boolean isArrived = false;

    public Cheval(int x, int y, float speed, float endurance, Color color) {
        this.x = x;
        this.y = y;
        this.speed = speed;
        this.endurance = endurance;
        this.color = color;
    }

    public void draw(PaintEventArgs e) {
        Brush brush = new SolidBrush(this.color);
        Rectangle rect = new Rectangle((int)this.x, (int)this.y, (int)this.width, (int)this.height);
        e.Graphics.FillEllipse(brush, rect);
    }

    public void move(float portion, float finished) {
        if (this.distance >= finished) {
            this.totalTime = this.time / 60;
            this.isArrived = true;
            return;
        }
        this.time ++;
        float incrementation = this.speed;
        if (this.distance >= portion) {
            incrementation += this.speed * this.endurance / 100;
        }
        if(this.x < 195 || this.x >= 705) {
            this.moveSpin(incrementation);
        } else {
            this.moveLine(incrementation);
        }
    }

    public void moveSpin(float incrementation) {
        this.x += (incrementation / 2) * (float) (Math.Cos(angle) * 2);
        this.y += (incrementation / 2) * (float) (Math.Sin(angle) * 2);
        this.angle += (incrementation / 2) * 1 * (float)Math.PI / 290;
        this.distance += (float) Math.Sqrt(Math.Pow((incrementation / 2) * (float) (Math.Cos(angle) * 2), 2) + Math.Pow((incrementation / 2) * (float) (Math.Sin(angle) * 2), 2));
    }

    public void moveLine(float incrementation) {
        this.x += incrementation * (float) Math.Cos(angle);
        this.distance += incrementation;
    }

}

public class ChevalTotalTimeComparer : IComparer<Cheval>
{
    public int Compare(Cheval x, Cheval y)
    {
        if (x == null || y == null)
            throw new System.ArgumentNullException();

        return x.totalTime.CompareTo(y.totalTime);
    }
}

