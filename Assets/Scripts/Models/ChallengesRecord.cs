using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChallengesRecord
{
	public int Friend;
	public int NbWins;
	public int NbLooses;
	
	public ChallengesRecord()
	{
	}
	public ChallengesRecord(User friend, int nbwins, int nblooses)
	{
		this.Friend = friend;
		this.NbWins = nbwins;
		this.NbLooses = nblooses;
	}
}



