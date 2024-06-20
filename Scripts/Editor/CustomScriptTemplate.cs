/*
 * CustonScriptTemplate.cs
 * Script Author: Charles d'Ansembourg
 * Creation Date: 05/06/2024
 * Contact: c.dansembourg@icloud.com
 */

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomScriptTemplate : AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        // Ensure the path is correct and points to a C# script
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            // Delay the operation to ensure the file is created
            EditorApplication.delayCall += () => ProcessNewScript(path);
        }
    }

    private static void ProcessNewScript(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                string creationDate = DateTime.Now.ToString("dd/MM/yyyy");
                string projectName = Application.productName;
                string scriptName = Path.GetFileNameWithoutExtension(path);
                string scriptContent = File.ReadAllText(path);
                scriptContent = scriptContent.Replace("#CREATIONDATE#", creationDate)
                                             .Replace("#NAMESPACE#", projectName)
                                             .Replace("#SCRIPTNAME#", scriptName);
                File.WriteAllText(path, scriptContent);
                AssetDatabase.Refresh();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to process new script: {e.Message}");
        }
    }
}
