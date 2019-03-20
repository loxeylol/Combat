using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;

public class ScriptTemplatesEditor : EditorWindow
{
    public string EditorFolder
    {
        get
        {
            string basePath = Path.GetDirectoryName(EditorApplication.applicationPath);
            return Path.Combine(basePath, "Data", "Resources", "ScriptTemplates").Replace('\\', '/');
        }
    }

    public string LocalFolder
    {
        //get { return Path.Combine(Application.dataPath.Replace('/', '\\'), "NeoxLib", "ScriptTemplates"); }
        get
        {
            string relativePath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            return Path.GetDirectoryName(Path.GetFullPath(relativePath)).Replace('\\', '/');
        }
    }

    private readonly int[] PREDEFINED_TEMPLATE_IDS = { 81, 83, 84, 85, 86, 87, 88, 88, 90, 91, 92 };

    [MenuItem("Window/Neox Studios/Script Template Updater")]
    static void CreateWindow()
    {
        GetWindow(typeof(ScriptTemplatesEditor));
    }

    private void OnEnable()
    {
        //Debug.LogFormat("Editor Folder:\n{0}", EditorFolder);
        //Debug.LogFormat("Local Folder:\n{0}", LocalFolder);
    }

    private void OnGUI()
    {
        if(Directory.Exists(EditorFolder) == false)
        {
            EditorGUILayout.HelpBox(string.Format("Couldn't find ScriptTemplates folder at path:\n'{0}'.", EditorFolder), MessageType.Error);
            return;
        }

        if(Directory.Exists(LocalFolder) == false)
        {
            EditorGUILayout.HelpBox(string.Format("Missing ScriptTemplates folder in project at path:\n'{0}'.", LocalFolder), MessageType.Error);
            return;
        }


        EditorGUILayout.Space();

        if(GUILayout.Button("Copy ScriptTemplates", GUILayout.Height(25f)))
        {
            int counter = 0;
            foreach(string fileName in Directory.GetFiles(LocalFolder, "*.txt").Select(fp => Path.GetFileName(fp)))
            {
                File.Copy(Path.Combine(LocalFolder, fileName), Path.Combine(EditorFolder, fileName), true);
                counter++;

            }

            ShowNotification(new GUIContent(string.Format("{0} files copied!", counter)));
        }
        EditorGUILayout.LabelField("Be sure to run as Administrator and restart afterwards!", EditorStyles.largeLabel);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Current templates", EditorStyles.boldLabel);

        Func<string, int> getId = (filePath) =>
        {
            string fileName = Path.GetFileName(filePath);            

            int id;
            if(int.TryParse(fileName.Split('-')[0], out id) == false)
                id = -1;

            return id;
        };

        IOrderedEnumerable<string> filePaths = Directory.GetFiles(EditorFolder, "*.txt").OrderBy(s => getId(s));
        foreach(string filePath in filePaths)
        {
            GUILayout.BeginHorizontal();
            {                
                EditorGUILayout.LabelField(Path.GetFileName(filePath));

                int id = getId(filePath);
                EditorGUI.BeginDisabledGroup(PREDEFINED_TEMPLATE_IDS.Contains(id));
                {
                    if(GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.Width(40f)))
                    {
                        File.Delete(filePath);
                    }
                }
                EditorGUI.EndDisabledGroup();
                
            }
            GUILayout.EndHorizontal();                        
        }
    }
}
#endif