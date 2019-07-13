using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Reflection;

namespace HiroponToolz
{
    public partial class Form1 : Form
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
        
        private TextBox textBoxPath;
        private Label labelPathName;
        
        private TextBox textBoxKeyword;
        private Label labelKeywordName;
        
        private ListBox listBoxFileList;
        
        private ContextMenuStrip contextMenuRight;
        
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        
        private BackgroundWorker backgroundWorker;
        
        private string sakuraPath = @"C:\Program Files (x86)\sakura\sakura.exe";
        
        private string[] indexFileData = null;
        private string[] indexHeaderFileData = null;
        
        private void ComponentInitialize()
        {
        	this.Text = "File Search";
        	this.Size = new Size(640, 480);

        	labelPathName = new Label(){
        					 Location = new Point(5, 5)
        					,Size = new Size(this.ClientSize.Width - 5 - 5, 15)
        					,Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left)
        					,Font = new Font("ＭＳ ゴシック", 9)
        					,Text = "SearchFilePath"
        					,
        	};

        	textBoxPath = new TextBox(){
        					 Location = new Point(5, 20)
        					,Size = new Size(this.ClientSize.Width - 5 - 5, 40)
        					,Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left)
        					,Font = new Font("ＭＳ ゴシック", 9)
        					,
        	};

        	labelKeywordName = new Label(){
        					 Bounds = new Rectangle(5, 40, this.ClientSize.Width - 5 - 5, 15)
        					,Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left)
        					,Font = new Font("ＭＳ ゴシック", 9)
        					,Text = "SearchKeyword"
        					,
        	};

        	textBoxKeyword = new TextBox(){
        					 Location = new Point(5, 55)
        					,Size = new Size(this.ClientSize.Width - 5 - 5, 40)
        					,Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left)
        					,Font = new Font("ＭＳ ゴシック", 9)
        					,
        	};

        	listBoxFileList = new ListBox(){
        					 Location = new Point(5, 80)
        					,Size = new Size(this.ClientSize.Width - 5 - 5, this.ClientSize.Height - 110)
        					,Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left)
        					,Font = new Font("ＭＳ ゴシック", 9)
        					,IntegralHeight = false
        					,HorizontalScrollbar = true
        					,
        	};

        	contextMenuRight = new ContextMenuStrip(){
        	
        	};

			backgroundWorker = new BackgroundWorker(){
								WorkerSupportsCancellation = true
								,
			};

        	statusStrip = new StatusStrip(){
        					
        	};

        	statusLabel = new ToolStripStatusLabel(){
        					Width = 640
        					,
        	};
			
        	contextMenuRight.Items.Add("開く", null, contextMenuOpen_Click);
        	if(File.Exists(sakuraPath))
        	{
        		contextMenuRight.Items.Add("SAKURAで開く", null, contextMenuSakura_Click);
        	}
        	contextMenuRight.Items.Add("ファイルパスコピー", null, contextMenuPathCopy_Click);

        	listBoxFileList.ContextMenuStrip = contextMenuRight;

        	contextMenuRight.Opening += new CancelEventHandler(contextMenuRight_Opening);

        	textBoxKeyword.TextChanged += textBoxKeyword_TextChanged;
        	textBoxPath.KeyDown += textBoxPath_KeyDown;

        	listBoxFileList.MouseDoubleClick += new MouseEventHandler(listBoxFileList_MouseDoubleClick);

        	this.Controls.Add(labelPathName);
        	this.Controls.Add(textBoxPath);
        	this.Controls.Add(labelKeywordName);
        	this.Controls.Add(textBoxKeyword);
        	this.Controls.Add(listBoxFileList);
        	this.Controls.Add(statusStrip);

        	statusStrip.Items.Add(statusLabel);

        	backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
        	backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
        	backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
	        
	        //インデックスファイルの読み込みor生成
	        if(File.Exists(FileControl.SEARCH_INDEX_FILE_PATH))
	        {
	        	indexFileData = FileControl.IndexFileRead();
	        }
	        else
	        {
	        	File.WriteAllText(FileControl.SEARCH_INDEX_FILE_PATH, "");
	        	indexFileData = FileControl.IndexFileRead();
	        }
	        
	        //インデックスファイル(ヘッダー)の読み込みor生成
	        if(File.Exists(FileControl.SEARCH_INDEX_HEADER_FILE_PATH))
	        {
	        	indexHeaderFileData = FileControl.IndexHeaderFileRead();
	        }
	        else
	        {
	        	File.WriteAllText(FileControl.SEARCH_INDEX_HEADER_FILE_PATH, "");
	        	indexHeaderFileData = FileControl.IndexHeaderFileRead();
	        }

        }
    }
}
