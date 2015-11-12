using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
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
		int target = GameController.instance.getCurrentPlayingCard() ;
		string text = "Devient furieux!";
		int amount = (int)Mathf.Floor((5 * base.skill.Level * base.card.GetTotalLife()) / 100);
		
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		GameController.instance.addCardModifier(target, -1*amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");	
		GameController.instance.addCardModifier(target, 0, ModifierType.Type_Crazy, ModifierStat.Stat_No, -1, 51, "FURIE", "Incontrolable, de déplace et attaque à chaque tour. Permanent", "");	
		GameView.instance.displaySkillEffect(target, text, 5);
		
		receivers.Add(GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()));
		receiversTexts.Add(text);
		
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Furie</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return "";
	}
}
