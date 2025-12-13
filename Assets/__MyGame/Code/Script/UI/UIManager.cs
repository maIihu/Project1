using System;
using __MyGame.Code.Script;
using _MyCore.DesignPattern.Observer.Runtime;
using _MyCore.DesignPattern.Singleton;
using System.Collections;
using System.Collections.Generic;
using __MyGame.Code.Script.UI.Popups;
using __MyGame.Code.Script.UI.Screens;
using UnityEngine;


[DefaultExecutionOrder(-900)]
public class UIManager : Singleton<UIManager>, IMessageHandle
{
	[Header("----------SCREEN----------")]
	[SerializeField] private UIGameplayScreen gameplayScreen;
	
	[Header("----------POPUP----------")]
	[SerializeField] private UIPausePopup pausePopup;

	[Header("----------OTHERS----------")]
	[SerializeField] private UIFade uiFade;
	[SerializeField] private SkillListController skillListController;
	[SerializeField] private PlayerInfoController playerInfoController;
	[SerializeField] private SkillDestinationUI skillDestinationUI;
	[SerializeField] private GrowthUIManager growthUIManager;
	
	private UIScreenBase _currentScreen;
	private UIPopupBase _currentPopup;

	private void Awake()
	{
		Initialize(this);
	}

	private void Start()
	{
		InitALlScreen();
		InitAllPopup();
		pausePopup.Hide();
	}

	private void InitALlScreen()
	{
		gameplayScreen.Init();
	}

	private void InitAllPopup()
	{
		pausePopup.Init();
	}
	
	private void OnEnable()
	{
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnGameStart, this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnLoadGame, this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnGameOver, this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnMoveControl, this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnDirectionRequiredSkillSelected,this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.OnNodeRequiredSkillSelected, this);
		MessageManager.Instance.AddSubscriber(ProjectMessageType.EndOfSkillRequireSelection, this);	
	}

	private void OnDisable()
	{
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnGameStart, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnLoadGame, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnGameOver, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnMoveControl, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnDirectionRequiredSkillSelected, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.OnNodeRequiredSkillSelected, this);
		MessageManager.Instance.RemoveSubscriber(ProjectMessageType.EndOfSkillRequireSelection,this);

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
				growthUIManager.Initial(player);
				break;
			case ProjectMessageType.OnLoadGame:
				uiFade.Fade();
				break;
			case ProjectMessageType.OnGameOver:
				skillListController.Clear();
				playerInfoController.Unbind();	
				break;
			// case ProjectMessageType.OnGameReload:
			// 	var dataReload = message.Data;
			// 	Debug.Log(dataReload[0]);
			// 	gameplayScreen.UpdateProgress((int)dataReload[0], (int)dataReload[1]);
			// 	break;
			case ProjectMessageType.OnMoveControl:
				var data = message.Data;
				gameplayScreen.UpdateProgress((int)data[0], (int)data[1]);
				break;
			case ProjectMessageType.OnDirectionRequiredSkillSelected:
				skillDestinationUI.Show("Choose skill direction.");
				break;
			case ProjectMessageType.OnNodeRequiredSkillSelected:
				skillDestinationUI.Show("Choose skill target node.");
				break;
			case ProjectMessageType.EndOfSkillRequireSelection:
				skillDestinationUI.Hide();
				break;

		}
	}
	
	public void ShowPausePopup()
	{
		pausePopup.Show();
	}
}
