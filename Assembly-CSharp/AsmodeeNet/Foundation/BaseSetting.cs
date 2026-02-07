using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x0200094F RID: 2383
	[Serializable]
	public abstract class BaseSetting<T> : IBaseSetting
	{
		// Token: 0x06004014 RID: 16404 RVA: 0x00051328 File Offset: 0x0004F528
		protected BaseSetting(string name)
		{
			this.Name = name;
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06004015 RID: 16405 RVA: 0x00051337 File Offset: 0x0004F537
		// (set) Token: 0x06004016 RID: 16406 RVA: 0x0005133F File Offset: 0x0004F53F
		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0015D7F0 File Offset: 0x0015B9F0
		public virtual void Clear()
		{
			T value = this.Value;
			KeyValueStore.DeleteKey(this._FullPath);
			KeyValueStore.Save();
			if (!EqualityComparer<T>.Default.Equals(this.DefaultValue, value) && this.OnValueChanged != null)
			{
				this.OnValueChanged(value, this.DefaultValue);
			}
		}

		// Token: 0x14000159 RID: 345
		// (add) Token: 0x06004018 RID: 16408 RVA: 0x0015D844 File Offset: 0x0015BA44
		// (remove) Token: 0x06004019 RID: 16409 RVA: 0x0015D87C File Offset: 0x0015BA7C
		public event Action<T, T> OnValueChanged;

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x0600401A RID: 16410 RVA: 0x00051348 File Offset: 0x0004F548
		protected string _FullPath
		{
			get
			{
				return "Settings." + this.Name;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x0600401B RID: 16411 RVA: 0x0005135A File Offset: 0x0004F55A
		// (set) Token: 0x0600401C RID: 16412 RVA: 0x0015D8B4 File Offset: 0x0015BAB4
		public T Value
		{
			get
			{
				if (KeyValueStore.HasKey(this._FullPath))
				{
					return this._ReadValue();
				}
				return this.DefaultValue;
			}
			set
			{
				T value2 = this.Value;
				if (!EqualityComparer<T>.Default.Equals(value, value2))
				{
					this._WriteValue(value);
					KeyValueStore.Save();
					if (this.OnValueChanged != null)
					{
						this.OnValueChanged(value2, value);
					}
				}
			}
		}

		// Token: 0x0600401D RID: 16413
		protected abstract T _ReadValue();

		// Token: 0x0600401E RID: 16414
		protected abstract void _WriteValue(T value);

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600401F RID: 16415 RVA: 0x00051376 File Offset: 0x0004F576
		public T DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x0005137E File Offset: 0x0004F57E
		public override string ToString()
		{
			return string.Format("{0}={1} (default:{2})", this.Name, this.Value, this.DefaultValue);
		}

		// Token: 0x040030EC RID: 12524
		[HideInInspector]
		[SerializeField]
		private string _name;

		// Token: 0x040030EE RID: 12526
		[SerializeField]
		private T _defaultValue;
	}
}
