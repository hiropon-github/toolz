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
    class IconSetting
    {
		public string AppFilePath{ get; private set; }
		public string IconDataPath{ get; private set; }
		public int IconIndex{ get; private set; }
		
		public IconSetting(string appFilePath, string iconDataPath, int iconIndex)
		{
			this.AppFilePath  = appFilePath;
			this.IconDataPath = iconDataPath;
			this.IconIndex    = iconIndex;
		}
		
		public void SetIconDataPath(string iconDataPath)
		{
			this.IconDataPath = iconDataPath;
		}
		
		public void SetIconIndex(int iconIndex)
		{
			this.IconIndex = iconIndex;
		}
		
	}
}
