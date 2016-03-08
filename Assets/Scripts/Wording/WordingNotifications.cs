using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNotifications
{
	public static IList<string[]> references;
	public static IList<string[]> contents;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getContent(int idNotificationType)
	{
		return contents[idNotificationType][ApplicationModel.player.IdLanguage];
	}
	static WordingNotifications()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Nouveau !","New !"}); //0

		contents=new List<string[]>();
		contents.Add(new string[]{"#*communication*#","#*communication*#"}); //0
		contents.Add(new string[]{"a acheté votre #*card*# pour #*value*# cristaux","has acquired your #*card*#. You earn #*value*# cristals"}); //1
		contents.Add(new string[]{"a accepté votre demande et est désormais votre ami","has accepted your friend request"}); //2
		contents.Add(new string[]{"souhaite faire partie de vos amis. Accepter ?","wants you to become his friend. Do you agree ?"}); //3
	}
}