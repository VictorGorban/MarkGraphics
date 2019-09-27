using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Telescopes
{
    public partial class MainForm : Form
    {
        public long MinCoordinate = long.MinValue;
        public long MaxCoordinate = long.MaxValue;

        public Scene Scene = null;

        public bool IsPan = false;
        private Camera tempCamera = null;

        public TelescopeObject currentObject = null;

        public MainForm()
        {
            InitializeComponent();

            Scene = new Scene(Canvas);

            IsUpdating = true;

            FOV.Minimum = (int)Camera.MIN_FOV;
            FOV.Maximum = (int)Camera.MAX_FOV;
            CameraPitch.Minimum = (int)Camera.MIN_THETA;
            CameraPitch.Maximum = (int)Camera.MAX_THETA;

            NearZ.Maximum = FarZ.Maximum = MaxCoordinate;

            CameraTargetX.Minimum = CameraTargetY.Minimum = CameraTargetZ.Minimum = MinCoordinate;
            CameraPositionX.Minimum = CameraPositionY.Minimum = CameraPositionZ.Minimum = MinCoordinate;

            CameraTargetX.Maximum = CameraTargetY.Maximum = CameraTargetZ.Maximum = MaxCoordinate;
            CameraPositionX.Maximum = CameraPositionY.Maximum = CameraPositionZ.Maximum = MaxCoordinate;

            ProjectionType.SelectedIndex = 0;
            RotationType.SelectedIndex = 0;

            ViewType.SelectedIndex = 0;

            PositionX.Minimum = PositionY.Minimum = PositionZ.Minimum = MinCoordinate;
            PositionX.Maximum = PositionY.Maximum = PositionZ.Maximum = MaxCoordinate;

            LightPositionX.Value = (decimal)Scene.Light.X;
            LightPositionY.Value = (decimal)Scene.Light.Y;
            LightPositionZ.Value = (decimal)Scene.Light.Z;

            IsUpdating = false;

        }

        private void ResetCamera_Click(object sender, EventArgs e)
        {
            Scene.ResetCamera();
            ProjectionType.SelectedIndex = 0;
            UpdateCameraValues();
        }

        private bool IsUpdating = false;

        private void UpdateCameraValues()
        {
            IsUpdating = true;

            CameraTargetX.Value = (decimal)Scene.Camera.Target.X;
            CameraTargetY.Value = (decimal)Scene.Camera.Target.Y;
            CameraTargetZ.Value = (decimal)Scene.Camera.Target.Z;

            CameraPositionX.Value = (decimal)Scene.Camera.Position.X;
            CameraPositionY.Value = (decimal)Scene.Camera.Position.Y;
            CameraPositionZ.Value = (decimal)Scene.Camera.Position.Z;

            if (Scene.Camera.AnglePhi >= CameraYaw.Minimum && Scene.Camera.AnglePhi <= CameraYaw.Maximum)
            {
                CameraYaw.Value = Scene.Camera.AnglePhi;
            }
            if (Scene.Camera.AngleTheta >= CameraPitch.Minimum && Scene.Camera.AngleTheta <= CameraPitch.Maximum)
            {
                CameraPitch.Value = Scene.Camera.AngleTheta;
            }

            FOV.Value = (int)Scene.Camera.FOV;

            NearZ.Value = (decimal)Scene.Camera.NearClipZ;
            FarZ.Value = (decimal)Scene.Camera.FarClipZ;

            ProjectionType.SelectedIndex = Scene.Camera.IsCentralProjection ? 0 : 1;

            IsUpdating = false;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (Scene != null)
            {
                if (IsPan)
                {
                    Scene.Camera.FlyCameraLeftRight(-1);
                }
                Scene.DrawObjects();

                //UpdateParametersValues();
            }
        }

        private bool IsCanvasAction = false;
        private bool LeftMouse = false;
        private bool RightMouse = false;
        private Point start;

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            IsCanvasAction = true;
            if (e.Button == MouseButtons.Left && !RightMouse)
            {
                LeftMouse = true;
            }
            else if (e.Button == MouseButtons.Right && !LeftMouse)
            {
                RightMouse = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsCanvasAction)
            {
                int dx = e.Location.X - start.X;
                int dy = e.Location.Y - start.Y;

                if (LeftMouse)
                {
                    if (RotationType.SelectedIndex == 0)
                    {
                        Scene.Camera.FlyCameraLeftRight(-dx);
                        Scene.Camera.FlyCameraUpDown(-dy);
                    }
                    else
                    {
                        Scene.Camera.HeadCameraLeftRight(-dx);
                        Scene.Camera.HeadCameraUpDown(dy * 25);
                    }
                }
                else if (RightMouse)
                {
                    Scene.Camera.MoveCameraLeftRight(dx);
                    Scene.Camera.MoveCameraUpDown(-dy);
                }

                start = e.Location;

                UpdateCameraValues();
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsCanvasAction)
            {
                IsCanvasAction = false;
            }
            if (e.Button == MouseButtons.Left)
            {
                LeftMouse = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                RightMouse = false;
            }
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                RotationType.SelectedIndex = RotationType.SelectedIndex == 0 ? 1 : 0;
            }
        }

        private void ProjectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Scene.Camera.IsCentralProjection = ProjectionType.SelectedIndex == 0;
            FOV.Enabled = ProjectionType.SelectedIndex == 0;
        }

        private void CoordinateBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar == '-')
                {
                    if (box.TextLength > 0 && (box.SelectionStart != 0 || box.SelectionLength == 0 && box.Text[0] == '-'))
                    {
                        e.Handled = true;
                    }
                }
                else if (char.IsDigit(e.KeyChar))
                {
                    if (box.TextLength > 0 && box.SelectionStart > 0 && box.Text[0] == '0')
                    {
                        int start = box.SelectionStart;
                        box.Text = box.Text.Substring(1);
                        box.SelectionStart = start - 1;
                    }
                    else if (box.TextLength > 1 && box.SelectionStart > 1 && box.Text[0] == '-' && box.Text[1] == '0')
                    {
                        int start = box.SelectionStart;
                        box.Text = "-" + box.Text.Substring(2);
                        box.SelectionStart = start - 1;
                    }
                }
            }
        }

        private void CoordinateBox_Leave(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            int value = 0;
            if (!int.TryParse(box.Text, out value))
            {
                box.Text = "0";
            }
        }

        private void ViewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Scene.Mode = (Scene.MODE)ViewType.SelectedIndex;
        }

        private void ObjectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentObject = Scene.GetObjectByName(ObjectsList.SelectedItem.ToString());

            UpdateCameraValues();
            ObjectPanButton.Enabled = true;
            DeleteObject.Enabled = true;
            ObjectSpacePanel.Enabled = true;
            ObjectSpacePanel.Visible = true;

            // currObj может быть null, если нет объектов
            if (currentObject is null)
            {
                ObjectViewPanel.Visible = ObjectViewPanel.Enabled = false;
            }
            else
            {
                UpdateParametersValues();
                UpdateSpaceValues();
            }
        }

        private void UpdateSpaceValues()
        {
            //position
            PositionX.Value = (decimal)currentObject.BasePoint.X;
            PositionY.Value = (decimal)currentObject.BasePoint.Y;
            PositionZ.Value = (decimal)currentObject.BasePoint.Z;

            // rotation
            RotateX.Value = (decimal)currentObject.AngleX;
            RotateY.Value = (decimal)currentObject.AngleY;
            RotateZ.Value = (decimal)currentObject.AngleZ;

            //scale 
            ScaleX.Value = (decimal)currentObject.ScaleX;
            ScaleY.Value = (decimal)currentObject.ScaleY;
            ScaleZ.Value = (decimal)currentObject.ScaleZ;

        }

        private void UpdateParametersValues()
        {
            RightVolumeControllerStandRadius.Value = (decimal)currentObject.Basis1CylinderRadius;
            RightVolumeControllerWidestPartRadius.Value = (decimal)currentObject.PrimaryLegsLength;
            RightVolumeControllerStripeRadius.Value = (decimal)currentObject.Basis2CylinderRadius;
            LeftVolumeControllerStandRadius.Value = (decimal)currentObject.Basis3CylinderRadius;
            LeftVolumeControllerWidestPartRadius.Value = (decimal)currentObject.LenseRadius;
            LeftVolumeControllerStripeRadius.Value = (decimal)currentObject.SecondaryLegsLength;
            ControlButtonsHeight.Value = (decimal)currentObject.PrimaryLegsCount;
            ControlButtonsWidth.Value = (decimal)currentObject.SecondaryLegsCount;
            SphereCount.Value = currentObject.HandsCount;
            PlaybackButtonsCount.Value = currentObject.HandsRadius;

            ObjectViewPanel.Visible = ObjectViewPanel.Enabled = true;
        }


        private void CameraYaw_ValueChanged(object sender, EventArgs e)
        {
            if (!IsCanvasAction && !IsUpdating)
            {
                int dx = (int)CameraYaw.Value - Scene.Camera.AnglePhi;
                if (RotationType.SelectedIndex == 0)
                {
                    Scene.Camera.FlyCameraLeftRight(dx);
                }
                else
                {
                    Scene.Camera.HeadCameraLeftRight(dx);
                }
            }
        }

        private void CameraPitch_ValueChanged(object sender, EventArgs e)
        {
            if (!IsCanvasAction && !IsUpdating)
            {
                int dy = (int)CameraPitch.Value - Scene.Camera.AngleTheta;

                if (RotationType.SelectedIndex == 0)
                {
                    Scene.Camera.FlyCameraUpDown(-dy);
                }
                else
                {
                    Scene.Camera.HeadCameraUpDown(-dy);
                }
            }
        }

        private void FOV_ValueChanged(object sender, EventArgs e)
        {
            if (!IsCanvasAction && !IsUpdating)
            {
                Scene.Camera.FOV = (int)FOV.Value;
            }
        }

        private void PositionX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.BasePoint.X = (double)PositionX.Value;
            }
        }

        private void PositionY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.BasePoint.Y = (double)PositionY.Value;
            }
        }

        private void PositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.BasePoint.Z = (double)PositionZ.Value;
            }
        }

        private void RotateX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.AngleX = (int)RotateX.Value;
            }
        }

        private void RotateY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.AngleY = (int)RotateY.Value;
            }
        }

        private void RotateZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.AngleZ = (int)RotateZ.Value;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                MainTimer.Enabled = false;
            }
            else
            {
                MainTimer.Enabled = true;
                if (Scene != null)
                {
                    Scene.Camera.setSize(Canvas.ClientSize.Width, Canvas.ClientSize.Height);
                    Scene.DrawObjects();
                }
            }
        }

        private void RotationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RotationType.SelectedIndex == 0)
            {
                Scene.Camera.FlyCameraLeftRight(0);
                Scene.Camera.FlyCameraUpDown(0);
            }
            else
            {
                Scene.Camera.HeadCameraLeftRight(0);
                Scene.Camera.HeadCameraUpDown(0);
            }
            UpdateCameraValues();
        }

        private void AddObject_Click(object sender, EventArgs e)
        {
            if (Scene.Objects.Count >= 3)
            {
                MessageBox.Show("Не могу создать объект, уже достигнуто макс. кол-во");
                return;
            }
            CreateObject form = new CreateObject(this);
            form.ShowDialog();
        }

        private void DeleteObject_Click(object sender, EventArgs e)
        {
            Scene.Objects.Remove(currentObject);
            int index = ObjectsList.SelectedIndex;
            ObjectsList.Items.RemoveAt(index);
            ObjectsList.SelectedIndex = index - 1;
            if (ObjectsList.SelectedIndex == -1 && ObjectsList.Items.Count > 0)
            {
                ObjectsList.SelectedIndex = 0;
            }
            if (ObjectsList.Items.Count == 0)
            {
                ObjectPanButton.Enabled = false;
                DeleteObject.Enabled = false;
                ObjectSpacePanel.Enabled = false;
                PositionX.Value = PositionY.Value = PositionZ.Value = 0;
                RotateX.Value = RotateY.Value = RotateZ.Value = 0;
                ScaleX.Value = 1;
                ScaleY.Value = 1;
                ScaleZ.Value = 1;
                ObjectViewPanel.Visible = ObjectViewPanel.Enabled = false;
                ObjectViewPanel.Visible = ObjectViewPanel.Enabled = false;
            }
        }

        private void ObjectPan_Click(object sender, EventArgs e)
        {
            if (!IsPan)
            {
                PrepareFormForPan();

                var target = currentObject.BasePoint;
                var position = new Point3D(currentObject.BasePoint.X,
                    currentObject.BasePoint.Y + currentObject.MaxLength,
                    currentObject.BasePoint.Z + currentObject.MaxLength);

                Scene.Camera.SetPositionAndTarget(position, target);
            }
            else
            {
                UndoPrepareFormForPan();
            }
        }

        private void PrepareFormForPan(bool isScenePan = false)
        {
            if (isScenePan)
            {
                ScenePanButton.Text = "Остановить панораму";
                ObjectPanButton.Enabled = false;
            }
            else
            {
                ObjectPanButton.Text = "Остановить панораму";
                ScenePanButton.Enabled = false;
            }

            ExportPanel.Enabled = false;
            ObjectsPanel.Enabled = false;
            CameraPanel.Enabled = false;
            ObjectSpacePanel.Enabled = false;
            ObjectViewPanel.Enabled = false;
            SceneViewPanel.Enabled = false;
            TransformScenePanel.Enabled = false;
            Canvas.Enabled = false;


            if (!isScenePan)
            {
                Scene.SelectedForPanObject = currentObject;
            }

            Scene.IsPanning = true;
            IsPan = true;

            tempCamera = Scene.Camera;
            Scene.ResetCamera();
        }

        private void UndoPrepareFormForPan(bool isScenePan = false)
        {
            if (isScenePan)
            {
                ScenePanButton.Text = "Панорама";
                ObjectPanButton.Enabled = true;
            }
            else
            {
                ObjectPanButton.Text = "Панорама";
                ScenePanButton.Enabled = true;
            }

            ExportPanel.Enabled = true;
            ObjectsPanel.Enabled = true;
            CameraPanel.Enabled = true;
            ObjectSpacePanel.Enabled = true;
            ObjectViewPanel.Enabled = true;
            SceneViewPanel.Enabled = true;
            TransformScenePanel.Enabled = true;
            Canvas.Enabled = true;


            if (!isScenePan)
            {
                Scene.SelectedForPanObject = null;
            }
            Scene.IsPanning = false;

            IsPan = false;
            Scene.Camera = tempCamera;
        }

        private void PositionX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionX.Value = 0;
            }
        }

        private void PositionY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionY.Value = 0;
            }
        }

        private void PositionZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionZ.Value = 0;
            }
        }

        private void RotateX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RotateX.Value = 0;
            }
        }

        private void RotateY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RotateY.Value = 0;
            }
        }

        private void RotateZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RotateZ.Value = 0;
            }
        }

        private void NearZ_ValueChanged(object sender, EventArgs e)
        {
            Scene.Camera.NearClipZ = (double)NearZ.Value;
        }

        private void FarZ_ValueChanged(object sender, EventArgs e)
        {
            Scene.Camera.FarClipZ = (double)FarZ.Value;
        }

        private void CameraTargetX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                Scene.Camera.Target.Set(
                (double)CameraTargetX.Value,
                (double)CameraTargetY.Value,
                (double)CameraTargetZ.Value
                );
            }
        }

        private void CameraTargetY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                Scene.Camera.Target.Set(
                (double)CameraTargetX.Value,
                (double)CameraTargetY.Value,
                (double)CameraTargetZ.Value
                );
            }
        }

        private void CameraTargetZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                Scene.Camera.Target.Set(
                (double)CameraTargetX.Value,
                (double)CameraTargetY.Value,
                (double)CameraTargetZ.Value
                );
            }
        }

        private void CameraPositionX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                Scene.Camera.Position.Set(
                (double)CameraPositionX.Value,
                (double)CameraPositionY.Value,
                (double)CameraPositionZ.Value
                );
            }
        }

        private void CameraPositionY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                Scene.Camera.Position.Set(
                (double)CameraPositionX.Value,
                (double)CameraPositionY.Value,
                (double)CameraPositionZ.Value
                );
            }
        }

        private void CameraPositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                Scene.Camera.Position.Set(
                (double)CameraPositionX.Value,
                (double)CameraPositionY.Value,
                (double)CameraPositionZ.Value
                );
            }
        }

        private void ScaleX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
            }
        }

        private void ScaleY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
            }
        }

        private void ScaleZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                currentObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
            }
        }

        private void ScaleX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ScaleX.Value = 1;
            }
        }

        private void ScaleY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ScaleY.Value = 1;
            }
        }

        private void ScaleZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ScaleZ.Value = 1;
            }
        }

        private void RightVolumeControllerStandRadius_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)RightVolumeControllerStandRadius.Value;
            if (value != currentObject.Basis1CylinderRadius)
            {
                currentObject.Basis1CylinderRadius = value;
                currentObject.UpdateObject();
            }
        }

        private void ControlButtonsWidth_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)ControlButtonsWidth.Value;
            if (value != currentObject.SecondaryLegsCount)
            {
                currentObject.SecondaryLegsCount = (double)ControlButtonsWidth.Value;
                currentObject.UpdateObject();
            }
        }

        private void RightVolumeControllerWidestPartRadius_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)RightVolumeControllerWidestPartRadius.Value;
            if (value != currentObject.PrimaryLegsLength)
            {
                currentObject.PrimaryLegsLength = (double)RightVolumeControllerWidestPartRadius.Value;
                currentObject.UpdateObject();
            }
        }

        private void RightVolumeControllerStripeRadius_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)RightVolumeControllerStripeRadius.Value;
            if (value != currentObject.Basis2CylinderRadius)
            {
                currentObject.Basis2CylinderRadius = (double)RightVolumeControllerStripeRadius.Value;
                currentObject.UpdateObject();
            }
        }

        private void LeftVolumeControllerStandRadius_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)LeftVolumeControllerStandRadius.Value;
            if (value != currentObject.Basis3CylinderRadius)
            {
                currentObject.Basis3CylinderRadius = (double)LeftVolumeControllerStandRadius.Value;
                currentObject.UpdateObject();
            }
        }

        private void LeftVolumeControllerWidestPartRadius_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)LeftVolumeControllerWidestPartRadius.Value;
            if (value != currentObject.LenseRadius)
            {
                currentObject.LenseRadius = (double)LeftVolumeControllerWidestPartRadius.Value;
                currentObject.UpdateObject();
            }
        }

        private void LeftVolumeControllerStripeRadius_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)LeftVolumeControllerStripeRadius.Value;
            if (value != currentObject.SecondaryLegsLength)
            {
                currentObject.SecondaryLegsLength = (double)LeftVolumeControllerStripeRadius.Value;
                currentObject.UpdateObject();
            }
        }

        private void ControlButtonsHeight_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)ControlButtonsHeight.Value;
            if (value != currentObject.PrimaryLegsCount)
            {
                currentObject.PrimaryLegsCount = (double)ControlButtonsHeight.Value;
                currentObject.UpdateObject();
            }
        }

        private void SphereCount_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)SphereCount.Value;
            if (value != currentObject.HandsCount)
            {
                currentObject.HandsCount = (int)SphereCount.Value;
                currentObject.UpdateObject();
            }
        }

        private void PlayBackButtonsCount_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)PlaybackButtonsCount.Value;
            if (value != currentObject.HandsRadius)
            {
                currentObject.HandsRadius = (int)PlaybackButtonsCount.Value;
                currentObject.UpdateObject();
            };
        }

        // xml запись сцены в файл
        private void ExportScene_Click(object sender, EventArgs e)
        {
            if (ExportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IFormatProvider format = new System.Globalization.CultureInfo("en-us");

                    XDocument document = new XDocument();

                    XElement scene = new XElement("scene");
                    scene.SetAttributeValue("ViewMode", Scene.Mode);

                    var camera = Scene.Camera;
                    XElement cameraElement = new XElement("camera");

                    cameraElement.SetElementValue("central-projection", camera.IsCentralProjection);

                    XElement position = new XElement("position");
                    position.SetAttributeValue("x", string.Format(format, "{0:0.00}", camera.Position.X));
                    position.SetAttributeValue("y", string.Format(format, "{0:0.00}", camera.Position.Y));
                    position.SetAttributeValue("z", string.Format(format, "{0:0.00}", camera.Position.Z));
                    cameraElement.Add(position);

                    XElement target = new XElement("target");
                    target.SetAttributeValue("x", string.Format(format, "{0:0.00}", camera.Target.X));
                    target.SetAttributeValue("y", string.Format(format, "{0:0.00}", camera.Target.Y));
                    target.SetAttributeValue("z", string.Format(format, "{0:0.00}", camera.Target.Z));
                    cameraElement.Add(target);

                    XElement clippingplanes = new XElement("clipping-planes");
                    clippingplanes.SetAttributeValue("near", camera.NearClipZ);
                    clippingplanes.SetAttributeValue("far", camera.FarClipZ);
                    cameraElement.Add(clippingplanes);

                    cameraElement.SetElementValue("fov", camera.FOV);

                    scene.Add(cameraElement);

                    var light = Scene.Light;
                    XElement lightElement = new XElement("light");

                    XElement lightPosition = new XElement("position");
                    lightPosition.SetAttributeValue("x", string.Format(format, "{0:0.00}", light.X));
                    lightPosition.SetAttributeValue("y", string.Format(format, "{0:0.00}", light.Y));
                    lightPosition.SetAttributeValue("z", string.Format(format, "{0:0.00}", light.Z));
                    lightElement.Add(lightPosition);

                    scene.Add(lightElement);

                    XElement objects = new XElement("objects");
                    foreach (TelescopeObject radio in Scene.Objects)
                    {
                        XElement objectelement;

                        objectelement = new XElement("Telescope");

                        objectelement.SetElementValue("Basis1CylinderRadius", string.Format(format, "{0:0.00}", radio.Basis1CylinderRadius));
                        objectelement.SetElementValue("PrimaryLegsLength", string.Format(format, "{0:0.00}", radio.PrimaryLegsLength));
                        objectelement.SetElementValue("Basis2CylinderRadius", string.Format(format, "{0:0.00}", radio.Basis2CylinderRadius));
                        objectelement.SetElementValue("Basis3CylinderRadius", string.Format(format, "{0:0.00}", radio.Basis3CylinderRadius));
                        objectelement.SetElementValue("LenseRadius", string.Format(format, "{0:0.00}", radio.LenseRadius));
                        objectelement.SetElementValue("SecondaryLegsLength", string.Format(format, "{0:0.00}", radio.SecondaryLegsLength));
                        objectelement.SetElementValue("PrimaryLegsCount", string.Format(format, "{0:0.00}", radio.PrimaryLegsCount));
                        objectelement.SetElementValue("SecondaryLegsCount", string.Format(format, "{0:0.00}", radio.SecondaryLegsCount));
                        objectelement.SetElementValue("HandsCount", radio.HandsCount);
                        objectelement.SetElementValue("HandsRadius", radio.HandsRadius);

                        objectelement.SetAttributeValue("name", radio.ObjectName);

                        XElement objectposition = new XElement("position");
                        objectposition.SetAttributeValue("x", string.Format(format, "{0:0.00}", radio.BasePoint.X));
                        objectposition.SetAttributeValue("y", string.Format(format, "{0:0.00}", radio.BasePoint.Y));
                        objectposition.SetAttributeValue("z", string.Format(format, "{0:0.00}", radio.BasePoint.Z));
                        objectelement.Add(objectposition);

                        XElement objectrotate = new XElement("rotate");
                        objectrotate.SetAttributeValue("x", radio.AngleX);
                        objectrotate.SetAttributeValue("y", radio.AngleY);
                        objectrotate.SetAttributeValue("z", radio.AngleZ);
                        objectelement.Add(objectrotate);

                        XElement objectscale = new XElement("scale");
                        objectscale.SetAttributeValue("x", string.Format(format, "{0:0.00}", radio.ScaleX));
                        objectscale.SetAttributeValue("y", string.Format(format, "{0:0.00}", radio.ScaleY));
                        objectscale.SetAttributeValue("z", string.Format(format, "{0:0.00}", radio.ScaleZ));
                        objectelement.Add(objectscale);

                        objects.Add(objectelement);
                    }
                    scene.Add(objects);

                    document.Add(scene);
                    document.Save(ExportDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Не получается экспортировать сцену!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // XML импорт сцены из файла
        private void ImportScene_Click(object sender, EventArgs e)
        {
            if (ImportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IFormatProvider format = new System.Globalization.CultureInfo("en-us");

                    XDocument document = XDocument.Load(ImportDialog.FileName);

                    XElement scene = document.Element("scene");

                    var mode = scene.Attribute("ViewMode").Value;
                    if (mode == "WIREFRAME")
                    {
                        Scene.Mode = (Scene.MODE.WIREFRAME);
                        ViewType.SelectedIndex = 0;
                    }
                    else
                    {
                        Scene.Mode = (Scene.MODE.SOLID);
                        ViewType.SelectedIndex = 1;
                    }


                    Scene.Camera = new Camera();
                    Scene.ResetCamera();

                    var cameraElement = scene.Element("camera");
                    Scene.Camera.IsCentralProjection = bool.Parse(cameraElement.Element("central-projection").Value);

                    XElement position = cameraElement.Element("position");
                    Scene.Camera.Position = new Point3DSpherical(new Point3D(
                        double.Parse(position.Attribute("x").Value, format),
                        double.Parse(position.Attribute("y").Value, format),
                        double.Parse(position.Attribute("z").Value, format)
                        ));

                    XElement target = cameraElement.Element("target");
                    Scene.Camera.Target = new Point3DSpherical(new Point3D(
                        double.Parse(target.Attribute("x").Value, format),
                        double.Parse(target.Attribute("y").Value, format),
                        double.Parse(target.Attribute("z").Value, format)
                        ));

                    Scene.Camera.MoveCameraLeftRight(0);
                    Scene.Camera.MoveCameraUpDown(0);

                    var lightElement = scene.Element("light");

                    XElement lightPosition = lightElement.Element("position");
                    Scene.Light.X = double.Parse(lightPosition.Attribute("x").Value, format);
                    Scene.Light.Y = double.Parse(lightPosition.Attribute("y").Value, format);
                    Scene.Light.Z = double.Parse(lightPosition.Attribute("z").Value, format);


                    Scene.Objects.Clear();
                    ObjectsList.Items.Clear();
                    ObjectViewPanel.Visible = ObjectViewPanel.Enabled = ObjectSpacePanel.Visible = ObjectSpacePanel.Enabled = false;

                    foreach (XElement objectElement in scene.Element("objects").Elements("Telescope"))
                    {
                        var radioName = objectElement.Attribute("name").Value;
                        var tel = new TelescopeObject(radioName);

                        XElement objectPosition = objectElement.Element("position");
                        tel.BasePoint = new Point3D(
                            double.Parse(objectPosition.Attribute("x").Value, format),
                            double.Parse(objectPosition.Attribute("y").Value, format),
                            double.Parse(objectPosition.Attribute("z").Value, format)
                            );

                        XElement rotate = objectElement.Element("rotate");
                        tel.AngleX = int.Parse(rotate.Attribute("x").Value);
                        tel.AngleY = int.Parse(rotate.Attribute("y").Value);
                        tel.AngleZ = int.Parse(rotate.Attribute("z").Value);

                        XElement scale = objectElement.Element("scale");
                        tel.SetScale(
                            double.Parse(scale.Attribute("x").Value, format),
                            double.Parse(scale.Attribute("y").Value, format),
                            double.Parse(scale.Attribute("z").Value, format)
                            );

                        tel.Basis1CylinderRadius = double.Parse(objectElement.Element("Basis1CylinderRadius").Value, format);
                        tel.PrimaryLegsLength = double.Parse(objectElement.Element("PrimaryLegsLength").Value, format);
                        tel.Basis2CylinderRadius = double.Parse(objectElement.Element("Basis2CylinderRadius").Value, format);
                        tel.Basis3CylinderRadius = double.Parse(objectElement.Element("Basis3CylinderRadius").Value, format);
                        tel.LenseRadius = double.Parse(objectElement.Element("LenseRadius").Value, format);
                        tel.SecondaryLegsLength = double.Parse(objectElement.Element("SecondaryLegsLength").Value, format);
                        tel.PrimaryLegsCount = double.Parse(objectElement.Element("PrimaryLegsCount").Value, format);
                        tel.SecondaryLegsCount = double.Parse(objectElement.Element("SecondaryLegsCount").Value, format);
                        tel.HandsCount = int.Parse(objectElement.Element("HandsCount").Value, format);
                        tel.HandsRadius = int.Parse(objectElement.Element("HandsRadius").Value, format);

                        tel.UpdateObject();

                        Scene.AddObject(tel);
                        ObjectsList.Items.Add(tel.ObjectName);

                    }

                    // if there are no objects, the user will see it.
                    ObjectsList.SelectedIndex = -1;
                    if (Scene.Objects.Count > 0)
                    {
                        ObjectsList.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не получается импортировать сцену!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ScenePanButton_Click(object sender, EventArgs e)
        {
            if (!IsPan)
            {
                if (Scene.Objects.Count is 0)
                {
                    return;
                }

                PrepareFormForPan(true);

                var target = Scene.Objects.GetMidPoint();
                var maxDistance = Scene.Objects.GetMaxDistanceFrom(target);
                if (maxDistance < 150)
                {
                    maxDistance = 150;
                }

                var position = new Point3D(target.X + 0,
                    target.Y + maxDistance * 2,
                    target.Z + maxDistance * 1.5
                    //- Math.Max(currentObject.MaxLength + Scene.Camera.NearClipZ, 100)
                    );

                Scene.Camera.SetPositionAndTarget(position, target);

            }
            else
            {
                UndoPrepareFormForPan(true);
            }
        }

        private void TransformSceneButton_Click(object sender, EventArgs e)
        {
            var x = (double)TranslateSceneX.Value;
            var y = (double)TranslateSceneY.Value;
            var z = (double)TranslateSceneZ.Value;

            foreach (var obj in Scene.Objects)
            {
                obj.BasePoint.Add(x, y, z);
            }

            TranslateSceneX.Value = 0;
            TranslateSceneY.Value = 0;
            TranslateSceneZ.Value = 0;
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void LightPositionX_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)(sender as NumericUpDown).Value;

            Scene.Light.X = value;
        }

        private void LightPositionY_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)(sender as NumericUpDown).Value;

            Scene.Light.Y = value;
        }

        private void LightPositionZ_ValueChanged(object sender, EventArgs e)
        {
            var value = (double)(sender as NumericUpDown).Value;

            Scene.Light.X = value;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var radio = new TelescopeObject("telescope1");
            radio.BasePoint = new Point3D(0,0,0);
            radio.AngleX = (int)RotateX.Value;
            radio.AngleY = (int)RotateY.Value;
            radio.AngleZ = (int)RotateZ.Value;
            radio.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);


            Scene.AddObject(radio);
            int index = ObjectsList.Items.Add(radio.ObjectName);
            ObjectsList.SelectedIndex = index;
            ObjectPanButton.Enabled = true;
        }
    }
}
