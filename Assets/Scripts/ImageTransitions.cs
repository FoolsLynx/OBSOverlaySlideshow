using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTransitions : MonoBehaviour
{
    private bool isTransitioning = false;

    public bool IsTransitioning { get => isTransitioning; }

    private ImageSlideshow slideshow;
    public RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slideshow = FindObjectOfType<ImageSlideshow>();
    }


    public IEnumerator SlideRight(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startX = startPoint.x;

        float endX = startX + slideshow.widthSlider.value;
        

        LeanTween.moveLocalX(gameObject, endX, time).setEase(ease);
        while(LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
    }

    public IEnumerator SlideLeft(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startX = startPoint.x;

        float endX = startX - slideshow.widthSlider.value;


        LeanTween.moveLocalX(gameObject, endX, time).setEase(ease); 
        while (LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
    }


    public IEnumerator SlideDown(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startY = startPoint.y;

        float endY = startY - slideshow.heightSlider.value;


        LeanTween.moveLocalY(gameObject, endY, time).setEase(ease);
        while (LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;
    }

    public IEnumerator SlideUp(float time, Vector3 startPoint, LeanTweenType ease = LeanTweenType.notUsed)
    {
        isTransitioning = true;

        rectTransform.localPosition = startPoint;

        float startY = startPoint.y;

        float endY = startY + slideshow.heightSlider.value;


        LeanTween.moveLocalY(gameObject, endY, time).setEase(ease);
        while (LeanTween.isTweening(gameObject))
        {
            yield return null;
        }

        isTransitioning = false;

    }


    



}
