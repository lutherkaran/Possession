using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildCommand
{
    public static void BuildAndroid()
    {
        string[] scenes = {
            "Assets/Scenes/LOADING.unity",
            "Assets/Scenes/MENU.unity",
            "Assets/Scenes/BEGINNING.unity"
        };

        string buildPath = "Builds/Android";
        string fileName = "Possession.apk";

        Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(scenes, Path.Combine(buildPath, fileName),
            BuildTarget.Android, BuildOptions.None);

        Debug.Log("Build finished successfully!...");
    }

}
