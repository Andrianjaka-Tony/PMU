namespace PMU;

public partial class Form1 : Form
{   
    Piste p = new Piste();
    public Form1()
    {
        InitializeComponent();
        this.Controls.Add(p);
    }
}
