using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
	private void Awake()
	{
		RectTransform rect = GetComponent<RectTransform>();
		var safeArea = Screen.safeArea;
		var anchorMin = safeArea.position;
		var anchorMax = anchorMin + safeArea.size;

		anchorMin.x /= Screen.width;
		anchorMin.y /= Screen.height;
		anchorMax.x /= Screen.width;
		anchorMax.y /= Screen.height;
		
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
	}
}
