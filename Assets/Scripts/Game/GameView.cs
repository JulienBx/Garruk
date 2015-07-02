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
				if(tempInt!=this.gameVM.timerSeconds){
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
		GUILayout.BeginArea(gameVM.topCenterRect);
		{
			GUILayout.BeginVertical();
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					if (gameVM.timerSeconds >= 0)
					{
						GUILayout.FlexibleSpace();
						if (gameVM.timerSeconds > 9)
						{
							GUILayout.Label(gameVM.timerSeconds.ToString(), gameVM.timerStyle);
							GUILayout.Label("BLABLABLA", gameVM.timerStyle);
						}
						else{
							GUILayout.Label("0"+gameVM.timerSeconds.ToString(), gameVM.timerStyle);
						}
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("", gameVM.quitButtonStyle, GUILayout.Height(gameVM.timerStyle.fontSize), GUILayout.Width(gameVM.timerStyle.fontSize)))
					{
						GameController.instance.quitGameHandler();
					}
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
		
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
	}
}

