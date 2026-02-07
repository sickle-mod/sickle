using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Scythe.Multiplayer;
using UnityEngine;

// Token: 0x020000FC RID: 252
public static class EmailFactory
{
	// Token: 0x06000840 RID: 2112 RVA: 0x00077DC4 File Offset: 0x00075FC4
	public static void SendMailGmail(string mailText)
	{
		MailMessage mailMessage = new MailMessage();
		mailMessage.From = new MailAddress("feedbacktestscythe@gmail.com");
		mailMessage.To.Add("feedbacktestscythe@gmail.com");
		mailMessage.Subject = "Scythe feedback";
		mailMessage.Body = mailText + EmailFactory.AddPlayerInfo();
		SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
		smtpClient.Port = 587;
		smtpClient.Credentials = new NetworkCredential("feedbacktestscythe@gmail.com", "feedback123");
		smtpClient.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
		smtpClient.Send(mailMessage);
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00077E70 File Offset: 0x00076070
	private static string AddPlayerInfo()
	{
		string text = "Player info:  \n  ";
		text += SystemInfo.deviceModel;
		if (AsmodeeLogic.Instance.GetUser().LoginName != null)
		{
			text = text + AsmodeeLogic.Instance.GetUser().LoginName + "\n";
		}
		if (AsmodeeLogic.Instance.GetUser().UserId != 0)
		{
			text = text + AsmodeeLogic.Instance.GetUser().UserId.ToString() + "\n";
		}
		return text;
	}
}
