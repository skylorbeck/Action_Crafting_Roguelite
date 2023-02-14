using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenHandler : MonoBehaviour
{
    public Image titleImage;
    public Image gradientImage;
    public SpriteRenderer[] spritesToFade;
    public float fadeDuration = 1f;
    public float fadeDelay = 1f;
    public float maxDistance = 1f;
    public Vector3 startPosition;
    public IEnumerator Start()
    {
        titleImage.gameObject.SetActive(true);
        gradientImage.gameObject.SetActive(true);
        titleImage.transform.localPosition = Vector3.zero;
        gradientImage.transform.localScale = Vector3.one*0.5f;
        yield return new WaitUntil(() => SaveManager.instance != null);
        yield return new WaitUntil(() => SaveManager.instance.loaded);
        Player.instance.SetMetaStats(SaveManager.instance.GetMetaStats());
        startPosition = Player.instance.transform.position;
    }
    
    
    public void FadeOut()
    {
        foreach (SpriteRenderer sprite in spritesToFade)
        {
            sprite.DOFade(0f, fadeDuration).SetDelay(fadeDelay);
        }
        this.enabled = false;

    }
    
    public void ZoomOut()
    {
        titleImage.transform.DOLocalMoveY(1000f, fadeDuration);
        gradientImage.transform.DOScale(new Vector3(5,5,5), fadeDuration).onComplete += FadeOut;
    }

    public void FixedUpdate()
    {
        if (startPosition == Vector3.zero) return;
        if (Vector3.Distance(Player.instance.transform.position, startPosition) > maxDistance)
        {
            ZoomOut();
        }
        else
        {
            gradientImage.transform.localScale = Vector3.Lerp(Vector3.one*0.5f, Vector3.one*0.75f,Mathf.Sin(Time.time*2f));
        }
    }
}
