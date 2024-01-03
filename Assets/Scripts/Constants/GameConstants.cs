public static class GameConstants
{
    #region Range Constants
    public const float STARTCONVERSATIONRANGE = 0.05f;
    public const float CAMERASAFERANGE = 0.01f; //Khcach để Cam ngưng tự động move khi move ra new pos

    #endregion

    #region Time Constants
    public const float DELAYPLAYERRUNSTATE = 0.1f;

    #endregion

    #region ANIMATION_CONSTANT
    //-----------------------------//

    #region Player
    public const string DEAD_ANIMATION = "dead";
    #endregion

    #region Shield String Constants
    public const string RUNNINGOUT = "RunningOut";
    public const string IDLE = "Idle";
    #endregion

    //-----------------------------//
    #endregion

    #region Layer Constants
    public const string GROUND_LAYER = "Ground";
    public const string PLAYER_LAYER = "Player";
    public const string IGNORE_ENEMIES_LAYER = "Ignore Enemies";
    public const string SHIELD_LAYER = "Shield";
    #endregion

    #region Render Layer Constants
    public const string RENDER_MAP_LAYER = "Map_Layer";
    #endregion

    #region Tag Constants
    public const string GROUND_TAG = "Ground";
    public const string PLATFORM_TAG = "Platform";
    public const string TRAP_TAG = "Trap";
    public const string BULLET_TAG = "Bullet";
    public const string SHIELD_TAG = "Shield";
    public const string ENEMIES_TAG = "Enemy";
    #endregion

    #region Name Constants
    public const string PLAYER_NAME = "Player";
    #endregion

    #region Axis Constants
    public const string VERTICAL_AXIS = "Vertical";
    public const string HORIZONTAL_AXIS = "Horizontal";
    #endregion

    #region HEALTHPOINT(HP) Constants
    public const int HP_STATE_NORMAL = 0;
    public const int HP_STATE_LOST = 1;
    public const int HP_STATE_TEMP = 2;
    #endregion

    #region Enemies Bullets Constants
    public const int PLANT_BULLET = 0;
    public const int BEE_BULLET = 1;
    public const int TRUNK_BULLET = 2;
    #endregion

    #region Effect
    public const string DASHABLE_EFFECT = "DashableEffect";
    public const string GECKO_APPEAR_EFFECT = "GeckoAppear";
    public const string GECKO_DISAPPEAR_EFFECT = "GeckoDisappear";
    public const string HIT_SHIELD_EFFECT = "HitShieldEffect";
    public const string COLLECT_HP_EFFECT = "CollectHP";
    public const string BROWN_EXPLOSION = "BrownExplosion";
    #endregion
}
