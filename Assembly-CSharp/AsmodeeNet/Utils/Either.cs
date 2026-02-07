using System;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000845 RID: 2117
	public class Either<T, U>
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x0004ECF6 File Offset: 0x0004CEF6
		public T Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06003BEE RID: 15342 RVA: 0x0004ECFE File Offset: 0x0004CEFE
		public U Error
		{
			get
			{
				return this._error;
			}
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0004ED06 File Offset: 0x0004CF06
		protected Either(T value, U error)
		{
			this._value = value;
			this._error = error;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x001540E4 File Offset: 0x001522E4
		public static Either<T, U> newWithValue(T value)
		{
			return new Either<T, U>(value, default(U));
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00154100 File Offset: 0x00152300
		public static Either<T, U> newWithError(U error)
		{
			return new Either<T, U>(default(T), error);
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x0004ED1C File Offset: 0x0004CF1C
		public bool HasError
		{
			get
			{
				return this.Error != null;
			}
		}

		// Token: 0x04002D91 RID: 11665
		private T _value;

		// Token: 0x04002D92 RID: 11666
		private U _error;
	}
}
