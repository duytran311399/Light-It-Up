using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DuyTran
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager _instance;
        public ScreenLayer screenMain;
        public List<ScreenLayer> screens;
        //public List<PopupLayer> popups;
        //[Header("Panel")]
        //public PanelGold panelGold;
        [HideInInspector] public ScreenLayer screenCurrent;
        public Stack<ScreenLayer> screenHistory;

        private void Awake()
        {
            #region Singleton
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
                DestroyImmediate(gameObject);
            #endregion
        }
        private void Start()
        {
            ShowScreenMain();
        }
        #region History Show Screen Layer
        void PushHistoryLayer(ScreenLayer screenLayer)
        {
            screenHistory.Push(screenLayer);
        }
        void RemoveScreenTopOnHistoryScreenLayer()
        {
            screenHistory.Pop();
        }
        void RemoveAllScreenLayerOnHistory()
        {
            foreach (var s in screenHistory)
            {
                s.CloseLayer();
            }
        }
        #endregion
        #region Show Screen
        public void ShowScreen(ScreenName screenId)
        {
            for (int i = 0; i < screens.Count; i++)
            {
                if (screens[i].screenName == screenId)
                {
                    screens[i].ShowLayer();
                    PushHistoryLayer(screens[i]);
                    OnChangeScreenCurrent(screens[i]);
                    return;
                }
            }
            Debug.LogError("Dont Have Screen Name: " + screenId);
        }

        //public void ShowPopup(string popupId)
        //{
        //    for (int i = 0; i < popups.Count; i++)
        //    {
        //        if (popups[i].popupId == popupId)
        //        {
        //            popups[i].Showpopup();
        //            return;
        //        }
        //    }
        //    Debug.LogError("Dont Have Popup Name: " + popupId);
        //}
        public void ShowScreenMain()
        {
            AudioManager.instance.PlayMusic(StringStatic.BGMainMenu);
            DisableAllScreen();
            screenMain.ShowLayer();
            PushHistoryLayer(screenMain);
        }
        public void BackToScreenPrevious()
        {
            var screen = GetScreenLayerTop();
            screen.CloseLayer();
            RemoveScreenTopOnHistoryScreenLayer();
            var screenPrevious = GetScreenLayerTop();
            screenPrevious.ShowLayer();
        }
        void OnChangeScreenCurrent(ScreenLayer screen)
        {
            screenCurrent = screen;
        }
        #endregion
        #region Close Screen
        public void DisableAllScreen()
        {
            for (int i = 0; i < screens.Count; i++)
            {
                screens[i].CloseLayer();
            }
            screenHistory = new Stack<ScreenLayer>();
        }
        //public void DisableAllPopup()
        //{
        //    for (int i = 0; i < popups.Count; i++)
        //    {
        //        popups[i].ClosePopup();
        //    }
        //}
        #endregion
        #region TryGet Screen
        public ScreenLayer GetScreenLayer(ScreenName screenId)
        {
            var screen = screens.Where(s => s.screenName == screenId).First();
            return screen;
        }
        public ScreenLayer GetScreenLayerTop()
        {
            var screen = screenHistory.Peek();
            return screen;
        }
        //public PopupLayer GetPopupLayer(string popupId)
        //{
        //    var popup = popups.Where(s => s.popupId == popupId).First();
        //    return popup;
        //}
        #endregion
    }

}