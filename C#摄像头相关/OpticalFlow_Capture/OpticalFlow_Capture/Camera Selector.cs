using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//DiresctShow
using DirectShowLib;

namespace OpticalFlow_Capture_Farneback
{
    public partial class Camera_Selector : Form
    {
        Video_Device[] WebCams;
        Form1 Owner;
        public int Camera_Count { get; private set; }
        public int Camera_ID { get; private set; }
        public Camera_Selector(Form1 _Owner)
        {
            InitializeComponent();
            Owner = _Owner;

            DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            WebCams = new Video_Device[_SystemCamereas.Length];
            Camera_Count = _SystemCamereas.Length;
            for (int i = 0; i < _SystemCamereas.Length; i++)
            {
                WebCams[i] = new Video_Device(i, _SystemCamereas[i].Name, _SystemCamereas[i].ClassID); //fill web cam array
                Camera_Selection.Items.Add(WebCams[i].ToString());
            }
            if (Camera_Selection.Items.Count > 0)
            {
                Camera_Selection.SelectedIndex = 0; //Set the selected device the default
                BTN_Capture.Enabled = true; //Enable the start
            }
        }

        private void BTN_Capture_Click(object sender, EventArgs e)
        {
            Camera_ID = Camera_Selection.SelectedIndex;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

    }
}
