using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000852 RID: 2130
	public static class Reflection
	{
		// Token: 0x06003C12 RID: 15378 RVA: 0x00154720 File Offset: 0x00152920
		public static Hashtable HashtableFromObject(object obj, HashSet<string> excludedFields = null, uint maxDepth = 30U)
		{
			object obj2 = Reflection.ParseObject(obj, "root", excludedFields, 1U, maxDepth);
			Hashtable hashtable = obj2 as Hashtable;
			if (obj2 != null && hashtable == null)
			{
				hashtable = new Hashtable { { "array", obj2 } };
			}
			return hashtable;
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0004EDE8 File Offset: 0x0004CFE8
		private static object ParseObject(object obj, string varPath, HashSet<string> excludedFields, uint depth, uint maxDepth)
		{
			if (depth > maxDepth)
			{
				return null;
			}
			if (Reflection.CanBeLogged(obj))
			{
				return obj;
			}
			if (Reflection.IsCollection(obj))
			{
				return Reflection.ParseCollection(obj as ICollection, depth, maxDepth, varPath, excludedFields);
			}
			return Reflection.ParseUnknownObject(obj, depth, maxDepth, varPath, excludedFields);
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x0015475C File Offset: 0x0015295C
		private static object ParseCollection(ICollection collec, uint depth, uint maxDepth, string varPath, HashSet<string> excludedFields)
		{
			if (depth > maxDepth)
			{
				return null;
			}
			if (collec == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			int num = 0;
			foreach (object obj in collec)
			{
				object obj2 = Reflection.ParseObject(obj, varPath + "." + num.ToString(), excludedFields, depth + 1U, maxDepth);
				hashtable.Add(num, obj2);
				num++;
			}
			return hashtable;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x001547EC File Offset: 0x001529EC
		private static object ParseUnknownObject(object obj, uint depth, uint maxDepth, string varPath, HashSet<string> excludedFields)
		{
			if (depth > maxDepth)
			{
				return null;
			}
			if (obj == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			Type type = obj.GetType();
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				object value = type.GetField(fieldInfo.Name).GetValue(obj);
				string text = varPath + "." + fieldInfo.Name;
				if (!Reflection.PathMatchesExclusion(text, excludedFields))
				{
					object obj2 = Reflection.ParseObject(value, text, excludedFields, depth + 1U, maxDepth);
					hashtable.Add(fieldInfo.Name, obj2);
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				object value2 = type.GetProperty(propertyInfo.Name).GetValue(obj, null);
				string text2 = varPath + "." + propertyInfo.Name;
				if (!Reflection.PathMatchesExclusion(text2, excludedFields))
				{
					object obj3 = Reflection.ParseObject(value2, text2, excludedFields, depth + 1U, maxDepth);
					hashtable.Add(propertyInfo.Name, obj3);
				}
			}
			return hashtable;
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0004EE1F File Offset: 0x0004D01F
		public static bool CanBeLogged(object obj)
		{
			return obj != null && Reflection.CanLogType(obj.GetType());
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x001548F4 File Offset: 0x00152AF4
		public static bool CanLogType(Type type)
		{
			if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
			{
				return true;
			}
			if (type.IsArray)
			{
				return Reflection.CanLogType(type.GetElementType());
			}
			if (typeof(ICollection).IsAssignableFrom(type))
			{
				Type[] genericArguments = type.GetGenericArguments();
				bool flag = true;
				Type[] array = genericArguments;
				for (int i = 0; i < array.Length; i++)
				{
					if (!Reflection.CanLogType(array[i]))
					{
						flag = false;
					}
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x0004EE31 File Offset: 0x0004D031
		public static bool IsCollection(object obj)
		{
			return obj != null && typeof(ICollection).IsAssignableFrom(obj.GetType());
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x00154974 File Offset: 0x00152B74
		private static bool PathMatchesExclusion(string path, HashSet<string> excludedFields)
		{
			if (excludedFields == null)
			{
				return false;
			}
			foreach (string text in excludedFields)
			{
				if (path.Contains(text))
				{
					return true;
				}
			}
			return false;
		}
	}
}
