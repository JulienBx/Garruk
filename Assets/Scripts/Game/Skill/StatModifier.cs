using UnityEngine;

public class StatModifier
{
	public int Amount;                      // de combien ça modifie        
	public int Duration;                    // -1 permanent ou bien le nombre de tour
	public int Area;                        // Nombre de cases adjacentes
	public ModifierType Type;               // AddPermanent = 0
	public ModifierStat Stat;               // Attack=0, Move=1, Energy=2, Speed=3, Damage=4, capacité à lancer des sorts
	public bool Active;
	public int idIcon ;
	
	public string title ;
	public string description ;
	public string additionnalInfo ;

	public StatModifier()
	{

	}
	
	public StatModifier(int amount, ModifierType type, ModifierStat stat, int duration, int idIco, string t, string d, string a)
	{
		this.Amount = amount;
		this.Type = type;
		this.Stat = stat;
		this.Duration = duration;
		this.Active = true;
		this.idIcon = idIco ;
		this.title = t ;
		this.description = d ;
		this.additionnalInfo = a ;
	}
	

	public int modifyAttack(int attack)
	{
		if (Stat == ModifierStat.Stat_Attack)
		{
			if (Type == ModifierType.Type_BonusMalus)
			{
				attack += Amount;
			} else if (Type == ModifierType.Type_Multiplier)
			{
				attack = attack + Amount * attack / 100;
			}
		}
		return attack;
	}
	public int modifySpeed(int speed)
	{
		if (Stat == ModifierStat.Stat_Speed)
		{
			if (Type == ModifierType.Type_BonusMalus)
			{
				speed += Amount;
			} else if (Type == ModifierType.Type_Multiplier)
			{
				speed = speed + Amount * speed / 100;
			}
		}
		return speed;
	}
	public int modifyMove(int move)
	{
		if (Stat == ModifierStat.Stat_Move)
		{
			if (Type == ModifierType.Type_BonusMalus)
			{
				move += Amount;
			} else if (Type == ModifierType.Type_Multiplier)
			{
				move = move + Amount * move / 100;
			}
		}
		return move;
	}
	
	public int modifyLife(int life)
	{
		if (Stat == ModifierStat.Stat_Life)
		{
			if (Type == ModifierType.Type_BonusMalus)
			{
				life += Amount;
			} else if (Type == ModifierType.Type_Multiplier)
			{
				life = life + Amount * life / 100;
			}
		}
		if (Stat == ModifierStat.Stat_Dommage)
		{
			if (Type == ModifierType.Type_BonusMalus)
			{
				life -= Amount;
			} else if (Type == ModifierType.Type_Multiplier)
			{
				life = life - Amount * life / 100;
			}
		}
		return life;
	}
}
