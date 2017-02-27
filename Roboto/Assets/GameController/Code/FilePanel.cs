using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FilePanel {
    public FileInfo[] ReadFilesInFolder(string folderName)
    {
        try
        {
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/" + folderName);
            FileInfo[] files = info.GetFiles();
            return files;
        }
        catch(System.Exception ex)
        {
            Debug.Log("Can't read file. Ex: " + ex);
        }
        return null;
    }
}
