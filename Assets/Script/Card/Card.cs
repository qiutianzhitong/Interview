using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    private int iconIndex;

    public void SetIcon(int index)
    {
        iconIndex = index;
        // 这里可以根据iconIndex设置不同的图标
        frontImage.sprite = Resources.Load<Sprite>("Icon" + index);
    }

    public int GetIcon()
    {
        return iconIndex;
    }

    public void Flip()
    {
        StartCoroutine(FlipAnimation(true));
    }

    public void FlipBack()
    {
        StartCoroutine(FlipAnimation(false));
    }

    private IEnumerator FlipAnimation(bool isFlippingForward)
    {
        float flipDuration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < flipDuration)
        {
            float scaleX = Mathf.Lerp(1f, 0f, elapsedTime / flipDuration);
            transform.localScale = new Vector3(scaleX, 1f, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        backImage.enabled = !isFlippingForward;
        frontImage.enabled = isFlippingForward;

        elapsedTime = 0f;
        while (elapsedTime < flipDuration)
        {
            float scaleX = Mathf.Lerp(0f, 1f, elapsedTime / flipDuration);
            transform.localScale = new Vector3(scaleX, 1f, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}