using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupLayer : MonoBehaviour
{
    public string popupId;
    [SerializeField] private UnityEvent afterShow;
    [SerializeField] private UnityEvent beforeClose;

    public virtual void ClosePopup()
    {
        beforeClose?.Invoke();
        gameObject.SetActive(false);
    }
    public virtual void Showpopup()
    {
        afterShow?.Invoke();
        gameObject.SetActive(true);
    }
}
