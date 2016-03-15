using UnityEngine;
using System.Collections;

namespace Xsolla {

	public class CCPayment : XsollaPaymentImpl {

		public delegate void OnNextStepRequired(XsollaForm form);
		public delegate void OnPaymentSuccess(XsollaStatus paymentStatus);
//		public delegate void OnError(XsollaError error);

		public event OnNextStepRequired NextStepRecieved;
		public event OnPaymentSuccess PaymentSuccessRecieved;
//		public event OnError ErrorRecieved;

		public string _cardNumber;
		public string _cardExpMonth;
		public string _cardExpYear;
		public string _cardCvv;
		public string _cardZip;
		public string _cardHolder;

		public CCPayment(){
		}

		public CCPayment(string accessToken, string cardNumber, string cardExpMonth, string cardExpYear,
		                 string cardCvv, string cardZip, string cardHolder)
		{
			this._accessToken = accessToken;
			this._cardNumber = cardNumber;
			this._cardExpMonth = cardExpMonth;
			this._cardExpYear = cardExpYear;
			this._cardCvv = cardCvv;
			this._cardZip = cardZip;
			this._cardHolder = cardHolder;
		}

		public void SetParams(string cardNumber, string cardExpMonth, string cardExpYear,
		                      string cardCvv, string cardZip, string cardHolder)
		{
			this._cardNumber = cardNumber;
			this._cardExpMonth = cardExpMonth;
			this._cardExpYear = cardExpYear;
			this._cardCvv = cardCvv;
			this._cardZip = cardZip;
			this._cardHolder = cardHolder;
		}


		public void InitPaystation(){
			base.FormReceived += OnFormReceived;
			base.StatusReceived += OnStatusReceived;
			base.ErrorReceived += OnErrorReceived;
			base.StartPaymentWithoutUtils (XsollaWallet.Factory.CreateWallet(_accessToken));
		}

		public new void OnFormReceived(XsollaForm form){
			XsollaForm.CurrentCommand command = form.GetCurrentCommand ();
			switch (command) {
				case XsollaForm.CurrentCommand.FORM:
						XsollaError error = form.GetError();
						if (!form.IsValidPaymentSystem ()) {
							OnErrorReceived (XsollaError.GetUnsuportedError ());
						} else if (error == null) {
							form.UpdateElement (XsollaApiConst.CARD_NUMBER, _cardNumber);
							form.UpdateElement (XsollaApiConst.CARD_EXP_YEAR, _cardExpYear);
							form.UpdateElement (XsollaApiConst.CARD_EXP_MONTH, _cardExpMonth);
							form.UpdateElement (XsollaApiConst.CARD_CVV, _cardCvv);
							form.UpdateElement (XsollaApiConst.CARD_ZIP, _cardZip);
							form.UpdateElement (XsollaApiConst.CARD_HOLDER, _cardHolder);
							NextStep (form.GetXpsMap());
						} else {
							OnErrorReceived (error);
						}
					break;
				case XsollaForm.CurrentCommand.CHECK:
					if(form.GetItem(XsollaApiConst.CARD_ZIP) != null)
					{
						form.UpdateElement (XsollaApiConst.CARD_ZIP, _cardZip);
						NextStep (form.GetXpsMap());
					} else {
						OnNextStepRecieved(form);
					}
					break;
				case XsollaForm.CurrentCommand.ACCOUNT:
				case XsollaForm.CurrentCommand.STATUS:
				case XsollaForm.CurrentCommand.CREATE:
				default:
					break;
			}

		}

		public void OnStatusReceived(XsollaStatus status){
			OnPaymentSuccessRecieved (status);
		}

		public new void OnErrorReceived(XsollaError error){
			
		}

		protected virtual void OnNextStepRecieved(XsollaForm form) 
		{
			if (NextStepRecieved != null)
				NextStepRecieved(form);
		}
		
		protected virtual void OnPaymentSuccessRecieved(XsollaStatus status) 
		{
			if (PaymentSuccessRecieved != null)
				PaymentSuccessRecieved(status);
		}
		
//		protected virtual void OnErrorReceived(XsollaError error) 
//		{
//			if (ErrorRecieved != null)
//				ErrorRecieved(error);
//		}

	}

}
