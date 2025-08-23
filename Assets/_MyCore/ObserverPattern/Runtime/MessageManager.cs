using System.Collections;
using System.Collections.Generic;
using _MyCore.Singleton;
using UnityEngine;

namespace _MyCore.ObserverPattern.Runtime
{
    public enum ProjectMessageType
    {
        /// <summary>
        /// When the game is over
        /// </summary>
        OnGameOver,
    }
    public class Message
    {
        public ProjectMessageType Type;
        public object[] Data;
        public Message(ProjectMessageType type)
        {
            this.Type = type;
        }
        public Message(ProjectMessageType type, object[] data)
        {
            this.Type = type;
            this.Data = data;
        }
    }
    public interface IMessageHandle
    {
        void Handle(Message message);
    }
    public class MessageManager : Singleton<MessageManager>, ISerializationCallbackReceiver
    {
        private static MessageManager instance = null;
    
        //Stores information when Serialize data in the subcribers-Dictionary
        [HideInInspector] public List<ProjectMessageType> _keys = new List<ProjectMessageType>();
        [HideInInspector] public List<List<IMessageHandle>> _values = new List<List<IMessageHandle>>();
    
    
        private Dictionary<ProjectMessageType, List<IMessageHandle>> subcribers = new Dictionary<ProjectMessageType, List<IMessageHandle>>();
        /*public static MessageManager Instance { get { return instance; } }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }*/
        public void AddSubscriber(ProjectMessageType type, IMessageHandle handle)
        {
            if (!subcribers.ContainsKey(type))
                subcribers[type] = new List<IMessageHandle>();
            if (!subcribers[type].Contains(handle))
                subcribers[type].Add(handle);
        }
        public void RemoveSubscriber(ProjectMessageType type, IMessageHandle handle)
        {
            if (subcribers.ContainsKey(type))
                if (subcribers[type].Contains(handle))
                    subcribers[type].Remove(handle);
        }
        public void SendMessage(Message message)
        {
            if (subcribers.ContainsKey(message.Type))
                for (int i = subcribers[message.Type].Count - 1; i > -1; i--)
                    subcribers[message.Type][i].Handle(message);
        }
        public void SendMessageWithDelay(Message message, float delay)
        {
            StartCoroutine(_DelaySendMessage(message, delay));
        }
        private IEnumerator _DelaySendMessage(Message message, float delay)
        {
            yield return new WaitForSeconds(delay);
            SendMessage(message);
        }
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (var element in subcribers)
            {
                _keys.Add(element.Key);
                _values.Add(element.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            subcribers = new Dictionary<ProjectMessageType, List<IMessageHandle>>();
            for (int i = 0; i < _keys.Count; i++)
            {
                subcribers.Add(_keys[i], _values[i]);
            }
        }
    }
}