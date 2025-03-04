using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    /// <summary>
    /// Initializes the save system by ensuring the designated save folder exists.
    /// </summary>
    /// <remarks>
    /// If the save folder does not exist, it is created and a debug message is logged. Regardless, a log message confirming the initialization of the save system is output.
    /// </remarks>
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
            Debug.Log("Save Folder Created At: " + SAVE_FOLDER);
        }
        Debug.Log("Save System Initialized");
    }

    /// <summary>
    /// Saves the provided data to a new JSON file in the designated save folder.
    /// </summary>
    /// <param name="saveString">The save data to be written to the file.</param>
    /// <param name="saveName">Optional parameter for a desired file name; currently ignored as the file naming is auto-incremented.</param>
    public static void Save(string saveString, string saveName = null)
    {
        int saveNumber = 0;

        while (File.Exists($"{SAVE_FOLDER}save_{saveNumber}.json"))
        {
            saveNumber++;
        }
        File.WriteAllText($"{SAVE_FOLDER}save_{saveNumber}.json", saveString);

    }

    /// <summary>
    /// Returns the content of the most recently modified JSON save file from the save directory.
    /// </summary>
    /// <returns>
    /// A string containing the content of the most recent save file, or null if no save files exist.
    /// </returns>
    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.json");
        FileInfo mostRecentSave = null;
        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostRecentSave == null)
            {
                mostRecentSave = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentSave.LastWriteTime)
                {
                    mostRecentSave = fileInfo;
                }
            }
        }

        if (mostRecentSave != null)
        {
            return File.ReadAllText(mostRecentSave.FullName);
        }
        else
        {
            return null;
        }

    }
}
