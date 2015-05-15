using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : MonoBehaviour
{
	public BottomZoneViewModel bottomZoneVM;
	public TopZoneViewModel topZoneVM ;
	public GameScreenViewModel gameScreenVM ;

	void Awake()
	{
		gameScreenVM = new GameScreenViewModel();
		gameScreenVM.recalculate();
	}
	
	void Start()
	{	
	
	}

	void Update()
	{

	}

	void OnGUI()
	{
		if (gameScreenVM.toDisplayStartWindows){
			GUILayout.BeginArea(gameScreenVM.startButtonRect);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.Label(gameScreenVM.messageStartWindow, gameScreenVM.centerMessageTextStyle);
					if (GUILayout.Button(gameScreenVM.messageStartWindowButton, gameScreenVM.centerMessageTextStyle)){
						GameController.instance.StartFight();
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
			GUILayout.BeginArea(gameScreenVM.opponentStartButtonRect);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.Label(gameScreenVM.messageOpponentStartWindow, gameScreenVM.centerMessageTextStyle);
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}

		if (gameScreenVM.hasAMessage)
		{
			GUILayout.BeginArea(gameScreenVM.centerMessageRect);
			{
				GUILayout.BeginHorizontal(gameScreenVM.centerMessageTextStyle);
				{
					GUILayout.Label(gameScreenVM.messageToDisplay, gameScreenVM.centerMessageTextStyle);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
		if (gameScreenVM.timer > 0)
		{
			GUILayout.BeginArea(gameScreenVM.rightMessageRect);
			{
				GUILayout.BeginHorizontal(gameScreenVM.rightMessageTextStyle);
				{
					GUILayout.Label(Mathf.Ceil(gameScreenVM.timer).ToString(), gameScreenVM.rightMessageTextStyle);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
	}
}

