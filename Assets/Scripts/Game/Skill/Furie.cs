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
		GameView.instance.getGC().play();
	}
	
	public override void applyOn(){
		int target = GameView.instance.getGC().getCurrentPlayingCard() ;
		string text = "Devient furieux!";
		
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts =  new List<string>();
		
		GameView.instance.getGC().addCardModifier(target, 0, ModifierType.Type_Crazy, ModifierStat.Stat_No, 50, 14, "Furieux", "Attaque à chaque tour le héros le plus proche. Permanent", "Actif 2 tours");	
		GameView.instance.displaySkillEffect(target, text, 5);
		
		receivers.Add(GameView.instance.getCard(GameView.instance.getGC().getCurrentPlayingCard()));
		receiversTexts.Add(text);
		
		if(!GameView.instance.getIsMine(GameView.instance.getGC().getCurrentPlayingCard())){
			GameView.instance.setSkillPopUp("lance <b>Furie</b>...", base.card, receivers, receiversTexts);
		}
	}
	
	public override string isLaunchable(){
		return "";
	}
}
