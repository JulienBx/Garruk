﻿using UnityEngine;
using System.Collections.Generic;

public class Frenesie : GameSkill
{
	public Frenesie()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameView.instance.getGC().play();
	}
	
	public override void applyOn(){
		int amount = base.skill.ManaCost;
		int myCurrentLife = base.card.GetLife();
		int amount2 = Mathf.Min(amount, myCurrentLife);
		
		int target = GameView.instance.getGC().getCurrentPlayingCard();
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts = new List<string>();
		
		GameView.instance.getGC().addCardModifier(target, amount2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameView.instance.getGC().addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Frénésie", "+"+amount+" ATK", "Permanent");
		
		string text = "+"+amount+" ATK\n-"+amount+" PV";
		if(GameView.instance.getCard(target).GetLife()==amount2){
			text+="\nMORT";
		}
		GameView.instance.displaySkillEffect(target, text, 4);
		
		receivers.Add (GameView.instance.getCard(target));
		receiversTexts.Add(text);
		
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Frénésie</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
