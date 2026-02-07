using System;
using System.Text.RegularExpressions;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000846 RID: 2118
	public static class EmailFormatValidator
	{
		// Token: 0x06003BF3 RID: 15347 RVA: 0x0004ED2C File Offset: 0x0004CF2C
		public static bool IsValidEmail(string emailCandidate)
		{
			return Regex.IsMatch(emailCandidate, "^(?(\")(\"[^\"]+?\"@)|(([0-9a-zA-Z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-zA-Z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,6}))$");
		}
	}
}
