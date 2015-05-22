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
	}
	
	void Start()
	{	
	
	}

	void Update()
	{

	}

	void OnGUI()
	{
		if (this.gameScreenVM.toDisplayGameScreen)
		{
			if (this.gameScreenVM.toDisplayPlayingCard){
				GUI.Label(gameScreenVM.namePlayingCardRect, gameScreenVM.myPlayingCardName, gameScreenVM.namePlayingCardTextStyle );
			}
			if (this.gameScreenVM.toDisplayOpponentPlayingCard){
				GUI.Label(gameScreenVM.nameOpponentPlayingCardRect, gameScreenVM.hisPlayingCardName, gameScreenVM.nameOpponentPlayingCardTextStyle );
			}
			if (this.gameScreenVM.toDisplayQuitButton)
			{
				if (GUI.Button(gameScreenVM.quitButtonRect, gameScreenVM.quitButtonText, gameScreenVM.quitButtonStyle))
				{
					GameController.instance.quitGameHandler();
				}
			}
		
			if (gameScreenVM.toDisplayStartWindows)
			{
				GUILayout.BeginArea(gameScreenVM.startButtonRect, this.gameScreenVM.startWindowStyle);
				{
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						if (this.gameScreenVM.iHaveStarted)
						{
							GUILayout.Label(gameScreenVM.messageStartWindow, gameScreenVM.whiteSmallTextStyle);
							GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Label(gameScreenVM.messageStartWindowButton, gameScreenVM.greenInformationTextStyle, GUILayout.Width(gameScreenVM.startButtonRect.width / 3f));
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndHorizontal();
						} else
						{
							GUILayout.Label(gameScreenVM.messageStartWindow, gameScreenVM.whiteSmallTextStyle);
							GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
							{
								GUILayout.FlexibleSpace();
								if (GUILayout.Button(gameScreenVM.messageStartWindowButton, gameScreenVM.buttonTextStyle, GUILayout.Width(gameScreenVM.startButtonRect.width / 2f)))
								{
									GameController.instance.StartFight();
								}
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndHorizontal();
						}
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndArea();
				GUILayout.BeginArea(gameScreenVM.opponentStartButtonRect, this.gameScreenVM.startWindowStyle);
				{
					GUILayout.BeginVertical();
					{
						GUILayout.FlexibleSpace();
						if (this.gameScreenVM.heHasStarted)
						{
							GUILayout.Label(gameScreenVM.messageOpponentStartWindow, gameScreenVM.whiteSmallTextStyle);
							GUILayout.FlexibleSpace();
							GUILayout.BeginHorizontal();
							{
								GUILayout.FlexibleSpace();
								GUILayout.Label(gameScreenVM.messageStartWindowButton, gameScreenVM.greenInformationTextStyle, GUILayout.Width(gameScreenVM.startButtonRect.width / 4f));
								GUILayout.FlexibleSpace();
							}
							GUILayout.EndHorizontal();
						} else
						{
							GUILayout.Label(gameScreenVM.messageOpponentStartWindow, gameScreenVM.whiteSmallTextStyle);
						}
						GUILayout.FlexibleSpace();
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
}

