using UnityEngine;
using System.Collections.Generic;

public class Furtivite : GameSkill
{
	public Furtivite()
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
		int attackBonus = this.skill.ManaCost;
		int target = GameView.instance.getGC().getCurrentPlayingCard() ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		string text = "Invisible\n+"+attackBonus+" ATK";
		
		GameView.instance.getGC().addCardModifier(target, 0, ModifierType.Type_Intouchable, ModifierStat.Stat_No, 2, 20, "Invisible", "Ne peut pas etre ciblé par une attaque ou compétence", "Actif 2 tours");
		GameView.instance.getGC().addCardModifier(target, attackBonus, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 9, "Renforcement", "Attaque augmentée de "+attackBonus+" pour un tour", "Actif 2 tours");
		
		receivers.Add(GameView.instance.getCard(GameView.instance.getGC().getCurrentPlayingCard()));
		receiversTexts.Add(text);
		
		GameView.instance.displaySkillEffect(target, text, 4);
		
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Furtivité</b>...", base.card, receivers, receiversTexts);
		}
		this.skill.nbLeft--;
	}
	
	public override string isLaunchable(){
		string s = "";
		int nbLeft = 0 ; 
		Card c = GameView.instance.getCard(GameView.instance.getGC().getCurrentPlayingCard());
		for (int i = 0 ; i < c.Skills.Count ; i++){
			if (c.Skills[i].Id==11){
				nbLeft = c.Skills[i].nbLeft;
			}
		}
		if (nbLeft==0){
			s = "Déjà utilisé pendant le combat";
		}
		return s ; 
	}
}
