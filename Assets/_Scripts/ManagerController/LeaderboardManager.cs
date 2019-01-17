using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;

/// <summary>
/// Leaderboard manager.
/// Auther : Nikunj Rola
/// </summary>

public class LeaderboardManager : MonoBehaviour
{
    #region GAME_CENTER
    public static LeaderboardManager Instance = new LeaderboardManager();
    /// <summary>
    /// Authenticates to game center.
    /// </summary>
    bool isLoginSuccessful;
    //string leaderBoardID = "PROVIDE_YOUR_LEADERBOARD_ID_HERE";
    public void AuthenticateToGameCenter()
    {
#if UNITY_IPHONE
        Social.localUser.Authenticate(success =>
                                      {
                                          if (success)
                                          {
                                              isLoginSuccessful = true;
                                              Debug.Log("Authentication successful");
                                          }
                                          else
                                          {
                                              isLoginSuccessful = false;
                                              Debug.Log("Authentication failed");
                                          }
                                      });
#endif
    }

    /// <summary>
    /// Reports the score on leaderboard.
    /// </summary>
    /// <param name="score">Score.</param>
    /// <param name="leaderboardID">Leaderboard I.</param>

    public void ReportScore(long score, string leaderboardID)
    {
#if UNITY_IPHONE
        //Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        if (isLoginSuccessful)
        {
            Social.ReportScore(score, leaderboardID, success =>
               {
                   if (success)
                   {
                       Debug.Log("Reported score successfully");
                   }
                   else
                   {
                       Debug.Log("Failed to report score");
                   }

                   Debug.Log(success ? "Reported score successfully" : "Failed to report score"); Debug.Log("New Score:" + score);
               });
        }
        else
        {
            AuthenticateToGameCenter();
        }
#endif
    }

    /// <summary>
    /// Shows the leaderboard UI.
    /// </summary>

    public void ShowLeaderboard(string leaderboardID)
    {
#if UNITY_IPHONE
        if (isLoginSuccessful)
        {
            GameCenterPlatform.ShowLeaderboardUI(leaderboardID, UnityEngine.SocialPlatforms.TimeScope.AllTime);
        }
        else
        {
            AuthenticateToGameCenter();
        }
#endif
    }
    #endregion
}