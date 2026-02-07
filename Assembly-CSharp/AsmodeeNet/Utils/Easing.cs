using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000833 RID: 2099
	public static class Easing
	{
		// Token: 0x06003B5B RID: 15195 RVA: 0x0004E650 File Offset: 0x0004C850
		public static IEnumerator EaseFromTo(float from, float to, float duration, Easer easer, Action<float> easeMethod, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			float range = to - from;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				float num = from + range * easer(elapsed / duration);
				easeMethod(num);
				yield return 0;
			}
			easeMethod(to);
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0004E684 File Offset: 0x0004C884
		public static IEnumerator EaseFromTo(Vector2 from, Vector2 to, float duration, Easer easer, Action<Vector2> easeMethod, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector2 range = to - from;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				Vector2 vector = from + range * easer(elapsed / duration);
				easeMethod(vector);
				yield return 0;
			}
			easeMethod(to);
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0004E6B8 File Offset: 0x0004C8B8
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer easer, Action actionAfterEasing)
		{
			float elapsed = 0f;
			Vector3 start = transform.localPosition;
			Vector3 range = target - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				if (transform != null)
				{
					transform.localPosition = start + range * easer(elapsed / duration);
				}
				yield return 0;
			}
			if (transform != null)
			{
				transform.localPosition = target;
			}
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0004E6E4 File Offset: 0x0004C8E4
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, Easer ease, Action<object[]> actionAfterEasing = null, params object[] parameters)
		{
			float elapsed = 0f;
			Vector3 start = transform.localPosition;
			Vector3 range = target - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				if (transform != null)
				{
					transform.localPosition = start + range * ease(elapsed / duration);
				}
				yield return 0;
			}
			if (transform != null)
			{
				transform.localPosition = target;
			}
			if (actionAfterEasing != null)
			{
				actionAfterEasing(parameters);
			}
			yield break;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0004E718 File Offset: 0x0004C918
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration)
		{
			return transform.MoveTo(target, duration, Ease.Linear, null, Array.Empty<object>());
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0004E72D File Offset: 0x0004C92D
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action actionAfterEasing)
		{
			return transform.MoveTo(target, duration, Ease.FromType(ease), actionAfterEasing);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x0004E73F File Offset: 0x0004C93F
		public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action<object[]> actionAfterEasing = null, params object[] parameters)
		{
			return transform.MoveTo(target, duration, Ease.FromType(ease), actionAfterEasing, parameters);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x00152F28 File Offset: 0x00151128
		public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, Easer ease)
		{
			Vector3 localPosition = transform.localPosition;
			transform.localPosition = target;
			return transform.MoveTo(localPosition, duration, ease, null, Array.Empty<object>());
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0004E753 File Offset: 0x0004C953
		public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration)
		{
			return transform.MoveFrom(target, duration, Ease.Linear);
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0004E762 File Offset: 0x0004C962
		public static IEnumerator MoveFrom(this Transform transform, Vector3 target, float duration, EaseType ease)
		{
			return transform.MoveFrom(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0004E772 File Offset: 0x0004C972
		public static IEnumerator ScaleLayoutTo(this LayoutElement layoutElement, float minWidth, float minHeight, float duration, Easer ease, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector2 start = new Vector2(layoutElement.minWidth, layoutElement.minHeight);
			Vector2 range = new Vector2(minWidth, minHeight) - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				layoutElement.minWidth = start.x + range.x * ease(elapsed / duration);
				layoutElement.minHeight = start.y + range.y * ease(elapsed / duration);
				yield return 0;
			}
			layoutElement.minWidth = minWidth;
			layoutElement.minHeight = minHeight;
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x0004E7A6 File Offset: 0x0004C9A6
		public static IEnumerator ScaleRectransformTo(this RectTransform rectTransform, float width, float height, float duration, Easer ease, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector2 start = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
			Vector2 range = new Vector2(width, height) - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, start.x + range.x * ease(elapsed / duration));
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, start.y + range.y * ease(elapsed / duration));
				yield return 0;
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0004E7DA File Offset: 0x0004C9DA
		public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, Easer ease, Action actionAfterEasing = null)
		{
			float elapsed = 0f;
			Vector3 start = transform.localScale;
			Vector3 range = target - start;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				transform.localScale = start + range * ease(elapsed / duration);
				yield return 0;
			}
			transform.localScale = target;
			if (actionAfterEasing != null)
			{
				actionAfterEasing();
			}
			yield break;
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0004E806 File Offset: 0x0004CA06
		public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration)
		{
			return transform.ScaleTo(target, duration, Ease.Linear, null);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0004E816 File Offset: 0x0004CA16
		public static IEnumerator ScaleTo(this Transform transform, Vector3 target, float duration, EaseType ease, Action actionAfterEasing = null)
		{
			return transform.ScaleTo(target, duration, Ease.FromType(ease), actionAfterEasing);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x00152F54 File Offset: 0x00151154
		public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, Easer ease)
		{
			Vector3 localScale = transform.localScale;
			transform.localScale = target;
			return transform.ScaleTo(localScale, duration, ease, null);
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0004E828 File Offset: 0x0004CA28
		public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration)
		{
			return transform.ScaleFrom(target, duration, Ease.Linear);
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0004E837 File Offset: 0x0004CA37
		public static IEnumerator ScaleFrom(this Transform transform, Vector3 target, float duration, EaseType ease)
		{
			return transform.ScaleFrom(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x0004E847 File Offset: 0x0004CA47
		public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, Easer ease)
		{
			float elapsed = 0f;
			Quaternion start = transform.localRotation;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				transform.localRotation = Quaternion.Lerp(start, target, ease(elapsed / duration));
				yield return 0;
			}
			transform.localRotation = target;
			yield break;
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x0004E86B File Offset: 0x0004CA6B
		public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration)
		{
			return transform.RotateTo(target, duration, Ease.Linear);
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0004E87A File Offset: 0x0004CA7A
		public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration, EaseType ease)
		{
			return transform.RotateTo(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00152F7C File Offset: 0x0015117C
		public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, Easer ease)
		{
			Quaternion localRotation = transform.localRotation;
			transform.localRotation = target;
			return transform.RotateTo(localRotation, duration, ease);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0004E88A File Offset: 0x0004CA8A
		public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration)
		{
			return transform.RotateFrom(target, duration, Ease.Linear);
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0004E899 File Offset: 0x0004CA99
		public static IEnumerator RotateFrom(this Transform transform, Quaternion target, float duration, EaseType ease)
		{
			return transform.RotateFrom(target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0004E8A9 File Offset: 0x0004CAA9
		public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, Easer ease)
		{
			float elapsed = 0f;
			Vector3 start = transform.localPosition;
			while (elapsed < duration)
			{
				elapsed = Mathf.MoveTowards(elapsed, duration, Time.deltaTime);
				float num = ease(elapsed / duration);
				Vector3 vector;
				vector.x = start.x * (1f - num) * (1f - num) + control.x * 2f * (1f - num) * num + target.x * num * num;
				vector.y = start.y * (1f - num) * (1f - num) + control.y * 2f * (1f - num) * num + target.y * num * num;
				vector.z = start.z * (1f - num) * (1f - num) + control.z * 2f * (1f - num) * num + target.z * num * num;
				transform.localPosition = vector;
				yield return 0;
			}
			transform.localPosition = target;
			yield break;
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0004E8D5 File Offset: 0x0004CAD5
		public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration)
		{
			return transform.CurveTo(control, target, duration, Ease.Linear);
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0004E8E5 File Offset: 0x0004CAE5
		public static IEnumerator CurveTo(this Transform transform, Vector3 control, Vector3 target, float duration, EaseType ease)
		{
			return transform.CurveTo(control, target, duration, Ease.FromType(ease));
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x00152FA0 File Offset: 0x001511A0
		public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, Easer ease)
		{
			Vector3 localPosition = transform.localPosition;
			transform.localPosition = start;
			return transform.CurveTo(control, localPosition, duration, ease);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0004E8F7 File Offset: 0x0004CAF7
		public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration)
		{
			return transform.CurveFrom(control, start, duration, Ease.Linear);
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0004E907 File Offset: 0x0004CB07
		public static IEnumerator CurveFrom(this Transform transform, Vector3 control, Vector3 start, float duration, EaseType ease)
		{
			return transform.CurveFrom(control, start, duration, Ease.FromType(ease));
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x0004E919 File Offset: 0x0004CB19
		public static IEnumerator Shake(this Transform transform, Vector3 amount, float duration)
		{
			Vector3 start = transform.localPosition;
			Vector3 shake = Vector3.zero;
			while (duration > 0f)
			{
				duration -= Time.deltaTime;
				shake.Set(global::UnityEngine.Random.Range(-amount.x, amount.x), global::UnityEngine.Random.Range(-amount.y, amount.y), global::UnityEngine.Random.Range(-amount.z, amount.z));
				transform.localPosition = start + shake;
				yield return 0;
			}
			transform.localPosition = start;
			yield break;
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0004E936 File Offset: 0x0004CB36
		public static IEnumerator Shake(this Transform transform, float amount, float duration)
		{
			return transform.Shake(new Vector3(amount, amount, amount), duration);
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0004E947 File Offset: 0x0004CB47
		public static IEnumerator Wait(float duration)
		{
			while (duration > 0f)
			{
				duration -= Time.deltaTime;
				yield return 0;
			}
			yield break;
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x0004E956 File Offset: 0x0004CB56
		public static IEnumerator WaitUntil(Predicate predicate)
		{
			while (!predicate())
			{
				yield return 0;
			}
			yield break;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x00152FC8 File Offset: 0x001511C8
		public static float Loop(float duration, float from, float to, float offsetPercent)
		{
			float num = to - from;
			float num2 = (Time.time + duration * offsetPercent) * (Mathf.Abs(num) / duration);
			if (num > 0f)
			{
				return from + Time.time - num * (float)Mathf.FloorToInt(Time.time / num);
			}
			return from - (Time.time - Mathf.Abs(num) * (float)Mathf.FloorToInt(num2 / Mathf.Abs(num)));
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0004E965 File Offset: 0x0004CB65
		public static float Loop(float duration, float from, float to)
		{
			return Easing.Loop(duration, from, to, 0f);
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0004E974 File Offset: 0x0004CB74
		public static Vector3 Loop(float duration, Vector3 from, Vector3 to, float offsetPercent)
		{
			return Vector3.Lerp(from, to, Easing.Loop(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0004E98E File Offset: 0x0004CB8E
		public static Vector3 Loop(float duration, Vector3 from, Vector3 to)
		{
			return Vector3.Lerp(from, to, Easing.Loop(duration, 0f, 1f));
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0004E9A7 File Offset: 0x0004CBA7
		public static Quaternion Loop(float duration, Quaternion from, Quaternion to, float offsetPercent)
		{
			return Quaternion.Lerp(from, to, Easing.Loop(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0004E9C1 File Offset: 0x0004CBC1
		public static Quaternion Loop(float duration, Quaternion from, Quaternion to)
		{
			return Quaternion.Lerp(from, to, Easing.Loop(duration, 0f, 1f));
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0015302C File Offset: 0x0015122C
		public static float Wave(float duration, float from, float to, float offsetPercent)
		{
			float num = (to - from) / 2f;
			return from + num + Mathf.Sin((Time.time + duration * offsetPercent) / duration * 6.2831855f) * num;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0004E9DA File Offset: 0x0004CBDA
		public static float Wave(float duration, float from, float to)
		{
			return Easing.Wave(duration, from, to, 0f);
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0004E9E9 File Offset: 0x0004CBE9
		public static Vector3 Wave(float duration, Vector3 from, Vector3 to, float offsetPercent)
		{
			return Vector3.Lerp(from, to, Easing.Wave(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0004EA03 File Offset: 0x0004CC03
		public static Vector3 Wave(float duration, Vector3 from, Vector3 to)
		{
			return Vector3.Lerp(from, to, Easing.Wave(duration, 0f, 1f));
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0004EA1C File Offset: 0x0004CC1C
		public static Quaternion Wave(float duration, Quaternion from, Quaternion to, float offsetPercent)
		{
			return Quaternion.Lerp(from, to, Easing.Wave(duration, 0f, 1f, offsetPercent));
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0004EA36 File Offset: 0x0004CC36
		public static Quaternion Wave(float duration, Quaternion from, Quaternion to)
		{
			return Quaternion.Lerp(from, to, Easing.Wave(duration, 0f, 1f));
		}
	}
}
