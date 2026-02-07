using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Assets.Scripts.Multiplayer.Messages.Lobby;
using BestHTTP;
using Newtonsoft.Json;
using Scythe.Multiplayer.Data;
using Scythe.Multiplayer.Messages;
using Scythe.Utilities;

namespace Scythe.Multiplayer
{
	// Token: 0x02000290 RID: 656
	public class RequestController
	{
		// Token: 0x060014BF RID: 5311 RVA: 0x0009BB54 File Offset: 0x00099D54
		public static void Reset()
		{
			RequestController.appPaused = false;
			RequestController.gettingUpdate = false;
			RequestController.messageCounter = 0;
			RequestController.sendingCounter = 0;
			RequestController.gettingUpdateCounter = 0;
			RequestController.updateErrors = 0;
			RequestController.serverUpdates.Clear();
			RequestController.messagesToSend.Clear();
			RequestController.currentMessage = default(KeyValuePair<string, Scythe.Multiplayer.Data.Action>);
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0003608A File Offset: 0x0003428A
		public static void OnAppPausedStateChanged(bool pauseState)
		{
			if (PlatformManager.IsMobile)
			{
				RequestController.appPaused = pauseState;
				return;
			}
			RequestController.appPaused = false;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x000360A0 File Offset: 0x000342A0
		public static void SetCounter(int counter)
		{
			RequestController.messageCounter = counter;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0009BBA4 File Offset: 0x00099DA4
		public static void AddAction(Scythe.Multiplayer.Messages.Message message)
		{
			if (MultiplayerController.Instance.SpectatorMode)
			{
				return;
			}
			message.SetCounter(RequestController.messageCounter++);
			DebugLog.Log("Added message " + GameSerializer.JsonMessageSerializer<Scythe.Multiplayer.Messages.Message>(message).Replace("Assembly-CSharp", "ScytheWebRole"));
			RequestController.messagesToSend.Enqueue(new KeyValuePair<string, Scythe.Multiplayer.Data.Action>("Scythe/Action", new Scythe.Multiplayer.Data.Action(GameSerializer.JsonMessageSerializer<Scythe.Multiplayer.Messages.Message>(message).Replace("Assembly-CSharp", "ScytheWebRole"))));
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0009BC24 File Offset: 0x00099E24
		public static void SendMessage()
		{
			if (RequestController.appPaused)
			{
				return;
			}
			if (RequestController.sendingAction)
			{
				RequestController.sendingCounter++;
				if (RequestController.sendingCounter < 8)
				{
					return;
				}
				RequestController.sendingAction = false;
			}
			if (RequestController.messagesToSend.Count == 0 && RequestController.currentMessage.Key == null)
			{
				return;
			}
			if (RequestController.currentMessage.Key == null)
			{
				RequestController.currentMessage = RequestController.messagesToSend.Dequeue();
			}
			while (RequestController.messagesToSend.Count > 0)
			{
				KeyValuePair<string, Scythe.Multiplayer.Data.Action> keyValuePair = RequestController.messagesToSend.Peek();
				if (!(keyValuePair.Key == RequestController.currentMessage.Key))
				{
					break;
				}
				RequestController.currentMessage.Value.Actions.AddRange(keyValuePair.Value.Actions);
				RequestController.messagesToSend.Dequeue();
			}
			RequestController.sendingAction = true;
			RequestController.sendingCounter = 0;
			RequestController.SendMessage<Scythe.Multiplayer.Data.Action>(RequestController.currentMessage.Key, RequestController.currentMessage.Value, new Action<string>(RequestController.ProcessSendActionResponse), new Action<Exception>(RequestController.ShowSendActionError));
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0009BD2C File Offset: 0x00099F2C
		public static void RemoveMessageFromQueue(int counter)
		{
			if (string.IsNullOrEmpty(RequestController.currentMessage.Key) || RequestController.currentMessage.Value == null)
			{
				return;
			}
			foreach (string text in RequestController.currentMessage.Value.Actions)
			{
				Scythe.Multiplayer.Messages.Message message = GameSerializer.JsonMessageDeserializer<Scythe.Multiplayer.Messages.Message>(text.Replace("ScytheWebRole", "Assembly-CSharp"));
				if (message.GetCounter() == counter)
				{
					RequestController.currentMessage.Value.Actions.Remove(text);
					break;
				}
				if (message.GetCounter() > counter)
				{
					break;
				}
			}
			if (RequestController.currentMessage.Value.Actions.Count == 0)
			{
				RequestController.currentMessage = default(KeyValuePair<string, Scythe.Multiplayer.Data.Action>);
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x000360A8 File Offset: 0x000342A8
		public static void SaveErronLogOnTheServer(ErrorLogData data)
		{
			if (MultiplayerController.Instance.IsMultiplayer)
			{
				RequestController.SendMessage("Scythe/SendErrorLog", GameSerializer.Serialize<ErrorLogData>(data));
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x000360C6 File Offset: 0x000342C6
		public static string GenerateStringFromMessage<T>(T message)
		{
			return GameSerializer.Serialize<T>(message);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0009BE04 File Offset: 0x0009A004
		public static void GetUpdate()
		{
			if (RequestController.appPaused)
			{
				return;
			}
			if (RequestController.gettingUpdate)
			{
				RequestController.gettingUpdateCounter++;
				if (RequestController.gettingUpdateCounter < 5)
				{
					return;
				}
				RequestController.gettingUpdate = false;
			}
			RequestController.gettingUpdate = true;
			RequestController.gettingUpdateCounter = 0;
			RequestController.getUpdateReady = false;
			RequestController.RequestGetCall(string.Format("{0}Update?playerId={1}&lastExecutedMessage={2}", "Scythe/", PlayerInfo.me.PlayerStats.Id, MessageExecutor.LastMessageCounter()), new Action<string>(RequestController.ProcessUpdateResponse), new Action<Exception>(RequestController.ProcessUpdateError));
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000360CE File Offset: 0x000342CE
		public static void SendMessage<T>(string endpoint, T message, Action<string> onSuccess, Action<Exception> onError)
		{
			RequestController.RequestPostCall(endpoint, GameSerializer.Serialize<T>(message), false, onSuccess, onError);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x000360E0 File Offset: 0x000342E0
		public static void SendMessage<T>(string endpoint, T message)
		{
			if (RequestController.appPaused)
			{
				return;
			}
			RequestController.RequestPostCall(endpoint, GameSerializer.Serialize<T>(message), false, new Action<string>(RequestController.ProcessResponse), new Action<Exception>(RequestController.ShowErrorPanel));
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00036110 File Offset: 0x00034310
		public static void SendMessage(string endpoint, string message)
		{
			if (RequestController.appPaused)
			{
				return;
			}
			RequestController.RequestPostCall(endpoint, message, false, new Action<string>(RequestController.ProcessResponse), new Action<Exception>(RequestController.ShowErrorPanel));
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0009BE9C File Offset: 0x0009A09C
		public static HTTPRequest RequestGetCall(string endpoint, Action<string> onSuccess, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(RequestController.GetApiUri(endpoint), HTTPMethods.Get, delegate(HTTPRequest req, HTTPResponse resp)
			{
				RequestController.HandleResponse(onSuccess, onError, req, resp);
			});
			RequestController.AddAuthorizationToken(httprequest);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0009BEE4 File Offset: 0x0009A0E4
		public static HTTPRequest RequestGetCallForAzureFunction(string endpoint, Action<string> onSuccess, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(RequestController.GetApiUriForAzureFunctions(endpoint), HTTPMethods.Get, delegate(HTTPRequest req, HTTPResponse resp)
			{
				RequestController.HandleResponse(onSuccess, onError, req, resp);
			});
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0009BF24 File Offset: 0x0009A124
		private static void HandleResponse(Action<string> onSuccess, Action<Exception> onError, HTTPRequest req, HTTPResponse resp)
		{
			if (req.Exception != null && onError != null)
			{
				onError(req.Exception);
				return;
			}
			if (resp == null)
			{
				if (onError != null)
				{
					onError(new Exception(string.Empty));
				}
				return;
			}
			if (resp.StatusCode != 200)
			{
				Exception ex;
				try
				{
					ErrorDetails errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(resp.DataAsText);
					if (resp.StatusCode == 401)
					{
						ex = new UnauthorizedAccessException(errorDetails.Message);
					}
					else
					{
						ex = new Exception(errorDetails.Message);
					}
				}
				catch
				{
					ex = new Exception(resp.DataAsText);
				}
				DebugLog.LogError("Server error: " + ex.Message);
				if (onError != null)
				{
					onError(ex);
					return;
				}
			}
			else
			{
				onSuccess(resp.DataAsText);
			}
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0009BFF0 File Offset: 0x0009A1F0
		public static HTTPRequest RequestDeleteCall(string endpoint, Action<string> onSuccess, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(RequestController.GetApiUri(endpoint), HTTPMethods.Delete, delegate(HTTPRequest req, HTTPResponse resp)
			{
				RequestController.HandleResponse(onSuccess, onError, req, resp);
			});
			RequestController.AddAuthorizationToken(httprequest);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0009C038 File Offset: 0x0009A238
		public static HTTPRequest RequestPostCall(string endpoint, string message, bool asJson, Action<string> onSuccess, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(RequestController.GetApiUri(endpoint), HTTPMethods.Post, delegate(HTTPRequest req, HTTPResponse resp)
			{
				RequestController.HandleResponse(onSuccess, onError, req, resp);
			});
			RequestController.AddAuthorizationToken(httprequest);
			if (asJson)
			{
				httprequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
			}
			else
			{
				httprequest.AddHeader("Content-Type", "application/xml; charset=UTF-8");
			}
			httprequest.RawData = Encoding.UTF8.GetBytes(message);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0009C0B8 File Offset: 0x0009A2B8
		public static HTTPRequest RequestPutCall(string endpoint, string message, bool asJson, Action<string> onSuccess, Action<Exception> onError = null)
		{
			HTTPRequest httprequest = new HTTPRequest(RequestController.GetApiUri(endpoint), HTTPMethods.Put, delegate(HTTPRequest req, HTTPResponse resp)
			{
				RequestController.HandleResponse(onSuccess, onError, req, resp);
			});
			RequestController.AddAuthorizationToken(httprequest);
			if (asJson)
			{
				httprequest.AddHeader("Content-Type", "application/json; charset=UTF-8");
			}
			else
			{
				httprequest.AddHeader("Content-Type", "application/xml; charset=UTF-8");
			}
			httprequest.RawData = Encoding.UTF8.GetBytes(message);
			httprequest.Send();
			return httprequest;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0003613B File Offset: 0x0003433B
		private static void AddAuthorizationToken(HTTPRequest request)
		{
			if (!string.IsNullOrEmpty(PlayerInfo.me.Token))
			{
				request.AddHeader("Authorization", "Bearer " + PlayerInfo.me.Token);
			}
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0009C138 File Offset: 0x0009A338
		private static void ProcessResponse(string response)
		{
			if (ConnectionProblem.ErrorShown)
			{
				ConnectionProblem.HideErrorPanel();
			}
			if (string.IsNullOrEmpty(response))
			{
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(response);
			foreach (object obj in xmlDocument.FirstChild.ChildNodes)
			{
				string innerText = ((XmlNode)obj).FirstChild.InnerText;
				DebugLog.Log("Get response " + innerText);
				Scythe.Multiplayer.Messages.Message message = GameSerializer.JsonMessageDeserializer<Scythe.Multiplayer.Messages.Message>(innerText.Replace("ScytheWebRole", "Assembly-CSharp"));
				if (MessageExecutor.IsMessageValid(message))
				{
					MessageExecutor.IncreaseCounter();
					if (message is ExecutedMessage || message is BadMessage)
					{
						(message as IExecutableMessage).Execute(null);
					}
					else
					{
						RequestController.serverUpdates.Enqueue(message);
					}
				}
			}
			RequestController.getUpdateReady = true;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0003616D File Offset: 0x0003436D
		private static void ProcessSendActionResponse(string response)
		{
			RequestController.ProcessResponse(response);
			RequestController.sendingAction = false;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0003617B File Offset: 0x0003437B
		private static void ProcessUpdateResponse(string response)
		{
			RequestController.gettingUpdate = false;
			RequestController.updateErrors = 0;
			RequestController.ProcessResponse(response);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0009C21C File Offset: 0x0009A41C
		private static void ProcessUpdateError(Exception e)
		{
			RequestController.gettingUpdate = false;
			RequestController.updateErrors++;
			if (RequestController.updateErrors > 2)
			{
				RequestController.updateErrors = 0;
				if (RequestController.InLobby())
				{
					MultiplayerController.Instance.Disconnected = true;
					RequestController.serverUpdates.Enqueue(new GetUpdateExceptionMessage(e));
					return;
				}
				if (!ReconnectManager.IsActive)
				{
					if (RequestController.IsPlayerTimedOut(e))
					{
						MultiplayerController.Instance.TryToReconnect();
						return;
					}
					if (RequestController.IsAuthorizationError(e))
					{
						ConnectionProblem.ShowDisconnectedPanel();
						return;
					}
					RequestController.ShowErrorPanel(e);
				}
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0003618F File Offset: 0x0003438F
		private static bool IsAuthorizationError(Exception exception)
		{
			return exception is UnauthorizedAccessException;
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0003619A File Offset: 0x0003439A
		private static bool IsPlayerTimedOut(Exception exception)
		{
			return exception is UnauthorizedAccessException && JsonConvert.DeserializeObject<AuthorizationError>(exception.Message).ErrorStatus == AuthorizationErrorStatus.PlayerNotLoggedIn;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000361B9 File Offset: 0x000343B9
		private static bool InLobby()
		{
			return MultiplayerController.Instance.InLobby;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0009C29C File Offset: 0x0009A49C
		private static void ShowErrorPanel(Exception e)
		{
			if (MultiplayerController.Instance.Disconnected || MultiplayerController.Instance.RunOutOfTime)
			{
				return;
			}
			if (ReconnectManager.IsActive)
			{
				return;
			}
			if (e is TimeoutException)
			{
				return;
			}
			if (!ConnectionProblem.ErrorShown && PlayerInfo.me.MapLoaded)
			{
				ConnectionProblem.ShowConnectionProblemPanel();
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000361C5 File Offset: 0x000343C5
		private static void ShowSendActionError(Exception e)
		{
			RequestController.sendingAction = false;
			RequestController.ShowErrorPanel(e);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x000361D3 File Offset: 0x000343D3
		private static void ShowUpdateError(Exception e)
		{
			RequestController.gettingUpdate = false;
			RequestController.getUpdateReady = true;
			RequestController.ShowErrorPanel(e);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000361E7 File Offset: 0x000343E7
		private static Uri GetApiUri(string endpoint)
		{
			return new Uri(string.Format("{0}{1}", ServerEndpoints.ServerEndpoint, endpoint));
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000361FE File Offset: 0x000343FE
		private static Uri GetApiUriForAzureFunctions(string endpoint)
		{
			return new Uri(string.Format("{0}{1}", ServerEndpoints.AzureFunctionsEndpoint, endpoint));
		}

		// Token: 0x04000F20 RID: 3872
		public static Queue<Scythe.Multiplayer.Messages.Message> serverUpdates = new Queue<Scythe.Multiplayer.Messages.Message>();

		// Token: 0x04000F21 RID: 3873
		private static Queue<KeyValuePair<string, Scythe.Multiplayer.Data.Action>> messagesToSend = new Queue<KeyValuePair<string, Scythe.Multiplayer.Data.Action>>();

		// Token: 0x04000F22 RID: 3874
		private static KeyValuePair<string, Scythe.Multiplayer.Data.Action> currentMessage = default(KeyValuePair<string, Scythe.Multiplayer.Data.Action>);

		// Token: 0x04000F23 RID: 3875
		private static int messageCounter = 0;

		// Token: 0x04000F24 RID: 3876
		private static int sendingCounter = 0;

		// Token: 0x04000F25 RID: 3877
		private static int gettingUpdateCounter = 0;

		// Token: 0x04000F26 RID: 3878
		private static int updateErrors = 0;

		// Token: 0x04000F27 RID: 3879
		private static bool gettingUpdate = false;

		// Token: 0x04000F28 RID: 3880
		private static bool sendingAction = false;

		// Token: 0x04000F29 RID: 3881
		private static bool appPaused = false;

		// Token: 0x04000F2A RID: 3882
		public static bool getUpdateReady = false;
	}
}
