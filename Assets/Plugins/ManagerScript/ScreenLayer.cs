using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace DuyTran
{
    public class ScreenLayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IPointerMoveHandler
    {
        CanvasGroup canvasGroup;
        public ScreenName screenName;
        public float durationFade;
        [SerializeField] private UnityEvent afterShow;
        [SerializeField] private UnityEvent beforeClose;
        public void Awake()
        {
            AddCanvasGroup();
        }
        public virtual void CloseLayer()
        {
            beforeClose?.Invoke();
            FadeOutCanvasGroup();
        }
        public virtual void ShowLayer()
        {
            afterShow?.Invoke();
            FadeInCanvasGroup();
        }
        void FadeInCanvasGroup()
        {
            canvasGroup.DOKill();
            gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, durationFade);
        }
        void FadeOutCanvasGroup()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0, durationFade).OnComplete(() => { gameObject.SetActive(false); });
        }
        protected void AddCanvasGroup()
        {
            if (!GetComponent<CanvasGroup>())
                gameObject.AddComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log(name + " Game Object Ented!");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.instance.PlaySound(StringStatic.ButtonClick);
            GameAction.OnClick?.Invoke(eventData.pointerPressRaycast.worldPosition);
            //Debug.LogError(name + " Game Object Clicked!");
            //Debug.Log(eventData.pointerPressRaycast.worldPosition + " eventData.pointerPressRaycast.screenPosition");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log(name + " Game Object Exited!");
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            //Debug.Log(name + " Game Object Draging!");
        }
    }
    public enum ScreenName
    {
        None = 0,
        MainMenu = 1,
        Setting = 2,
        GamePlay = 3,
    }

}