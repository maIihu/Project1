using __MyGame.Code.Script;
using _MyCore.DesignPattern.Observer.Runtime;
using _MyCore.DesignPattern.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-900)]
public class UIManager : Singleton<UIManager>, IMessageHandle
{
	[SerializeField] private SkillListController skillListController;
	[SerializeField] private PlayerInfoController playerInfoController;

	private void Awake()
	{
		Initialize(this);
	}
	private void OnEnable()
	{
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnGameStart, this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnGameOver, this);
	}

	private void OnDisable()
	{
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnGameStart, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnGameOver, this);
	}
	public void Handle(Message message)
	{
		switch (message.Type)
		{
			case ProjectMessageType.OnGameStart:
				var player = BoardController.Instance.GetPlayer();
				if(player == null) Debug.Log(message.Type + " player is null");
				if (player == null) return;
				playerInfoController.Bind(player);
				skillListController.BuildForm(player);
				break;
			case ProjectMessageType.OnGameOver:
				skillListController.Clear();
				playerInfoController.Unbind();	
				break;
		}
	}
}
