using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSILightEffectScript : MonoBehaviour
{
    public float initialScaleSpeed;
    public float finalAlpha;
    public float maxScale;
    public float minScale;
    public float thisAlphaScaleSpeed = 10f;
    public SpriteRenderer otherRender;
    public SpriteRenderer thisRenderer;
    public float otherAlphaDropSpeed = 10f;

    public void animateInitialEffect()
    {
        StartCoroutine(Animate());
    }
    private IEnumerator Animate()
    {
        bool animating = true;

        while(animating)
        {
            float changedAlpha = Mathf.Clamp(otherRender.color.a - otherAlphaDropSpeed, 0, 1);
            otherRender.color = new Color(otherRender.color.r, otherRender.color.g, otherRender.color.b, changedAlpha);

            if(changedAlpha <= 150/255)
            {
                increaseAnim(initialScaleSpeed);
                
            }
            if(changedAlpha <= 10/255)
            {
                break;
            }

            yield return null;
        }
        otherRender.gameObject.SetActive(false);

        float scaleSpeed = initialScaleSpeed;

        while(!increaseAnim(scaleSpeed))
        {
            scaleSpeed += initialScaleSpeed/20;
            yield return null;
        }
    }

    private bool increaseAnim(float scaleSpeed)
    {
        if(thisRenderer.color.a >= finalAlpha)
        {
            float changedAlpha = Mathf.Clamp(thisRenderer.color.a - thisAlphaScaleSpeed, finalAlpha, 1);
            thisRenderer.color = new Color(thisRenderer.color.r, thisRenderer.color.g, thisRenderer.color.b, changedAlpha);
        }

        float changedScale = Mathf.Clamp(transform.localScale.x + scaleSpeed, 0, maxScale);
        transform.localScale = new Vector3(changedScale, changedScale, 1);

        if(changedScale == maxScale)
        {
            return true;
        }

        return false;
        
    }
}
