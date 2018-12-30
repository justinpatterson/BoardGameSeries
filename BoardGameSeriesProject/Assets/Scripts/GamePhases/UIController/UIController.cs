using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public bool uiActive = false;
    public GameObject uiContainer;

    void Awake() { if (!uiContainer) uiContainer = this.gameObject; }
    public virtual void OpenUI()
    {
        uiContainer.SetActive(true);
        uiActive = true;
    }
    public virtual void CloseUI()
    {
        uiContainer.SetActive(false);
        uiActive = false;
    }
}
