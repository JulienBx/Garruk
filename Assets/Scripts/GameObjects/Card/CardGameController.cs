//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//public class CardGameController : CardController
//{
//
//	public CardGameView cardGameView;
//
//	public void setGameCard(Card c)
//	{
//		base.setCard (c);
//		base.setExperience ();
//		base.setSkills ();
//		base.show ();
//	}
//	public void resetGameCard(Card c)
//	{
//		this.eraseCard ();
//		this.setGameCard (c);
//	}
//	public override void eraseCard()
//	{
//		base.eraseCard ();
//	}
//	public override void updateExperience()
//	{
//		base.updateExperience ();
//		EndSceneController.instance.incrementXpDrawn ();
//	}
//	public override void updateCardXpLevel()
//	{
//		this.cardGameView = gameObject.AddComponent <CardGameView>();
//		cardGameView.cardGameVM.styles=new GUIStyle[ressources.gameEndSceneStyles.Length];
//		for(int i=0;i<ressources.gameEndSceneStyles.Length;i++)
//		{
//			cardGameView.cardGameVM.styles[i]=ressources.gameEndSceneStyles[i];
//		}
//		cardGameView.cardGameVM.initStyles();
//		cardGameView.cardGameVM.nextLevelRect = new Rect(base.GOPosition.x-base.GOSize.x/2f,
//		                                                 (Screen.height-base.GOPosition.y)+base.GOSize.y/2f,
//		                                                 base.GOSize.x,
//		                                                 base.GOSize.y/3f);
//	}
//	public override void resize()
//	{
//		base.resize();
//		if(cardGameView!=null)
//		{
//			this.cardGameView.cardGameVM.resize(base.GOSize.y);
//		}
//	}
//}
//
