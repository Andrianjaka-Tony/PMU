using System.Drawing.Drawing2D;

namespace PMU;

public partial class Piste : Panel {
    private System.Windows.Forms.Timer? timer; 
    public float tour = 2180;
    public float nbTours = 3;
    public float total;
    public float portion = 70;
    public float time;
    Cheval [] chevaux;
    public Boolean isFinished;
    public int J1 = 0;
    public int J2 = 1;
    public float mise = 5;
    public string result = "";
    public Piste() {
        this.Size = new System.Drawing.Size(1000, 505);
        this.Location = new Point(0, 0);
        this.BorderStyle = BorderStyle.FixedSingle;
        this.BackColor = Color.White;

        this.total = this.tour * this.nbTours;

        this.chevaux = new Cheval[5];
        chevaux[0] = new Cheval(225, 60, 5, -10, Color.DarkViolet);
        chevaux[1] = new Cheval(225, 60, 5, 0, Color.Orange);
        chevaux[2] = new Cheval(225, 60, 5, 10, Color.Blue);
        chevaux[3] = new Cheval(225, 60, 5, 20, Color.Black);
        chevaux[4] = new Cheval(225, 60, 5, -20, Color.Brown);

        this.InitTimer();
    }

    public void PaintTimer(PaintEventArgs e) {
        string value = (this.time / 60) + "";
        string print = "";
        for (int i = 0; i < value.Length; i ++) {
            print += value[i];
            if (i == 3) break;
        }
        String text = print + "";
        Font font = new Font("Poppins", 20);
        Brush brush = new SolidBrush(Color.Black);
        PointF point = new PointF(this.Width / 2 - 10, this.Height / 2 - 10);
        e.Graphics.DrawString(text, font, brush, point);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        GraphicsPath path = new GraphicsPath();
        
        path.StartFigure();
        path.AddArc(new Rectangle(0, 0, 350, 500), 90, 180);
        path.AddArc(new Rectangle(600, 0, 350, 500), 270, 180);
        path.CloseFigure();

        GraphicsPath path1 = new GraphicsPath();
        
        path1.StartFigure();
        path1.AddArc(new Rectangle(100, 80, 250, 350), 90, 180);
        path1.AddArc(new Rectangle(600, 80, 250, 350), 270, 180);
        path1.CloseFigure();

        e.Graphics.DrawPath(new Pen(Color.Black, 3), path);
        e.Graphics.DrawPath(new Pen(Color.Black, 3), path1);
        e.Graphics.DrawLine(new Pen(Color.White), new Point(225 , 90), new Point(725, 90));
        e.Graphics.DrawLine(new Pen(Color.White), new Point(100 , 255), new Point(850, 255));
        e.Graphics.DrawLine(new Pen(Color.White), new Point(225 , 420), new Point(725, 420));

        this.PaintTimer(e);
        this.paintResult(e);


        foreach (Cheval c in this.chevaux) c.draw(e);
    }

    public void InitTimer() {
        this.timer = new System.Windows.Forms.Timer();
        this.timer.Interval = 1;
        this.timer.Tick += Timer_tick;
        this.DoubleBuffered = true;
        this.timer.Start();
    }

    public void finished() {
        int count = 0;
        foreach (Cheval cheval in this.chevaux) {
            if (cheval.isArrived) {
                count ++;
            }
        }
        if (count == this.chevaux.Count()) {
            isFinished = true;
        }
    }

    public string afficherResultat() {
        string reponse = "";
        Array.Sort(this.chevaux, new ChevalTotalTimeComparer());
        foreach (Cheval cheval in this.chevaux) {
            float differenceTemps = cheval.totalTime - this.chevaux[0].totalTime;
            int prix = ((int) differenceTemps) * (int)this.mise;

            int minute = (int) (cheval.totalTime / 60);
            float secondes = cheval.totalTime - (float) (minute * 60);

            reponse += "Temps: " + minute + " : " + secondes + ", Couleur: " + cheval.color.Name + ", Difference: " + differenceTemps + "\n"; 
        }
        this.result = reponse;
        return reponse;
    }

    public void paintResult(PaintEventArgs e) {
        Font font = new Font("Poppins", 12);
        Brush brush = new SolidBrush(Color.Black);
        PointF point = new PointF(120 , 120);
        e.Graphics.DrawString(this.result, font, brush, point);
    }

    public void Timer_tick(Object sender, EventArgs e) {
        if(sender != null) {
            foreach (Cheval c in this.chevaux) {
                c.move((this.total * this.portion) / 100, this.total);
                this.finished();
            }
            if (this.isFinished) {
                timer.Stop();
                this.afficherResultat();
                this.Save();
            }
            this.time ++;
        }
        this.Invalidate();
        Refresh();
    }

    public void Save() {
        float tJ1 = this.chevaux[J1].totalTime;
        float tJ2 = this.chevaux[J2].totalTime;
        float difference = Math.Abs(tJ1 - tJ2);
        difference *= (float) 1000;
        difference /= 500;
        difference = (int) difference;

        string s1 = tJ1 + "";
        s1 = s1.Replace(",", ".");
        string s2 = tJ2 + "";
        s2 = s2.Replace(",", ".");
        string query = "INSERT into course (idJ1, idJ2, tempsJ1, tempsJ2, gain) VALUES (1, 2, " + s1 + ", " + s2 + ", " + (difference * this.mise) + ")";
        Connect.Insert(query);
    }
}