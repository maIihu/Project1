using __MyGame.Code.Script;
using _MyCore.DesignPattern.Observer.Runtime;
using _MyCore.DesignPattern.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SkillSelectedUIController : Singleton<SkillSelectedUIController>
{
	[SerializeField] private BoardController boardController;
	private GameplayManager gameplayManager;
	private GameLogic logic;
	private PlayerEntity player;
	private BaseCharacterAbility currentAbility;
	private bool isSelecting;
	private AbilityTarget currentTarget;
	private Node currentNode;
	private Vector2Int lastAimDir;
	private List<Node> highlightedNodes = new List<Node>();
	public bool IsSelecting => isSelecting;
	private void Awake()
	{
		Initialize(this);
	}

	private void OnEnable()
	{
		ActiveSkillButtonUI.OnActiveSkillButtonClicked += OnSkillButtonClicked;
	}
	private void OnDisable()
	{
		ActiveSkillButtonUI.OnActiveSkillButtonClicked -= OnSkillButtonClicked;
	}
	//private void Start()
	//{
	//	gameplayManager = GameplayManager.Instance;
	//	logic = gameplayManager.GameLogic;
	//	player = boardController.GetPlayer();
	//}

	public void InitiateReference()
	{
		gameplayManager = GameplayManager.Instance;
		logic = gameplayManager.GameLogic;
		player = boardController.GetPlayer();
	}

	private void OnSkillButtonClicked(BaseCharacterAbility ability)
	{
		isSelecting = true;
		if (ability == null && player == null)
		{
			Debug.Log("Null");
			return;
		}
		currentAbility = ability;
		currentTarget = ability.target;
		isSelecting = true;
		lastAimDir = Vector2Int.right;

		switch (currentTarget)
		{
			case AbilityTarget.Instant:
				//CastInstant();
				break;
			case AbilityTarget.Node:
				MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnNodeRequiredSkillSelected));
				StartNodeMode();
				break;
			case AbilityTarget.Direction:
				MessageManager.Instance.SendMessage(new Message(ProjectMessageType.OnDirectionRequiredSkillSelected));
				break;
		}
	}
	private void Update()
	{
		if (!isSelecting || currentAbility == null)
			return;
		switch (currentTarget)
		{
			case AbilityTarget.Direction:
				UpdateDirectionMode();
				break;
			case AbilityTarget.Node:
				UpdateNodeMode();
				break;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			CancelSelection();
		}
	}
	#region Direction mode}
	private void UpdateDirectionMode()
	{
		Vector2Int aim = Vector2Int.zero;
		if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.RightArrow)) aim = Vector2Int.right;
		if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftArrow)) aim = Vector2Int.left;
		if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.UpArrow)) aim = Vector2Int.up;
		if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.DownArrow)) aim = Vector2Int.down;

		if (aim != Vector2Int.zero)
		{
			lastAimDir = aim;
			ConfirmDirection(aim);
		}
	}
	private void ConfirmDirection(Vector2Int dir)
	{
		if (player == null || currentAbility == null)
		{
			return;
		}
		var ctx = new AbilityContext
		{
			board = boardController,
			gameLogic = logic,
			direction = dir,
			targetNode = null
		};
		gameplayManager.QueueAbility(player, currentAbility, ctx);
		StartCoroutine(boardController.ShiftAnimated(dir));
		FinishSelection();
	}
	#endregion
	#region Node mode
	private void StartNodeMode()
	{
		player = boardController.GetPlayer();
		currentNode = boardController.GetNodeAtPosition(player.transform.position);
		UpdateNodeHighLight();
	}

	private void UpdateNodeMode()
	{
		var nMouse = TryGetNodeUnderMouse();
		if(nMouse != null)
		{
			currentNode = nMouse;
			UpdateNodeHighLight();
		}
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			ConfirmNode();
		}
	}

	private void UpdateNodeHighLight()
	{
		foreach (var n in highlightedNodes)
			n.SetHighlighted(false);
		highlightedNodes.Clear();

		if (currentNode == null) return;
		currentNode.SetHighlighted(true);
		highlightedNodes.Add(currentNode);
	}
	private Node TryGetNodeUnderMouse()
	{
		var cam = Camera.main;
		if (cam == null) return null;
		var world = cam.ScreenToWorldPoint(Input.mousePosition);
		var p = new Vector2Int(Mathf.RoundToInt(world.x), Mathf.RoundToInt(world.y));
		return boardController.GetNodeAtPosition(p);
	}
	private async void ConfirmNode()
	{
		if (player == null || currentAbility == null || currentNode == null)
			return;
		var ctx = new AbilityContext
		{
			board = boardController,
			gameLogic = logic,
			direction = Vector2Int.zero,
			targetNode = currentNode
		};
		if(currentAbility.phase == CastPhase.Instant && !currentAbility.consumeTurn)
		{
			if (currentAbility.CanCast(player, ctx))
			{
				//StartCoroutine(CastImmediate(player, currentAbility, ctx));
				await CastImediate(player, currentAbility, ctx);

			}
		}
		else
		{
			gameplayManager.QueueAbility(player, currentAbility, ctx);	
		}
	}
	//private IEnumerator CastImmediate(PlayerEntity user, BaseCharacterAbility ability, AbilityContext context)
	//{
	//	gameplayManager.LockInput();
	//	Debug.Log(gameplayManager.IsInputLocked);
	//	yield return ability.OnCast(user, context);
	//	if (!user.abilities.ContainsKey(ability))
	//	{
	//		user.abilities[ability] = 0;
	//	}
	//	user.abilities[ability] = Mathf.Max(1, ability.cooldownTurns);
	//	gameplayManager.UnlockInput();
	//	Debug.Log(gameplayManager.IsInputLocked);
	//	FinishSelection();
	//}
	private async Task CastImediate(PlayerEntity user, BaseCharacterAbility ability, AbilityContext context)
	{
		gameplayManager.LockInput();
		Debug.Log(gameplayManager.IsInputLocked);
		await ability.OnCast(user, context);
		if (!user.abilities.ContainsKey(ability))
		{
			user.abilities[ability] = 0;
		}
		user.abilities[ability] = Mathf.Max(1, ability.cooldownTurns);
		gameplayManager.UnlockInput();
		Debug.Log(gameplayManager.IsInputLocked);
		FinishSelection();
	}
	private void CancelSelection()
	{
		FinishSelection();
	}
	private void FinishSelection()
	{
		currentAbility = null;
		currentNode = null;
		currentTarget = AbilityTarget.Instant;

		foreach (var n in highlightedNodes)
			n.SetHighlighted(false);
		highlightedNodes.Clear();
		MessageManager.Instance.SendMessage(new Message(ProjectMessageType.EndOfSkillRequireSelection));
		isSelecting = false;
	}

	#endregion
}