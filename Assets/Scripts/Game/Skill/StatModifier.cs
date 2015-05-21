using UnityEngine;

public class StatModifier
{
	public int Amount;                    // de combien ça modifie        
	public int Duration;                  // -1 permanent ou bien le nombre de tour
	public int Area;                        // Nombre de cases adjacentes
	public ModifierType Type;               // AddPermanent = 0
	public ModifierStat Stat;               // Attack=0, Move=1, Energy=2, Speed=3, Damage=4, capacité à lancer des sorts

	public StatModifier()
	{

	}

	public StatModifier(int amount, ModifierType type, ModifierStat stat)
	{
		this.Amount = amount;
		this.Type = type;
		this.Stat = stat;
	}
}
