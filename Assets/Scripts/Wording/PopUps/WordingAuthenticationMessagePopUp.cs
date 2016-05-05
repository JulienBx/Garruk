using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAuthenticationMessagePopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAuthenticationMessagePopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Continuer","Send"}); //0
		references.Add(new string[]{"Félicitations !!  \n \nVotre compte est créé, un lien d'activation a été envoyé à votre adresse mail.  \nEn cliquant sur ce lien vous pourrez activer votre compte et rejoindre Cristalia !","Congratulations !!  \n \nYour account has been created. Please check your mailbox for an activation email.  \nOnce you click on the activation link you may join us on Cristalia!"}); //1
		references.Add(new string[]{"Un lien permettant de réinitialiser votre mot de passe vient de vous être envoyé sur votre adresse mail.","You have been sent an email to modify your password. Please check your mailbox."}); //2
		references.Add(new string[]{"Un lien d'activation a été envoyé sur votre boite mail. Cliquez sur le lien pour accéder à Cristalia","Please check your mailbox for the activation link we have sent"}); //3
	}
}