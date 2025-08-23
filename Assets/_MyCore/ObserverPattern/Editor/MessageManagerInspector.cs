using _MyCore.ObserverPattern.Runtime;
using UnityEditor;
using UnityEngine;

namespace _MyCore.ObserverPattern.Editor
{
    [CustomEditor(typeof(MessageManager))]
    public class MessageManagerInspector : UnityEditor.Editor
    {
        private MessageManager _messageManager;
        void OnEnable()
        {
            _messageManager = (MessageManager)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            for (int i = _messageManager._keys.Count - 1; i > -1; i--)
                ShowElement(i);
            serializedObject.ApplyModifiedProperties();
        }
        private void ShowElement(int index)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(_messageManager._keys[index].ToString(), EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var subcriber in _messageManager._values[index])
                EditorGUILayout.LabelField(subcriber.ToString());
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}