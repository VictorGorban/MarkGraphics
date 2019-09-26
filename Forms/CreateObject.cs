using System.Drawing;
using System.Windows.Forms;

namespace Game_Consoles
{
    public partial class CreateObject : Form
    {
        private MainForm mainForm = null;

        public CreateObject(MainForm mainForm)
        {
            this.mainForm = mainForm;

            InitializeComponent();

            PositionX.Minimum = PositionY.Minimum = PositionZ.Minimum = mainForm.MinCoordinate;
            PositionX.Maximum = PositionY.Maximum = PositionZ.Maximum = mainForm.MaxCoordinate;

            AddObject.Enabled = false;
        }


        private void ObjectName_TextChanged(object sender, System.EventArgs e)
        {
            AddObject.Enabled = ObjectName.TextLength > 0;
        }

        private void AddObject_Click(object sender, System.EventArgs e)
        {
            Point3D basePoint = new Point3D(
                (double)PositionX.Value,
                (double)PositionY.Value,
                (double)PositionZ.Value
                );

            if (mainForm.Scene.GetObjectByName(ObjectName.Text) == null)
            {
                var radio = new RadioObject(ObjectName.Text);
                radio.BasePoint = basePoint;
                radio.AngleX = (int)RotateX.Value;
                radio.AngleY = (int)RotateY.Value;
                radio.AngleZ = (int)RotateZ.Value;
                radio.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);


                mainForm.Scene.AddObject(radio);
                int index = mainForm.ObjectsList.Items.Add(ObjectName.Text);
                mainForm.ObjectsList.SelectedIndex = index;
                mainForm.ObjectPanButton.Enabled = true;
                Close();
            }
            else
            {
                MessageBox.Show("Объект с таким именем уже существует, переименуйте объект.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ObjectName.Clear();
                ObjectName.Focus();
            }
        }
    }
}
