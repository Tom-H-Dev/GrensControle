using System;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;

public class BuildVersionProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    private const string initialVersion = "0.0";

    public void OnPreprocessBuild(BuildReport report)
    {
        string currenVersion = FindCurrenVersion();
        UpdateVersion(currenVersion);
    }

    private string FindCurrenVersion()
    {
        string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']'); // String in twee splitsen

        return currentVersion.Length == 1 ? initialVersion : currentVersion[1];
    }

    private void UpdateVersion(string version)
    {
        if (float.TryParse(version, out float versionNumber))
        {
            float NewVersion = versionNumber + 0.01f;
            string date = DateTime.Now.ToString("d");

            PlayerSettings.bundleVersion = string.Format("{1} - [{0}]", NewVersion, date); // format naar: datum - [versie]
            Debug.Log(PlayerSettings.bundleVersion);
        }


    }
}
