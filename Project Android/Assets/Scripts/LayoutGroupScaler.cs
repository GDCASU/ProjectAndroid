using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LayoutGroupScaler : MonoBehaviour {

    private Vector2 screenDimensions;

	void Start () {
        screenDimensions = new Vector2(Screen.width, Screen.height);
	}
	
	void Update () {
		if(screenDimensions.x != Screen.width || screenDimensions.y != Screen.height)
        {
            float xScale = Screen.width / screenDimensions.x;
            float yScale = Screen.height / screenDimensions.y;
            HorizontalOrVerticalLayoutGroup layout = GetComponent<HorizontalOrVerticalLayoutGroup>();
            RectOffset offset = layout.padding;
            offset.bottom = Mathf.RoundToInt(offset.bottom * yScale);
            offset.top = Mathf.RoundToInt(offset.top * yScale);
            offset.left = Mathf.RoundToInt(offset.left * xScale);
            offset.right = Mathf.RoundToInt(offset.right * xScale);
            if (layout is HorizontalLayoutGroup) layout.spacing *= xScale;
            else layout.spacing *= yScale;

            screenDimensions = new Vector2(Screen.width, Screen.height);
        }
	}
}
