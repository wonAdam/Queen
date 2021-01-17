using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WillBarDebug_wondong : MonoBehaviour
{
    [SerializeField] UnityEvent events;

    public void OnClick_AddWillBtn(){
        events.Invoke();
    }
}
