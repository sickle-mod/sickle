using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

// Token: 0x02000138 RID: 312
public class MailSender
{
	// Token: 0x06000954 RID: 2388 RVA: 0x0007B2E4 File Offset: 0x000794E4
	public MailSender(string username, string password, string smtpServerAddress, int port = 25, bool enableSSL = false, bool useDefaultCredentials = false)
	{
		this.mSmtpServer = new SmtpClient(smtpServerAddress);
		this.mSmtpServer.EnableSsl = enableSSL;
		this.mSmtpServer.UseDefaultCredentials = useDefaultCredentials;
		this.mSmtpServer.Port = port;
		this.mSmtpServer.Credentials = new NetworkCredential(username, password);
		if (enableSSL)
		{
			ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
		}
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0007B364 File Offset: 0x00079564
	public void SendMail(string from, string fromName, string to, string subject, string body, SendCompletedEventHandler onComplete = null, string CCMail = "")
	{
		MailMessage mailMessage = new MailMessage();
		mailMessage.From = new MailAddress(from, fromName);
		mailMessage.To.Add(to);
		mailMessage.Subject = subject;
		mailMessage.Body = body;
		if (CCMail != "")
		{
			mailMessage.CC.Add(CCMail);
		}
		if (onComplete != null)
		{
			this.mSmtpServer.SendCompleted += onComplete;
		}
		this.mSmtpServer.SendAsync(mailMessage, null);
	}

	// Token: 0x04000883 RID: 2179
	private SmtpClient mSmtpServer;
}
