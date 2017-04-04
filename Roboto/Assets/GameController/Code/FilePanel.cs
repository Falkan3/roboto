using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FilePanel {
    public FileInfo[] ReadFilesInFolder(string folderName, string pattern="")
    {
        try
        {
            DirectoryInfo info = new DirectoryInfo(folderName);
            FileInfo[] files = info.GetFiles(pattern);
            return files;
        }
        catch(System.Exception ex)
        {
            Debug.Log("Can't read file. Ex: " + ex);
        }
        return null;
    }
}
