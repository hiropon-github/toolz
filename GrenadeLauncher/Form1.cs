using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;

namespace Hiropon
{
    public partial class Form1
    {
    	[DllImport("shell32.dll", CharSet=CharSet.Auto)]
		private static extern bool PickIconDlg(IntPtr hwndOwner, StringBuilder lpstrFile, int nMaxFile, ref int lpdwIconIndex);
		
		[DllImport("shell32.dll")]
		private static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        
        private const string SettingFilePath = "setting.csv";
        private const string iniFilePath = "./setting.ini";
        
        private ContextMenuStrip contextMenuRight;
        private List<IconSetting> iconSetting = new List<IconSetting>();
        
        
        public Form1()
        {
            ComponentInitialize();
            
            InitializeSetting();
            
        }
        
        private void InitializeSetting()
        {
        	contextMenuRight = new ContextMenuStrip();
        	contextMenuRight.Items.Add("アイコンを編集する", null, contextMenuIconChange_Click);
        
            string inputFilePath = SettingFilePath;
			string[] lines = File.ReadAllLines(inputFilePath, Encoding.GetEncoding("shift_jis"));
            
            foreach(string line in lines)
            {
            	var param = line.Split(',');
            	iconSetting.Add(new IconSetting(param[0], param[1], int.Parse(param[2])));
            }
            
            ToolTip toolTip = new ToolTip();
            
            IntPtr[] hLargeIcon = new IntPtr[1] {IntPtr.Zero};
            
            for(int i = 0; i < lines.Length; i++)
            {
            	if (ExtractIconEx(iconSetting[i].IconDataPath, iconSetting[i].IconIndex, hLargeIcon, null, 1) < 1)
				{
					return;
				}
				
	            var btn = new Button(){
	                Location = new Point(5 + i * 55, 5),
	                Size = new Size(50, 50),
	                Image = Icon.FromHandle(hLargeIcon[0]).ToBitmap(),
	                TextImageRelation = TextImageRelation.ImageBeforeText,
	                Name = i.ToString(),
	                Tag = iconSetting[i].AppFilePath,
	            };
	            btn.ContextMenuStrip = contextMenuRight;
            	this.Controls.Add(btn);
            	
            	btn.Click += button_Click;
            	btn.MouseDown += button_MouseDown;
            	
            	toolTip.InitialDelay = 300;
            	toolTip.ReshowDelay = 300;
            	toolTip.AutoPopDelay = 5000;
            	toolTip.ShowAlways = true;
            	
            	toolTip.SetToolTip(btn, Path.GetFileName(btn.Tag.ToString()));
            	
            }
            
        }
        
        private void button_Click(object sender, EventArgs e)
        {
        	Button btn = sender as Button;
			Process.Start(btn.Tag.ToString());
			this.Close();
        }
        
        private void button_MouseDown(object sender, MouseEventArgs e)
        {
        	if(e.Button == MouseButtons.Right)
        	{
        		(sender as Button).Select();
        	}
        }
        
        private void contextMenuIconChange_Click(object sender, EventArgs e)
        {
        	var setting = iconSetting[int.Parse(((Button)this.ActiveControl).Name)];
        
        	StringBuilder path = new StringBuilder(setting.IconDataPath, 260);
        	
    		int iconIndex = 0;
    		if(PickIconDlg(this.Handle, path, path.Capacity, ref iconIndex))
    		{
	            IntPtr[] hLargeIcon = new IntPtr[1] {IntPtr.Zero};
	    		
	    		if (ExtractIconEx(path.ToString(), iconIndex, hLargeIcon, null, 1) < 1)
				{
					return;
				}
				
				((Button)this.ActiveControl).Image = Icon.FromHandle(hLargeIcon[0]).ToBitmap();
	    		
	    		setting.SetIconDataPath(path.ToString());
	    		setting.SetIconIndex(iconIndex);
	    		
	    		/*
	    		string[] data = new string[iconSetting.Count];
	    		for(int i = 0; i < iconSetting.Count; i++)
	    		{
	    			data[i] = iconSetting[i].AppFilePath + "," + iconSetting[i].IconDataPath + "," + iconSetting[i].IconIndex.ToString();
	    		}
	    		*/
	    		
	    		var settingArray = iconSetting.Select(s => s.AppFilePath + "," + s.IconDataPath + "," + s.IconIndex).ToArray();
	    		File.WriteAllLines(SettingFilePath, settingArray, Encoding.GetEncoding("shift_jis"));
    		}
        }
        
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
    		if (e.KeyChar == (char)Keys.Escape)
    		{
        		this.Close();
    		}
		}
		
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			IniFile ini = new IniFile(iniFilePath);
    		ini["WindowSize", "WindowWidth"]         = this.Size.Width.ToString();
    		ini["WindowPosition", "WindowPositionX"] = this.Location.X.ToString();
    		ini["WindowSize", "WindowHeight"]        = this.Size.Height.ToString();
    		ini["WindowPosition", "WindowPositionY"] = this.Location.Y.ToString();
		}
    }
}
