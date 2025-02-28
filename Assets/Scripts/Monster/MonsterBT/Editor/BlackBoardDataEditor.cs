using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

#if UNITY_EDITOR
[CustomEditor(typeof(MonsterBlackBoardSO), true)]
public class BlackBoardDataEditor : Editor
{
    Type[] dataTypes;
    string[] dataNames;
    int selectedIndex = -1;
    private void OnEnable()
    {
        var baseType = typeof(MonsterBlackBoardSO);
        dataTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type))
            .ToArray();

        dataNames = dataTypes.Select(type => type.Name).ToArray();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomEditorUtility.DrawLine();
        selectedIndex = EditorGUILayout.Popup("Change DataType",selectedIndex, dataNames);
        CustomEditorUtility.DrawButton("Apply", () => ApplySelection());

        CustomEditorUtility.DrawLine();
        CustomEditorUtility.DrawButton("SetUp", () => SetUp());
    }

    void ApplySelection()
    {
        if (selectedIndex == -1) return;
        MonsterBlackBoardSO currentData = (MonsterBlackBoardSO)target;

        string assetPath = AssetDatabase.GetAssetPath(currentData);
        string assetName = currentData.name;

        MonsterBlackBoardSO newData = (MonsterBlackBoardSO)CreateInstance(dataTypes[selectedIndex]);

        CopyData(currentData, newData);

        newData.name = assetName;

        AssetDatabase.CreateAsset(newData, assetPath);
        DestroyImmediate(currentData, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = newData;
    }

    void CopyData(MonsterBlackBoardSO _currentData, MonsterBlackBoardSO _newData)
    {
        var dataType = typeof(MonsterBlackBoardSO);
        var fields = dataType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var value = field.GetValue(_currentData);
            field.SetValue(_newData, value);
        }
    }

    void SetUp()
    {
        var currentData = (MonsterBlackBoardSO)target;
        if(currentData.id <= 0) return;

        string path = "Assets/Resources_moved/CSV/MonsterData.csv";
        if (!File.Exists(path)) return;

        string csv = File.ReadAllText(path);
        var lines = Regex.Split(csv, CSVReader.LINE_SPLIT_RE);
        string[] headers = Regex.Split(lines[0], CSVReader.SPLIT_RE);
        for(int i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            if (CSVReader.GetIntData(values[0]) == currentData.id)
            {
                SetUpDataCsvToSo(currentData, headers, values);
                EditorUtility.SetDirty(currentData);
                AssetDatabase.SaveAssets();
                return;
            }
        }
    }
    void SetUpDataCsvToSo(MonsterBlackBoardSO _data, string[] _headers, string[] _values)
    {
        var dataType = _data.GetType();

        for (int i = 0; i < _headers.Length; i++)
        {
            var field = dataType.GetField(_headers[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                object convertedValue = Convert.ChangeType(_values[i], field.FieldType);
                field.SetValue(_data, convertedValue);
            }
        }
    }
}
#endif
