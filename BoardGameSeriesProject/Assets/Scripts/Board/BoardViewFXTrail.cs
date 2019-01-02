using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardViewFXTrail : MonoBehaviour
{
	
	public void TriggerTrailFX(Vector2 start, Vector2 finish, float duration)
	{
		StartCoroutine(TrailFXCoroutine( start, finish, duration ));
	}

	protected virtual IEnumerator TrailFXCoroutine(Vector2 start, Vector2 finish, float duration)
	{
		float startTime = Time.unscaledTime;
		float targetDuration = duration;
		float percentage = 0f;
		while (percentage < 1f)
		{
			percentage = (Time.unscaledTime - startTime) / targetDuration;
			percentage = Mathf.Clamp(percentage, 0f, 1f);
			Vector3 nextPosition = (start + ((finish - start)*percentage));
			nextPosition += new Vector3(0f,0f,1f);
			transform.position = nextPosition;
			yield return new WaitForEndOfFrame();
		}
	}
}
