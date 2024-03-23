using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] LinesDrawer linesDrawer;

    [Space]
    [SerializeField] private CanvasGroup availableLineCanvasGroup;
    [SerializeField] private GameObject availableLineHolder;
    [SerializeField] private Image availableLinerFill;
    private bool isAvailableLineUIActive = false;

    [Space]
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeDuration;

    private Route activeRoute;

    private void Start()
    {
        fadePanel.DOFade(0f, fadeDuration).From(1f);

        availableLineCanvasGroup.alpha = 0f;

        linesDrawer.OnBeginDraw += OnBeginDrawHandler;
        linesDrawer.OnDraw += OnDrawHandler;
        linesDrawer.OnEndDraw += OnEndDrawHandler;
    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;
        availableLinerFill.color = activeRoute.carColor;
        availableLinerFill.fillAmount = 1f;

        availableLineCanvasGroup.DOFade(1f, 3f).From(0f);
        isAvailableLineUIActive = true;
    }
    private void OnDrawHandler()
    {
        if(isAvailableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLineLength;
            float lineLength = activeRoute.line.length;

            availableLinerFill.fillAmount = 1 - (lineLength/ maxLineLength);
        }
    }

    private void OnEndDrawHandler()
    {
        if(isAvailableLineUIActive)
        {
            isAvailableLineUIActive = false;
            activeRoute = null;

            availableLineCanvasGroup.DOFade(0f, 3f).From(1f);
        }
    }
}
