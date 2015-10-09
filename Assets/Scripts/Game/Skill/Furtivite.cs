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
		GameController.instance.play();
	}
	
	public override void applyOn(){
		int attackBonus = this.skill.ManaCost;
		int target = GameController.instance.getCurrentPlayingCard() ;
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		string text = "Invisible\n+"+attackBonus+" ATK";
		
		GameController.instance.addCardModifier(target, 0, ModifierType.Type_Intouchable, ModifierStat.Stat_No, 2, 52, "INTOUCHABLE", "Ne peut pas etre ciblé par une attaque ou compétence", "Actif 2 tours");
		GameController.instance.addCardModifier(target, attackBonus, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 18, "FORTIFIE", "+"+attackBonus+" ATK. Actif 2 tours", "");
		
		receivers.Add(GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()));
		receiversTexts.Add(text);
		
		GameView.instance.displaySkillEffect(target, text, 4);
		
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Furtivité</b>...", base.card, receivers, receiversTexts);
		}
		this.skill.nbLeft--;
	}
	
	public override string isLaunchable(){
		string s = "";
		int nbLeft = 0 ; 
		Card c = GameView.instance.getCard(GameController.instance.getCurrentPlayingCard());
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
