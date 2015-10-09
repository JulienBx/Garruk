using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
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
		int esquive = base.skill.ManaCost;
		int target = GameController.instance.getCurrentPlayingCard();
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		GameController.instance.addCardModifier(target, esquive, ModifierType.Type_EsquivePercentage, ModifierStat.Stat_No, -1, 1, "AGILITE", "Peut esquiver les dégats, Probabilité : "+esquive+"%. Permanent.", "Permanent");
		
		string text = "+"+esquive+"% esquive";
		GameView.instance.displaySkillEffect(target, text, 4);
		
		receivers.Add (GameView.instance.getCard(target));
		receiversTexts.Add(text);
		
		if(!GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Agilité</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
