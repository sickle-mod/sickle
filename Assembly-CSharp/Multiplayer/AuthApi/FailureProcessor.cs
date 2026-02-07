using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Multiplayer.AuthApi
{
	// Token: 0x020001A3 RID: 419
	public static class FailureProcessor
	{
		// Token: 0x06000C52 RID: 3154 RVA: 0x00080C5C File Offset: 0x0007EE5C
		public static object GetResponse(object response)
		{
			if (response == null)
			{
				return new FailureResponse
				{
					Result = Result.Error,
					Error = Error.UnspecifiedError
				};
			}
			ErrorResponse errorResponse = response as ErrorResponse;
			if (errorResponse != null)
			{
				return new FailureResponse
				{
					Result = errorResponse.Result,
					Error = errorResponse.Error,
					ValidationErrors = errorResponse.ValidationErrors
				};
			}
			UnauthorizedResponse unauthorizedResponse = response as UnauthorizedResponse;
			if (unauthorizedResponse != null)
			{
				return new FailureResponse
				{
					Result = unauthorizedResponse.Result
				};
			}
			ServerErrorResponse serverErrorResponse = response as ServerErrorResponse;
			if (serverErrorResponse == null)
			{
				return new FailureResponse
				{
					Result = Result.Error
				};
			}
			return new FailureResponse
			{
				Result = serverErrorResponse.Result
			};
		}
	}
}
