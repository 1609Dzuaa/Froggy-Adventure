using UnityEngine;

public static class GameConstants
{
    #region Range Constants
    //Có thể hằng số quá nhỏ dẫn đến việc check vị trí trò chuyện 0 đc như ý
    //=>Tăng giá trị hằng số cho lớn xíu
    public const float CAN_START_CONVERSATION_RANGE = 0.7f;
    public const float NEAR_CONVERSATION_RANGE = 0.1f;

    //Range tối thiểu mà bat có thể flip khi chase player
    //tránh flip loạn xạ khi dist to player quá gần
    public const float BAT_FLIPABLE_RANGE = 0.15f;
    public const float PIG_FLIPABLE_RANGE = 0.6f;
    public const float BOSS_FLIPABLE_RANGE = 0.75f;
    public const float BUNNY_KNOCK_FORCE_DECREASE = 3.0f;
    public const float NEAR_ZERO_THRESHOLD = 0.1f;

    //Định nghĩa các hằng số lock cam move cũng như player dưới
    public const float GAME_MIN_BOUNDARY = 10.0f;
    public const float GAME_MAX_BOUNDARY = 416.0f;

    #endregion

    #region Time Constants

    public const float DELAY_PLAYER_RUN = 0.1f;
    public const float DELAY_PLAYER_DOUBLE_JUMP = 0.15f;
    public const int HOURGLASS_BONUS_TIME = 30;
    public const int BUFF_DURATION = 15;
    public const int BUFF_RUN_OUT_DURATION = 3;
    public const int BUTTON_BUFF_COOLDOWN_DURATION = 30; 
    public const int TRASH_VALUE = 0123; //trash val for tween, dùng khi tween đó 0 quan trọng value
    public const float COOLDOWN_BUTTON = 0.75f; //thgian cooldown 1 button trong result

    #endregion

    #region Animation Constants
    //-----------------------------//

    #region Parameter State
    public const string ANIM_PARA_STATE = "state";
    #endregion

    #region Player
    public const string DEAD_ANIMATION = "dead";
    public const float PLAYER_FALL_GRAV_SCALE = 3.0f;
    public const float PLAYER_INIT_GRAV_SCALE = 1.7f;
    #endregion

    #region Shield String Constants
    public const string RUNNINGOUT = "RunningOut";
    public const string IDLE = "Idle";
    public const string DISABLE = "Disable";
    public const string NO_DECIMAL_FORMAT = "F0";
    public const string DECIMAL_DISPLAY_ONE_PLACE = "F1";
    #endregion

    #region Fire Trap Constants
    public const string FIRE_TRAP_ANIM_GOT_HIT = "GotHit";
    public const string FIRE_TRAP_ANIM_ON = "On";
    #endregion

    #region SceneTrans Fade IN - OUT
    public const string SCENE_TRANS_START = "Start";
    public const string SCENE_TRANS_END = "End";
    #endregion

    #region Checkpoint
    public const string CHECKPOINT_ANIM_FLAG_OUT = "FlagOut";
    public const string CHECKPOINT_ANIM_FLAG_IDLE = "FlagIdle";
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
    public const int RENDER_MAP_ORDER = 5;
    #endregion

    #region Tag Constants
    public const string GROUND_TAG = "Ground";
    public const string PLATFORM_TAG = "Platform";
    public const string TRAP_TAG = "Trap";
    public const string BULLET_TAG = "Bullet";
    public const string SHIELD_TAG = "Shield";
    public const string ENEMIES_TAG = "Enemy";
    public const string BUFF_TAG = "Buff";
    public const string PLAYER_TAG = "Player";
    public const string BOX_TAG = "Box";
    public const string PORTAL_TAG = "Portal";
    public const string ONLY_FAN_TAG = "OnlyFan";
    public const string DEAD_ZONE_TAG = "DeadZone";
    public const string BOSS_SHIELD_TAG = "BossShield";
    public const string CLONE = "Clone";
    public const string COIN_TAG = "Coin";
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

    #region Button Constants

    public const string JUMP_BUTTON = "Jump";
    public const string DASH_BUTTON = "Dash";

    #endregion

    #region GAME LEVEL

    public const int GAME_MENU = 0;
    public const int GAME_LEVEL_1 = 1;
    public const int GAME_LEVEL_2 = 2;
    public const int MAX_GAME_LEVEL = 4;

    #endregion

    #region PLAYER HP

    public const int PLAYER_MAX_HP_LEVEL_1 = 3;
    public const int PLAYER_MAX_HP_LEVEL_2 = 5;
    public const int PLAYER_MAX_HP = 8; //Lượng HP có thể đạt đc khi cộng dồn từ tempHP

    #endregion

    #region FILES

    public const string LEVEL_DATA_PATH = "\\";
    public const string PLAYER_DATA_PATH = "\\Player Data.json";
    public const string SKILLS_DATA_PATH = "\\List Skill.json";
    public const string FRUITS_DATA_PATH = "\\List Fruit.json";

    #endregion

    #region PLAYERPREFS'S KEYS

    //string
    public const string FRUIT_AND_SKILL_CREATED = "ListCreated";


    //int
    public const int CREATED = 1;

    #endregion

    #region DEFAULT CONST

    public const bool DEFAULT_UNLOCK_ITEM = false;
    public const int DEFAULT_ITEM_COUNT = 0;
    public const int DEFAULT_PLAYER_HP = 3;
    public const int DEFAULT_PLAYER_COIN = 0;
    public const bool DEFAULT_LEVEL_UNLOCK = false;
    public const bool DEFAULT_LEVEL_COMPLETED = false;
    public const int DEFAULT_TIME_COMPLETED = 0;

    #endregion

    #region Buffs's Constants

    public const float SPEED_BUFF_FACTOR = 1.3f;
    public const float JUMP_BUFF_FACTOR = 1.3f;
    public const float MAGNETIC_BUFF_RADIUS = 5.0f;

    #endregion
}
