using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeToggle : MonoBehaviour
{
    [SerializeField] GameObject[] upgradePanels;

    void Start()
    {
        ToggleUpgradePanel(0);
    }

    public void ToggleUpgradePanel(int index)
    {
        foreach (var panel in upgradePanels)
        {
            panel.SetActive(false);
        }
        upgradePanels[index].SetActive(true);
    }
}
