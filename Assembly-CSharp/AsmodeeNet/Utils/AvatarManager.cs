using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using BestHTTP;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200082A RID: 2090
	public class AvatarManager : MonoBehaviour
	{
		// Token: 0x06003B42 RID: 15170 RVA: 0x0004E58C File Offset: 0x0004C78C
		public void ClearCache()
		{
			this._userIdToAvatarURL.Clear();
			this._cachedAvatars.Clear();
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0004E5A4 File Offset: 0x0004C7A4
		public void UpdateCacheURLForUserId(int userId, string url)
		{
			if (userId <= 0)
			{
				throw new ArgumentException("UpdateCacheURLForUserId called with invalid userId");
			}
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentException("UpdateCacheURLForUserId called with empty url");
			}
			this._userIdToAvatarURL[userId] = url;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x001529A0 File Offset: 0x00150BA0
		public AvatarManager.RetrievalHandle CacheURLForUserIds(int[] userIds, Action<bool> onCompletion, AvatarManager.RetrievalHandle handle = null)
		{
			if (userIds == null || userIds.Length == 0)
			{
				throw new ArgumentException("CacheURLForUserIds called without userIds");
			}
			if (handle == null)
			{
				handle = new AvatarManager.RetrievalHandle();
			}
			AsmoLogger.Debug("AvatarManager", "Searching for avatar URLs", new Hashtable { { "userIds", userIds } });
			handle.endpoint = new SearchByIdEndpoint(userIds, Extras.None, 0, 100, null);
			Action<PaginatedResult<UserSearchResult>, WebError> searchCompletion = null;
			searchCompletion = delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				if (webError == null)
				{
					foreach (KeyValuePair<int, string> keyValuePair in result.Elements.Select((UserSearchResult x) => new KeyValuePair<int, string>(x.UserId, x.Avatar)))
					{
						this.UpdateCacheURLForUserId(keyValuePair.Key, keyValuePair.Value);
					}
					if (result.Next != null)
					{
						result.Next(searchCompletion);
						return;
					}
					handle.endpoint = null;
					if (onCompletion != null)
					{
						onCompletion(true);
						return;
					}
				}
				else if (onCompletion != null)
				{
					onCompletion(false);
				}
			};
			handle.endpoint.Execute(searchCompletion);
			return handle;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x00152A58 File Offset: 0x00150C58
		public AvatarManager.RetrievalHandle LoadPlayerAvatar(int userId, Image image, Action<bool> onCompletion)
		{
			if (userId <= 0)
			{
				throw new ArgumentException("LoadPlayerAvatar called with invalid userId");
			}
			if (image == null)
			{
				throw new ArgumentException("LoadPlayerAvatar called with null image");
			}
			if (this._userIdToAvatarURL.ContainsKey(userId))
			{
				return this.LoadPlayerAvatar(userId, this._userIdToAvatarURL[userId], image, onCompletion, null);
			}
			AvatarManager.RetrievalHandle handle = new AvatarManager.RetrievalHandle();
			this.CacheURLForUserIds(new int[] { userId }, delegate(bool cacheSucceeded)
			{
				if (cacheSucceeded && this._userIdToAvatarURL.ContainsKey(userId))
				{
					this.LoadPlayerAvatar(userId, this._userIdToAvatarURL[userId], image, onCompletion, handle);
					return;
				}
				image.sprite = this.defaultAvatar;
				if (onCompletion != null)
				{
					onCompletion(false);
				}
			}, handle);
			return handle;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x00152B2C File Offset: 0x00150D2C
		public AvatarManager.RetrievalHandle LoadPlayerAvatar(int userId, string url, Image image, Action<bool> onCompletion = null, AvatarManager.RetrievalHandle handle = null)
		{
			if (image == null)
			{
				throw new ArgumentException("LoadPlayerAvatar called with null image");
			}
			for (int i = 0; i < this._cachedAvatars.Count; i++)
			{
				AvatarManager.Avatar avatar = this._cachedAvatars[i];
				if (avatar.userId == userId)
				{
					this._cachedAvatars.RemoveAt(i);
					this._cachedAvatars.Insert(0, avatar);
					image.sprite = avatar.sprite;
					if (onCompletion != null)
					{
						onCompletion(true);
					}
					return null;
				}
			}
			string text = (string.IsNullOrEmpty(url) ? "https://cdn.daysofwonder.com/images/avatars/avatar-neutral.jpg" : url);
			if (handle == null)
			{
				handle = new AvatarManager.RetrievalHandle();
			}
			AsmoLogger.Debug("AvatarManager", "Downloading avatar", new Hashtable { { "userId", userId } });
			handle.httpRequest = new HTTPRequest(new Uri(text), delegate(HTTPRequest req, HTTPResponse response)
			{
				handle.httpRequest = null;
				bool flag = false;
				if (response != null && response.Data != null)
				{
					if (TextureLoader.StartWithJPEGHeader(response.Data) || TextureLoader.StartWithPNGHeader(response.Data))
					{
						Texture2D texture2D = new Texture2D(0, 0);
						texture2D.LoadImage(response.Data);
						Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
						AvatarManager.Avatar avatar2 = default(AvatarManager.Avatar);
						avatar2.userId = userId;
						avatar2.sprite = sprite;
						this._cachedAvatars.Insert(0, avatar2);
						if ((long)this._cachedAvatars.Count > (long)((ulong)this.cacheCapacity))
						{
							this._cachedAvatars.RemoveRange((int)this.cacheCapacity, this._cachedAvatars.Count - (int)this.cacheCapacity);
						}
						image.sprite = sprite;
						flag = true;
					}
					else
					{
						image.sprite = this.defaultAvatar;
					}
				}
				if (onCompletion != null)
				{
					onCompletion(flag);
				}
			});
			handle.httpRequest.Send();
			return handle;
		}

		// Token: 0x04002CE4 RID: 11492
		public Sprite defaultAvatar;

		// Token: 0x04002CE5 RID: 11493
		public uint cacheCapacity = 100U;

		// Token: 0x04002CE6 RID: 11494
		private const string _debugModuleName = "AvatarManager";

		// Token: 0x04002CE7 RID: 11495
		private const string _defaultAvatarLocation = "https://cdn.daysofwonder.com/images/avatars/avatar-neutral.jpg";

		// Token: 0x04002CE8 RID: 11496
		private Dictionary<int, string> _userIdToAvatarURL = new Dictionary<int, string>();

		// Token: 0x04002CE9 RID: 11497
		private List<AvatarManager.Avatar> _cachedAvatars = new List<AvatarManager.Avatar>();

		// Token: 0x0200082B RID: 2091
		public class RetrievalHandle
		{
			// Token: 0x06003B48 RID: 15176 RVA: 0x0004E5FB File Offset: 0x0004C7FB
			public void Abort()
			{
				if (this.endpoint != null)
				{
					this.endpoint.Abort();
					this.endpoint = null;
				}
				if (this.httpRequest != null)
				{
					this.httpRequest.Abort();
					this.httpRequest = null;
				}
			}

			// Token: 0x04002CEA RID: 11498
			public SearchByIdEndpoint endpoint;

			// Token: 0x04002CEB RID: 11499
			public HTTPRequest httpRequest;
		}

		// Token: 0x0200082C RID: 2092
		private struct Avatar
		{
			// Token: 0x04002CEC RID: 11500
			public int userId;

			// Token: 0x04002CED RID: 11501
			public Sprite sprite;
		}
	}
}
