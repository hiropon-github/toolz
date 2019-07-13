using System;
using System.Diagnostics;

namespace HiroponToolz
{
	//ファイルの検索結果を格納するクラス
	class SearchItem
	{
		public string FilePath{ get; private set; }				//フルパス+ファイル名の格納用
		public string FileName{ get; private set; }				//ファイル名の格納用
		public string FileRelativePath{ get; private set; }		//相対パスの格納用
		
		public SearchItem( string filePath, string fileName, string fileRelativePath )
		{
			this.FilePath = filePath;
			this.FileName = fileName;
			this.FileRelativePath = fileRelativePath;
		}
		
		public override string ToString()
		{
			if(String.IsNullOrEmpty(this.FileRelativePath))
			{
				return this.FileName;
			}
		
			return this.FileName + " [ " + this.FileRelativePath + " ]";
		}
	}
}