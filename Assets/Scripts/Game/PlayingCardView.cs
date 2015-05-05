using UnityEngine ;

public class PlayingCardView : MonoBehaviour
{
	public PlayingCardViewModel playingCardVM;

	public PlayingCardView ()
	{
		this.playingCardVM = new PlayingCardViewModel();
	}

	public void replace ()
	{
		gameObject.transform.localPosition = playingCardVM.position;
		gameObject.transform.localRotation = playingCardVM.rotation;
		gameObject.transform.localScale = playingCardVM.scale;
	}

	void Update ()
	{
	}

	void OnGUI ()
	{
		GUILayout.BeginArea (this.playingCardVM.infoRect);
		{
			GUILayout.BeginHorizontal(this.playingCardVM.backgroundStyle);
			{
				GUILayout.BeginVertical();
				{
					GUILayout.Label(this.playingCardVM.name, this.playingCardVM.nameTextStyle, GUILayout.Width(this.playingCardVM.skillInfoRectWidth), GUILayout.Height(this.playingCardVM.infoRect.height*12/100));
					GUILayout.FlexibleSpace();
					GUILayout.Box(this.playingCardVM.picture, this.playingCardVM.imageStyle, GUILayout.Width(this.playingCardVM.skillInfoRectWidth), GUILayout.Height(this.playingCardVM.infoRect.height*4/10));
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal(GUILayout.Height(this.playingCardVM.infoRect.height*1/10));
					{
						GUILayout.Label("Life", this.playingCardVM.attackZoneTextStyle, GUILayout.Width(this.playingCardVM.skillInfoRectWidth*3/10));
						GUILayout.BeginHorizontal(this.playingCardVM.backgroundLifeBar, GUILayout.Width(this.playingCardVM.skillInfoRectWidth*65/100));
						{
							GUILayout.Label(""+this.playingCardVM.life, this.playingCardVM.lifeTextStyle, GUILayout.Width(0.65f*this.playingCardVM.life*this.playingCardVM.skillInfoRectWidth/this.playingCardVM.maxLife));
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal(GUILayout.Height(this.playingCardVM.infoRect.height*1/10), GUILayout.Width(this.playingCardVM.skillInfoRectWidth));
					{
						GUILayout.Label("Speed", this.playingCardVM.attackZoneTextStyle, GUILayout.Width(this.playingCardVM.skillInfoRectWidth*3/10));
						GUILayout.BeginHorizontal(this.playingCardVM.backgroundLifeBar, GUILayout.Width(this.playingCardVM.skillInfoRectWidth*65/100));
						{
							GUILayout.Label(""+this.playingCardVM.quickness, this.playingCardVM.lifeTextStyle, GUILayout.Width(0.65f*this.playingCardVM.quickness*this.playingCardVM.skillInfoRectWidth/100));
							GUILayout.FlexibleSpace();
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal(GUILayout.Height(this.playingCardVM.infoRect.height*15/100), GUILayout.Width(this.playingCardVM.skillInfoRectWidth));
					{
						GUILayout.FlexibleSpace();
						GUILayout.Box(this.playingCardVM.attackIcon, this.playingCardVM.imageStyle, GUILayout.Height(this.playingCardVM.infoRect.height*15/100));
						GUILayout.Space(this.playingCardVM.skillInfoRectWidth*5f/100f);
						GUILayout.Label(this.playingCardVM.attack, this.playingCardVM.attackZoneTextStyle);
						GUILayout.FlexibleSpace();
						GUILayout.Box(this.playingCardVM.moveIcon, this.playingCardVM.imageStyle, GUILayout.Height(this.playingCardVM.infoRect.height*15/100));
						GUILayout.Space(this.playingCardVM.skillInfoRectWidth*5f/100f);
						GUILayout.Label(this.playingCardVM.move, this.playingCardVM.attackZoneTextStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();

				if (this.playingCardVM.isSelected){
					GUILayout.BeginVertical(GUILayout.Width(this.playingCardVM.skillInfoRectWidth));
					{
						GUILayout.Label("NAME", this.playingCardVM.nameTextStyle, GUILayout.Width(this.playingCardVM.skillInfoRectWidth), GUILayout.Height(this.playingCardVM.infoRect.height*12/100));
					}
					GUILayout.EndVertical();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea ();

		GUILayout.BeginArea (this.playingCardVM.infoRect);
		{
			if (GUILayout.Button("", this.playingCardVM.emptyButtonStyle)){
				gameObject.GetComponentInChildren<PlayingCardController>().clickPlayingCard();
			}
		}
		GUILayout.EndArea ();
		
		if(Input.mousePosition.x > this.playingCardVM.infoRect.xMin && (Screen.height-Input.mousePosition.y) > this.playingCardVM.infoRect.yMin && Input.mousePosition.x < this.playingCardVM.infoRect.xMax && (Screen.height-Input.mousePosition.y) < this.playingCardVM.infoRect.yMax){
			gameObject.GetComponentInChildren<PlayingCardController>().hoverPlayingCard();
		}
	}

	void OnMouseEnter(){
		//print ("HoverPlayingCard");
		//gameObject.GetComponentInChildren<PlayingCardController>().HoverTile();
	}

	void OnMouseDown(){
		//gameObject.GetComponentInChildren<PlayingCardController>().drag();
	}

	void OnMouseUp(){
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


