using UnityEngine;
using System.Collections;

[System.Serializable] 
public class CardType
{
	public string Name;
	public int Id;
	public string Description;
	public int Order;
	public int MinLife;
	public int MaxLife;
	public int MinAttack;
	public int MaxAttack;
	public int Rank;
	
	public CardType()
	{
	}
	public int getPictureId()
	{
		return this.Id;
	}
}



