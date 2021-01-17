using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GooglePlayService.InitializeGooglePlayService();
    }

    public void OnClick_LeaderboardBtn()
    {
        GooglePlayService.ShowLeadboard();
    }
}
