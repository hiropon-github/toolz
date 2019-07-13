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
using System.Threading;
using System.Threading.Tasks;

namespace HiroponToolz
{
	public partial class Form1
	{
		private List<SearchItem> searchItems = null;
		
		public Form1()
		{
			ComponentInitialize();
		}
		
		private void textBoxPath_KeyDown(object sender, KeyEventArgs e)
		{	
			if(e.KeyCode == Keys.Enter)
			{
				
				if(backgroundWorker.IsBusy)
				{
					backgroundWorker.CancelAsync();
				}
				
				
				if(!String.IsNullOrEmpty(textBoxKeyword.Text))
				{
					textBoxKeyword.Text = String.Empty;
				}
				
				listBoxFileList.Items.Clear();
			
				string dirPath = textBoxPath.Text;
				
				searchItems = new List<SearchItem>();
				
				//入力したパスがヘッダーファイルに存在しているか否か
				if(FileControl.IndexHeaderExists(indexHeaderFileData, dirPath))
				{
					searchItems = indexFileData.Select(f => new SearchItem(f, Path.GetFileName(f), Path.GetDirectoryName(f).Replace(dirPath, ""))).ToList();
					listBoxFileList.Items.AddRange(searchItems.ToArray());
				}
				else
				{
					/*
					try{
						//var fileList = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
						var fileList = Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories);
						
						searchItems = fileList.Select(f => new SearchItem(f, Path.GetFileName(f), Path.GetDirectoryName(f).Replace(dirPath, ""))).ToList();
						
						listBoxFileList.Items.AddRange(searchItems.ToArray());
						
					}catch(System.Exception ex){
						
					}
					*/
					//スレッド処理
					//backgroundWorker.RunWorkerAsync();
					backgroundWorker.RunWorkerAsync(dirPath);
					
				}
				
				
			}
			else if(e.KeyCode == Keys.Escape)
			{
				//MessageBox.Show("Escape");
				if(backgroundWorker.IsBusy)
				{
					//MessageBox.Show("IsBusy");
					backgroundWorker.CancelAsync();
				}
			}
			
			
		}

		private void textBoxKeyword_TextChanged(object sender, EventArgs e)
		{
			if(String.IsNullOrEmpty(textBoxPath.Text))
			{
				MessageBox.Show("ファイルパスを入力してください。",
						"エラー",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
						
				return;
			}
			
			listBoxFileList.Items.Clear();
			string textValue = textBoxKeyword.Text;
			
			if(String.IsNullOrEmpty(textValue))
			{
				listBoxFileList.Items.AddRange(searchItems.ToArray());
			}
			else
			{
				string[] keywords = textValue.Replace("　"," ").Trim().Split(' ');
			
				foreach(var item in searchItems)
				{
					bool isContains = true;
					
					foreach(string keyword in keywords)
					{
						if(item.FileName.Contains(keyword) == false)
						{
							isContains = false;
							break;
						}
					}
					
					if(isContains)
					{
						listBoxFileList.Items.Add(item);
					}
					
				}
			}
		}
		
		private void listBoxFileList_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if(listBoxFileList.SelectedItem == null)
			{
				return;
			}
			
			var item = (SearchItem)listBoxFileList.SelectedItem;
			System.Diagnostics.Process p = System.Diagnostics.Process.Start(item.FilePath);
		}
		
		/*
		private void listBoxFileList_MouseUp(object sender, MouseEventArgs e)
		{
			
			if(e.Button == MouseButtons.Right)
			{
				int index = listBoxFileList.IndexFromPoint(e.Location);
				
				if(index >= 0)
				{
					listBoxFileList.ClearSelected();
					listBoxFileList.SelectedIndex = index;
					contextMenuRight.Show(System.Windows.Forms.Cursor.Position);
				}
			}
		}
		*/
		
		private void contextMenuRight_Opening(object sender, CancelEventArgs e)
		{
			int index = listBoxFileList.IndexFromPoint(listBoxFileList.PointToClient(Cursor.Position));

			if(index >= 0)
			{
				listBoxFileList.ClearSelected();
				listBoxFileList.SelectedIndex = index;
				contextMenuRight.Show(System.Windows.Forms.Cursor.Position);
			}
			else
			{
				e.Cancel = true;
			}
		}
		
		private void contextMenuOpen_Click(object sender, EventArgs e)
		{
			var item = (SearchItem)listBoxFileList.SelectedItem;
			System.Diagnostics.Process p = System.Diagnostics.Process.Start(item.FilePath);
		}
		
		private void contextMenuSakura_Click(object sender, EventArgs e)
		{
			var item = (SearchItem)listBoxFileList.SelectedItem;
			System.Diagnostics.Process p = System.Diagnostics.Process.Start(sakuraPath, item.FilePath);
		}
		
		private void contextMenuPathCopy_Click(object sender, EventArgs e)
		{
			var item = (SearchItem)listBoxFileList.SelectedItem;
			Clipboard.SetText(Path.GetDirectoryName(item.FilePath));
		}
		
		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			/*
			while(true)
			{
				if(backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
					return;
				}
				//MessageBox.Show("DoWork");
				statusLabel.Text = "読み込み中...";
				Thread.Sleep(200);
				//break;
			}
			*/
			
			statusLabel.Text = "ファイル検索中...";
			
			//if(!String.IsNullOrEmpty(textBoxKeyword.Text))
			//{
			//		textBoxKeyword.Text = String.Empty;
			//}
				
			listBoxFileList.Items.Clear();
			
			string dirPath = (string)e.Argument;
			
			searchItems = new List<SearchItem>();
				
			try{
			   	//var fileList = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
			   	var fileList = Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories);
			   	
			   	
			   	searchItems = fileList.Select(f => new SearchItem(f, Path.GetFileName(f), Path.GetDirectoryName(f).Replace(dirPath, ""))).ToList();
			   	
			   	listBoxFileList.Items.AddRange(searchItems.ToArray());
			   	
			}catch(System.Exception ex){
				
			}
			
		}
		
		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			MessageBox.Show("ProgressChanged");
		}
		
		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if(e.Cancelled)
			{
				//MessageBox.Show("キャンセルされました");
				statusLabel.Text = "キャンセル";
			}
			else
			{
				//MessageBox.Show("RunWorkerCompleted");
				statusLabel.Text = "インデックス更新処理完了！";
			}
		}
	}
}
