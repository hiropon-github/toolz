using System;
using System.IO;
using System.Text;
using System.Linq;

namespace HiroponToolz
{
	//インデックス生成、操作用のクラス
	class FileControl
	{
		public const string SEARCH_INDEX_FILE_PATH = @"searchindex.txt";
		public const string SEARCH_INDEX_HEADER_FILE_PATH = @"searchindexheader.txt";
	
		public static string[] IndexHeaderFileRead()
		{
			//var fileData = File.ReadAllLines(SEARCH_INDEX_HEADER_FILE_PATH, Encoding.GetEncoding("SHIFT_JIS"));
			return File.ReadAllLines(SEARCH_INDEX_HEADER_FILE_PATH, Encoding.GetEncoding("SHIFT_JIS"));
		}
	
		public static string[] IndexFileRead()
		{
			//var fileData = File.ReadAllLines(SEARCH_INDEX_FILE_PATH, Encoding.GetEncoding("SHIFT_JIS"));
			return File.ReadAllLines(SEARCH_INDEX_FILE_PATH, Encoding.GetEncoding("SHIFT_JIS"));
		}
		
		public static void IndexHeaderFileWrite(string[] fileData)
		{
			File.WriteAllLines(SEARCH_INDEX_HEADER_FILE_PATH, fileData, Encoding.GetEncoding("SHIFT_JIS"));
		}
		
		public static void IndexFileWrite(string[] fileData)
		{
			File.WriteAllLines(SEARCH_INDEX_FILE_PATH, fileData, Encoding.GetEncoding("SHIFT_JIS"));
		}
		
		public static bool IndexHeaderExists(string[] data, string path)
		{
			if(data != null)
			{
				File.WriteAllText("log.txt", path, Encoding.GetEncoding("SHIFT_JIS"));
				foreach(string s in data)
				{
					if(s.Contains(path))
					{
						return true;
					}
				}
				
			}
		
			return false;
		}
	}
}