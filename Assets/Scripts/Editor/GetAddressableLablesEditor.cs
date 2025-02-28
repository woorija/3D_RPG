using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;

#if UNITY_EDITOR
    [CustomEditor(typeof(DownloadManager))]
    public class GetAddressableLableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DownloadManager myObject = (DownloadManager)target;

            if (GUILayout.Button("Assign Labels"))
            {
                AssignLabels(myObject);
            }

            base.OnInspectorGUI();
        }

        private void AssignLabels(DownloadManager myObject)
        {
            // 에디터에서 AddressableAssetSettings를 사용하여 라벨을 가져올 수 있습니다.
            AddressableAssetSettings settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;

            if (settings != null)
            {
                List<string> labels = settings.GetLabels();
                myObject.ClearLable();
                // AddressableAssetEntry는 AddressableAssetSettings에 등록된 각각의 asset을 나타냅니다.
                foreach (var label in labels)
                {
                    myObject.AddLable(label);
                }
            }
            else
            {
                Debug.LogError("Addressable Asset Settings is not found!");
            }
        }
    }
#endif

