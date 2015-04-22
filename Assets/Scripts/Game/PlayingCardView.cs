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

	void OnGUI ()
	{
		GUILayout.BeginArea (this.playingCardVM.infoRect);
		{
			GUILayout.BeginVertical(this.playingCardVM.backgroundStyle);
			{
				GUILayout.Label(this.playingCardVM.name, this.playingCardVM.nameTextStyle);
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(""+this.playingCardVM.life, this.playingCardVM.lifeTextStyle, GUILayout.Width(this.playingCardVM.life*this.playingCardVM.infoRect.width/this.playingCardVM.maxLife));
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					GUILayout.Box(this.playingCardVM.attackIcon, this.playingCardVM.imageStyle, GUILayout.Height(this.playingCardVM.infoRect.height/3));
					GUILayout.Label(this.playingCardVM.attack, this.playingCardVM.attackZoneTextStyle);
					GUILayout.FlexibleSpace();
					GUILayout.Box(this.playingCardVM.moveIcon, this.playingCardVM.imageStyle, GUILayout.Height(this.playingCardVM.infoRect.height/3));
					GUILayout.Label(this.playingCardVM.move, this.playingCardVM.moveZoneTextStyle);
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea ();
	}

	void OnMouseEnter(){
		print ("Je survole "+gameObject.GetComponentInChildren<PlayingCardController>().ID);	
	}

	void OnMouseDown(){
		gameObject.GetComponentInChildren<PlayingCardController>().drag();
	}

	void OnMouseUp(){
		gameObject.GetComponentInChildren<PlayingCardController>().release();
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


