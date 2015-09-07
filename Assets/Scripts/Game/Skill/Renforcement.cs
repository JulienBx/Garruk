using UnityEngine;
using System.Collections.Generic;

public class Renforcement : GameSkill
{
	public Renforcement()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			int arg = Random.Range(1,base.skill.ManaCost+1);
			GameController.instance.applyOn(target,arg);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 9, "Renforcement", "+"+arg+"ATK", "Actif 1 tour");
		GameView.instance.displaySkillEffect(target, "+"+arg+" ATK", 5);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = base.card.GetAttack();
		string text;
		
		if(attack==1){
			text = "ATK : "+attack+"->0";
		}
		else{
			text = "ATK : "+attack+"->"+(attack+1)+"-"+Mathf.Max (0,attack+amount);
		}
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int proba ;
		text += "HIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			text+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
		}
		else{
			proba = 100;
			text+=proba+"%";
		}
		
		return text ;
	}
}
