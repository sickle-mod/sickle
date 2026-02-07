using System;
using System.Reflection;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000843 RID: 2115
	public abstract class Builder<T> where T : class
	{
		// Token: 0x06003BE8 RID: 15336
		public abstract Builder<T>.BuilderErrors[] Validate();

		// Token: 0x06003BE9 RID: 15337 RVA: 0x00154094 File Offset: 0x00152294
		public Either<T, Builder<T>.BuilderErrors[]> Build(bool mustValidate = true)
		{
			if (mustValidate)
			{
				Builder<T>.BuilderErrors[] array = this.Validate();
				if (array != null)
				{
					return Either<T, Builder<T>.BuilderErrors[]>.newWithError(array);
				}
			}
			return Either<T, Builder<T>.BuilderErrors[]>.newWithValue(Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { this }, null) as T);
		}

		// Token: 0x02000844 RID: 2116
		public class BuilderErrors
		{
			// Token: 0x06003BEB RID: 15339 RVA: 0x0004ECC8 File Offset: 0x0004CEC8
			public BuilderErrors(string badField, string reason)
			{
				this.badField = badField;
				this.reason = reason;
			}

			// Token: 0x06003BEC RID: 15340 RVA: 0x0004ECDE File Offset: 0x0004CEDE
			public override string ToString()
			{
				return string.Format("'{0}' field is badly formatted. The reason is '{1}'", this.badField, this.reason);
			}

			// Token: 0x04002D8F RID: 11663
			public string badField;

			// Token: 0x04002D90 RID: 11664
			public string reason;
		}
	}
}
