namespace SnakeWinForms;

public static class ControlsManager
{
    public static Color BackgroundColor { get; set; } = Color.FromArgb(40, 44, 52);
    public static Color TextColor { get; set; } = Color.White;
    public static Label DockedLabel => (Label)SetupControl(new Label());
    public static Panel Placeholder => new Panel() { BackColor = Color.Transparent };
    public static Button DockedButton => (Button)SetupControl(new Button());
    public static TrackBar TrackBar => (TrackBar)SetupControl(new TrackBar());
    public static TableLayoutPanel TableLayoutPanel => (TableLayoutPanel)SetupControl(new TableLayoutPanel());
    public static NumericUpDown NumericUpDown => (NumericUpDown)SetupControl(new NumericUpDown());
    public static Panel Panel => (Panel)SetupControl(new Panel());
    public static CheckBox CheckBox
    {
        get
        {
            var checkBox = (CheckBox)SetupControl(new CheckBox());
            checkBox.CheckAlign = ContentAlignment.MiddleCenter;
            return checkBox;
        }
    }
    public static ComboBox ComboBox
    {
        get
        {
            var comboBox = (ComboBox)SetupControl(new ComboBox());  
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            return comboBox;
        }
    }
    public static PictureBox PictureBox
    {
        get
        {
            var pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            return pictureBox;
        }
    }
    public static Label Title 
    {
        get
        {
            var label = new Label();
            label.ForeColor = TextColor;
            label.BackColor = BackgroundColor;
            label.TextAlign = ContentAlignment.MiddleCenter;
            return label;
        }
    }
    public static Button Button
    {
        get
        {
            var button = new Button();
            button.ForeColor = TextColor;
            button.BackColor = BackgroundColor;
            button.TextAlign = ContentAlignment.MiddleCenter;
            return button;
        }
    }

    public static Control SetupControl(Control control)
    {
        control.Dock = DockStyle.Fill;
        control.BackColor = BackgroundColor;
        control.ForeColor = TextColor;
        return control;
    }
}
