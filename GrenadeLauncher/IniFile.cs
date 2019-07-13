using System.Runtime.InteropServices;
using System.Text;

//�Q�l�T�C�g
//https://anis774.net/codevault/inifile.html

namespace Hiropon
{
	/// <summary>
	/// INI�t�@�C����ǂݏ�������N���X
	/// </summary>
	public class IniFile {
    	[DllImport("kernel32.dll")]
    	private static extern int GetPrivateProfileString(
    	    string lpApplicationName, 
    	    string lpKeyName, 
    	    string lpDefault, 
    	    StringBuilder lpReturnedstring, 
    	    int nSize, 
    	    string lpFileName);

    	[DllImport("kernel32.dll")]
    	private static extern int WritePrivateProfileString(
    	    string lpApplicationName,
    	    string lpKeyName, 
    	    string lpstring, 
    	    string lpFileName);

    	string filePath;

    	/// <summary>
    	/// �t�@�C�������w�肵�ď��������܂��B
    	/// �t�@�C�������݂��Ȃ��ꍇ�͏��񏑂����ݎ��ɍ쐬����܂��B
    	/// </summary>
    	public IniFile(string filePath) {
    	    this.filePath = filePath;
    	}

    	/// <summary>
    	/// section��key����ini�t�@�C���̐ݒ�l���擾�A�ݒ肵�܂��B 
    	/// </summary>
    	/// <returns>�w�肵��section��key�̑g�����������ꍇ��""���Ԃ�܂��B</returns>
    	public string this[string section,string key] {
    	    set {
    	        WritePrivateProfileString(section, key, value, filePath);
    	    }
    	    get {
    	        StringBuilder sb = new StringBuilder(256);
    	        GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, filePath);
    	        return sb.ToString();
    	    }
    	}

    	/// <summary>
    	/// section��key����ini�t�@�C���̐ݒ�l���擾���܂��B
    	/// �w�肵��section��key�̑g�����������ꍇ��defaultvalue�Ŏw�肵���l���Ԃ�܂��B
    	/// </summary>
    	/// <returns>
    	/// �w�肵��section��key�̑g�����������ꍇ��defaultvalue�Ŏw�肵���l���Ԃ�܂��B
    	/// </returns>
    	public string GetValue(string section, string key, string defaultvalue) {
    	    StringBuilder sb = new StringBuilder(256);
    	    GetPrivateProfileString(section, key, defaultvalue, sb, sb.Capacity, filePath);
    	    return sb.ToString();
    	}
	}
}
