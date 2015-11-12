using UnityEngine;
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
		GameController.instance.play();
	}
	
	public override void applyOn(){
		int amount ;
		if(base.skill.Level<=2){
			amount = 2 ;
		}
		else if(base.skill.Level<=4){
			amount = 3 ;
		}
		else if(base.skill.Level<=6){
			amount = 4 ;
		}
		else if(base.skill.Level<=8){
			amount = 5 ;
		}
		else {
			amount = 6 ;
		}
		
		int myCurrentLife = base.card.GetLife();
		int amount2 = Mathf.Min(amount, myCurrentLife);
		if(base.skill.Level<=1){
			amount2 = 10 ;
		}
		else if(base.skill.Level<=3){
			amount2 = 8 ;
		}
		else if(base.skill.Level<=5){
			amount2 = 6 ;
		}
		else if(base.skill.Level<=7){
			amount2 = 5 ;
		}
		else if(base.skill.Level<=9){
			amount2 = 4 ;
		}
		else {
			amount2 = 3 ;
		}
		
		int target = GameController.instance.getCurrentPlayingCard();
		List<Card> receivers =  new List<Card>();
		List<string> receiversTexts = new List<string>();
		
		GameController.instance.addCardModifier(target, amount2, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 20, "FRENESIE", "+"+amount+" ATK. Permanent.", "Permanent");
		
		string text = "+"+amount+" ATK\n-"+amount+" PV";
		if(GameView.instance.getCard(target).GetLife()==amount2){
			text+="\nMORT";
		}
		GameView.instance.displaySkillEffect(target, text, 4);
		
		receivers.Add (GameView.instance.getCard(target));
		receiversTexts.Add(text);
		
		GameView.instance.setSkillPopUp("lance <b>Frénésie</b>...", base.card, receivers, receiversTexts);
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
