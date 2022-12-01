using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTransitions : MonoBehaviour
{
    private bool isTransitioning = false;

    public bool IsTransitioning { get => isTransitioning; }

    private RawImage image;
    private CanvasGroup canvasGroup;
    private ImageSlideshow slideshow;
    public RectTransform rectTransform;

    private void Start()
    {
        image = GetComponent<RawImage>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        slideshow = FindObjectOfType<ImageSlideshow>();
    }

    public IEnumerator Fade(float time, float alphaTarget, LeanTweenType ease = LeanTweenType.notUsed)
    {
        isTransitioning = true;

        LeanTween.alphaCanvas(canvasGroup, alphaTarget, time).setEase(ease);
        while(LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
        
    }

    public IEnumerator SlideRight(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed, float alpha = 1f)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startX = startPoint.x;

        float endX = startX + slideshow.widthSlider.value;
        
        LeanTween.alphaCanvas(canvasGroup, alpha, time).setEase(ease);
        LeanTween.moveLocalX(gameObject, endX, time).setEase(ease);
        while(LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
    }

    public IEnumerator SlideLeft(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed, float alpha = 1f)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startX = startPoint.x;

        float endX = startX - slideshow.widthSlider.value;

        LeanTween.alphaCanvas(canvasGroup, alpha, time).setEase(ease);
        LeanTween.moveLocalX(gameObject, endX, time).setEase(ease); 
        while (LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
    }


    public IEnumerator SlideDown(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed, float alpha = 1f)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startY = startPoint.y;

        float endY = startY - slideshow.heightSlider.value;

        LeanTween.alphaCanvas(canvasGroup, alpha, time).setEase(ease);
        LeanTween.moveLocalY(gameObject, endY, time).setEase(ease);
        while (LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
    }

    public IEnumerator SlideUp(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed, float alpha = 1f)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startY = startPoint.y;

        float endY = startY + slideshow.heightSlider.value;

        LeanTween.alphaCanvas(canvasGroup, alpha, time).setEase(ease);
        LeanTween.moveLocalY(gameObject, endY, time).setEase(ease);
        while (LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;

    }

    

    



}
