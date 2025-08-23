using _MyCore.DesignPattern.Observer.Runtime;
using UnityEditor;
using UnityEngine;

namespace _MyCore.DesignPattern.Observer.Editor
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
            for (int i = _messageManager.keys.Count - 1; i > -1; i--)
                ShowElement(i);
            serializedObject.ApplyModifiedProperties();
        }
        private void ShowElement(int index)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(_messageManager.keys[index].ToString(), EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var subcriber in _messageManager.Values[index])
                EditorGUILayout.LabelField(subcriber.ToString());
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}