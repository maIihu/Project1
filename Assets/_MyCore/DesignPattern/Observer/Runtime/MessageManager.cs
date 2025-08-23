﻿using System.Collections;
using System.Collections.Generic;
using _MyCore.DesignPattern.Singleton;
using UnityEngine;

namespace _MyCore.DesignPattern.Observer.Runtime
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
        [HideInInspector] public List<ProjectMessageType> keys = new List<ProjectMessageType>();
        [HideInInspector] public List<List<IMessageHandle>> Values = new List<List<IMessageHandle>>();
    
    
        private Dictionary<ProjectMessageType, List<IMessageHandle>> _subscribers = new Dictionary<ProjectMessageType, List<IMessageHandle>>();
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
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<IMessageHandle>();
            if (!_subscribers[type].Contains(handle))
                _subscribers[type].Add(handle);
        }
        public void RemoveSubscriber(ProjectMessageType type, IMessageHandle handle)
        {
            if (_subscribers.ContainsKey(type))
                if (_subscribers[type].Contains(handle))
                    _subscribers[type].Remove(handle);
        }
        public void SendMessage(Message message)
        {
            if (_subscribers.ContainsKey(message.Type))
                for (int i = _subscribers[message.Type].Count - 1; i > -1; i--)
                    _subscribers[message.Type][i].Handle(message);
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
            keys.Clear();
            Values.Clear();
            foreach (var element in _subscribers)
            {
                keys.Add(element.Key);
                Values.Add(element.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            _subscribers = new Dictionary<ProjectMessageType, List<IMessageHandle>>();
            for (int i = 0; i < keys.Count; i++)
            {
                _subscribers.Add(keys[i], Values[i]);
            }
        }
    }
}