public static class GameConstants
{
    #region Range Constants
    public const float STARTCONVERSATIONRANGE = 0.05f;
    public const float CAMERASAFERANGE = 0.01f; //Khcach để Cam ngưng tự động move khi move ra new pos

    #endregion

    #region Time Constants
    public const float DELAYPLAYERRUNSTATE = 0.1f;

    #endregion

    #region Shield String Constants
    public const string RUNNINGOUT = "RunningOut";
    public const string IDLE = "Idle";
    #endregion

    #region Layer Constants
    public const string PLAYER_LAYER = "Player";
    public const string IGNORE_ENEMIES_LAYER = "Ignore Enemies";
    #endregion

    #region Tag Constants
    public const string BULLET_TAG = "Bullet";
    public const string SHIELD_TAG = "Shield";
    public const string ENEMIES_TAG = "Enemies";
    #endregion

    #region Name Constants
    public const string PLAYER_NAME = "Player";
    #endregion
}
