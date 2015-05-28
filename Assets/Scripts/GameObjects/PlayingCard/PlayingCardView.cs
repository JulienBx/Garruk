using UnityEngine ;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardView : MonoBehaviour
{
	public PlayingCardViewModel playingCardVM;
	int displaySkillDescription = -1;

	public PlayingCardView()
	{
		this.playingCardVM = new PlayingCardViewModel();
	}

	public void replace()
	{
		gameObject.transform.localPosition = playingCardVM.position;
		gameObject.transform.localScale = playingCardVM.scale;
	}

	void Update()
	{

	}
	public void show()
	{
		transform.renderer.materials [1].mainTexture = playingCardVM.face; 
		transform.Find("LifeArea").FindChild("Life").renderer.materials [0].mainTexture = playingCardVM.lifeGauge;
		transform.Find("MoveArea").FindChild("Move").GetComponent<TextMesh>().text = playingCardVM.move;
		transform.Find("AttackArea").FindChild("Attack").GetComponent<TextMesh>().text = playingCardVM.attack;
	}
	public void setTextResolution(float resolution)
	{
		transform.Find("MoveArea").FindChild("Move").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 30);	
		transform.Find("MoveArea").FindChild("Move").localScale = new Vector3(0.3f / resolution, 0.3f / resolution, 0);
		transform.Find("AttackArea").FindChild("Attack").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 30);	
		transform.Find("AttackArea").FindChild("Attack").localScale = new Vector3(0.3f / resolution, 0.3f / resolution, 0);
	}
	public void drawLifeGauge(float percentage)
	{
		Vector3 scale = new Vector3(0.95f * percentage, 0.6f, 1f);
		Vector3 position = new Vector3(0.5f * 0.95f * (percentage - 1f), 0, -0.01f);
		gameObject.transform.Find("LifeArea").FindChild("Life").localScale = scale;
		gameObject.transform.Find("LifeArea").FindChild("Life").localPosition = position;
	}

	public void changeBorder()
	{
		renderer.materials [0].mainTexture = this.playingCardVM.border;
	}
	
	public void changeBackground()
	{
		renderer.materials [1].mainTexture = this.playingCardVM.background;
	}
	
	void OnMouseEnter()
	{
		if (this.playingCardVM.isActive)
		{
			gameObject.GetComponentInChildren<PlayingCardController>().hoverPlayingCard();
		}
		Debug.Log("nom :" + gameObject.GetComponentInChildren<PlayingCardController>().card.Title);
		Debug.Log("move :" + gameObject.GetComponentInChildren<PlayingCardController>().card.GetMove());
	}

	void OnMouseDown()
	{
		if (this.playingCardVM.isActive)
		{
			gameObject.GetComponentInChildren<PlayingCardController>().clickPlayingCard();
		}
	}

	void OnMouseUp()
	{
		if (this.playingCardVM.isActive)
		{
			gameObject.GetComponentInChildren<PlayingCardController>().releaseClickPlayingCard();
		}
		//		PlayingCardController pcc = gameObject.GetComponentInChildren<PlayingCardController>();
//
//		pcc.release();
//		if (GameController.instance.onGoingAttack)
//		{
//			pcc.getDamage();
//			GameController.instance.setStateOfAttack(false);
//			Debug.Log("total damage : " + pcc.damage);
//		}
	}

//				GUILayout.BeginHorizontal(statsZoneStyle, GUILayout.Width(stats.width));
//						{
//							// JBU			GUILayout.Label("", lifebarPoliceStyle, GUILayout.Width(stats.width*gnCard.currentLife/this.gnCard.card.Life), GUILayout.Height(stats.height/2));
//						}
//						GUILayout.EndHorizontal();
//					}
//					GUILayout.EndArea ();
//					
//					GUILayout.BeginArea (stats);
//					{
//						GUILayout.BeginVertical();
//						{
//							if (!isFocused){
//								// JBU				if(GUILayout.Button (this.gnCard.card.Title,NamePoliceStyle)){
//								// JBU					isFocused = true ;
//								// JBU				}
//							}
//							else{
//								// JBU				if (GUILayout.Button (gnCard.currentLife+"/"+this.gnCard.card.Life,LifePoliceStyle)){
//								// JBU					isFocused = false ;
//								// JBU				}
//							}
//							GUILayout.BeginHorizontal(statsZoneStyle);
//							{
//								GUILayout.FlexibleSpace();
//								GUILayout.BeginVertical();
//								{
//									GUILayout.FlexibleSpace();
//									GUILayout.Box (attackIcon, AttackPoliceStyle);
//									GUILayout.FlexibleSpace();
//								}
//								GUILayout.EndVertical();
//								GUILayout.FlexibleSpace();
//								// JBU			GUILayout.Label (""+gnCard.currentAttack,MovePoliceStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.BeginVertical();
//								{
//									GUILayout.FlexibleSpace();
//									GUILayout.Box (quicknessIcon,AttackPoliceStyle);
//									GUILayout.FlexibleSpace();
//								}
//								GUILayout.EndVertical();
//								GUILayout.FlexibleSpace();
//								// JBU		GUILayout.Label (""+gnCard.currentSpeed,MovePoliceStyle);
//								GUILayout.FlexibleSpace();
//								GUILayout.BeginVertical();
//								{
//									GUILayout.FlexibleSpace();
//									GUILayout.Box (moveIcon, AttackPoliceStyle);
//									GUILayout.FlexibleSpace();
//								}
//								GUILayout.EndVertical();
//								GUILayout.FlexibleSpace();
//								// JBU			GUILayout.Label (""+gnCard.currentMove,MovePoliceStyle);
//								GUILayout.FlexibleSpace();
//							}
//							GUILayout.EndHorizontal();
//							
//						}
//						GUILayout.EndVertical();
//					}
//					GUILayout.EndArea ();
//					
//					if (isHovered){
//						int j = 0 ;
//						GUI.depth=2;
//						for (int i = 0 ; i < 4 ; i++){
//							// JBU			if (this.gnCard.card.Skills[i].IsActivated==1){
//							// JBU					GUI.Label (new Rect(stats.x, stats.yMax+j*stats.height/2, stats.width, stats.height/2), this.gnCard.card.Skills[i].Level+"."+this.gnCard.card.Skills[i].Name, skillInfoStyle);
//							j++ ; 
//							// JBU}
//						}
//						GUI.depth=0;
//					}
//				}
//				// JBU}
//			}
}


