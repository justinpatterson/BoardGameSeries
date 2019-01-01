using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardElement_ConnectFour : BoardElement {
    protected override IEnumerator RevealPlayerNumberSpriteCoroutine()
    {

        playerSpriteSlot.transform.localScale = Vector3.zero;
        playerSpriteSlot.transform.localScale = Vector3.zero;
        float fallHeight = GameManager.instance.boardModel.height * GameManager.instance.boardViewer.spriteSize;

        float startTime = Time.unscaledTime;
        float targetDuration = 0.2f;
        float percentage = 0f;
        while (percentage < 1f)
        {
            percentage = (Time.unscaledTime - startTime) / targetDuration;
            percentage = Mathf.Clamp(percentage, 0f, 1f);
            playerSpriteSlot.transform.localScale = Vector3.one * percentage;
            playerSpriteSlot.transform.localPosition = Vector3.up * (1f - percentage) * fallHeight;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
}
