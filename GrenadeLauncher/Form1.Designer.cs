using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Data;

namespace Hiropon
{
    public partial class Form1 : Form
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
        
        private void ComponentInitialize()
        {
        	if(System.IO.File.Exists(iniFilePath))
        	{
        		this.StartPosition = FormStartPosition.Manual;
        		
        		IniFile ini = new IniFile(iniFilePath);
        		string WindowWidth     = ini["WindowSize", "WindowWidth"];
        		string WindowHeight    = ini["WindowSize", "WindowHeight"];
        		string WindowPositionX = ini["WindowPosition", "WindowPositionX"];
        		string WindowPositionY = ini["WindowPosition", "WindowPositionY"];
        		
        		this.SetBounds(int.Parse(WindowPositionX), int.Parse(WindowPositionY), int.Parse(WindowWidth), int.Parse(WindowHeight),  BoundsSpecified.All);
        	}
        	else
        	{
        		this.Size = new Size(1200, 100);
        	}
        	
        	this.Text = "GrenadeLauncher";
        	
        	this.KeyPreview = true;
        	this.KeyPress += Form1_KeyPress;
        	this.Closing += Form1_Closing;
        	
        	
        }
        
        
    }
}
