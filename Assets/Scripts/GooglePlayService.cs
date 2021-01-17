using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GooglePlayService : MonoBehaviour
{
    public static string STAGE1_ID = "CgkIhofAjJwOEAIQBw";
    public static string STAGE2_ID = "CgkIhofAjJwOEAIQCA";
    public static string STAGE3_ID = "CgkIhofAjJwOEAIQCQ";
    public static string STAGE4_ID = "CgkIhofAjJwOEAIQCg";
    public static string STAGE5_ID = "CgkIhofAjJwOEAIQCw";
    public static string STAGE6_ID = "CgkIhofAjJwOEAIQDA";
    
    public static void InitializeGooglePlayService()
    {
        try{
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => { Debug.Log("Authenticate " + success); });
        }
        catch(Exception exception)
        {
            Debug.Log(exception);
        }
    }

    public static void AddScoreToLeaderboard(float timetik, int stageIdx)
    {
        if (!Social.localUser.authenticated) return;

        // milliseconds to seconds
        timetik *= 1000;

        switch (stageIdx)
        {
            case 1:
                Social.ReportScore((long)timetik, STAGE1_ID, success => {  });
                break;
            case 2:
                Social.ReportScore((long)timetik, STAGE2_ID, success => {  });
                break;
            case 3:
                Social.ReportScore((long)timetik, STAGE3_ID, success => { });
                break;
            case 4:
                Social.ReportScore((long)timetik, STAGE4_ID, success => { });
                break;
            case 5:
                Social.ReportScore((long)timetik, STAGE5_ID, success => { });
                break;
            case 6:
                Social.ReportScore((long)timetik, STAGE6_ID, success => { });
                break;
            default:
                break;
        }

        Debug.Log($" Stage : {stageIdx} | Score : {(long)timetik}");
    }

    public static void ShowLeadboard()
    {
        if(Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
        else
            Debug.Log("You are not authenticated");
    }
}
