using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameView : MonoBehaviour
{
	public GameViewModel gameVM ;
	
	void Awake()
	{
		this.gameVM = new GameViewModel();
	}

	void Update()
	{
		if (this.gameVM.widthScreen != Screen.width || this.gameVM.heightScreen != Screen.height)
		{
			this.gameVM.widthScreen = Screen.width ;
			this.gameVM.heightScreen = Screen.height ;
			GameController.instance.resize(this.gameVM.widthScreen, this.gameVM.heightScreen);
		}
		
		if (this.gameVM.timer!=-1)
		{
			this.gameVM.timer -= Time.deltaTime;
			if (this.gameVM.timer<0)
			{
				GameController.instance.timeOut();
			}
			else{
				int tempInt = Mathf.FloorToInt(this.gameVM.timer);
				if(tempInt!=this.gameVM.timerSeconds && tempInt < 10){
					this.gameVM.timerSeconds = tempInt;
					this.gameVM.timerSeconds = tempInt;
					if(tempInt < 10){
						GameController.instance.playAlarm();
					}
				}
			}
		}
	}

	void OnGUI()
	{	
		if (GUI.Button(gameVM.quitButtonRect, gameVM.quitButtonText, gameVM.quitButtonStyle))
		{
			GameController.instance.quitGameHandler();
		}	

		if (gameVM.toDisplayStartWindows)
		{
			GUILayout.BeginArea(gameVM.startButtonRect, this.gameVM.startWindowStyle);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(gameVM.messageStartWindow, gameVM.startWindowTextStyle);
					if (!this.gameVM.haveIStarted)
					{
						GUILayout.BeginHorizontal();
						{
							GUILayout.FlexibleSpace();
							if (GUILayout.Button(gameVM.messageStartWindowButton, gameVM.startButtonTextStyle, GUILayout.Width(gameVM.startButtonRect.width*0.8f)))
							{
								GameController.instance.playerReady();
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
				
			GUILayout.BeginArea(gameVM.opponentStartButtonRect, this.gameVM.opponentStartWindowStyle);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Label(gameVM.messageOpponentStartWindow, gameVM.opponentStartWindowTextStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea();
		}

		if (gameVM.timerSeconds >= 0)
		{
			GUILayout.BeginArea(gameVM.timerRect);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(gameVM.timerSeconds.ToString(), gameVM.timerStyle);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
	}
}

