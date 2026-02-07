using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020008D6 RID: 2262
	public abstract class EndpointWithPaginatedResponse<T> : Endpoint<PaginatedResult<T>> where T : class
	{
		// Token: 0x06003D7E RID: 15742 RVA: 0x0004F8D6 File Offset: 0x0004DAD6
		public EndpointWithPaginatedResponse(OAuthGate oauthGate, bool mustUsePrivateScope)
			: base(mustUsePrivateScope, oauthGate)
		{
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x00157F94 File Offset: 0x00156194
		protected Action<Action<PaginatedResult<T>, WebError>> _LinkSetter(string url)
		{
			if (url == null)
			{
				return null;
			}
			return delegate(Action<PaginatedResult<T>, WebError> callback)
			{
				this._HTTPRequest = null;
				this._HTTPResponse = null;
				this._Execute(url, callback);
			};
		}
	}
}
