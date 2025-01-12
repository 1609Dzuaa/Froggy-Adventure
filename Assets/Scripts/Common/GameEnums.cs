public class GameEnums
{
    //Tạo các enum của State để gán giá trị tương ứng cho Animations
    #region EnumStates
    // ======================================================================== //
    
    public enum EPlayerState
    { idle, run, jump, fall, wallSlide, doubleJump, gotHit, wallJump, dash }

    public enum EMEnemiesState
    { idle, patrol, attack, gotHit }

    public enum ENMEnemiesState
    { idle, attack, gotHit }

    public enum ERhinoState
    { idle, patrol, attack, gotHit, wallHit }

    public enum EBatState
    { idle, patrol, attack, gotHit, sleep, ceilIn, ceilOut, retreat }

    public enum EBunnyState
    { idle, patrol, attackJump, gotHit, attackFall }

    public enum ESnailState
    { idle, patrol, attack, gotHit, shellHit}

    public enum EHedgehogState
    { idle, spikeOut, gotHit, spikeIn, spikeIdle }

    public enum ENPCState
    { idle }

    public enum ESlimState
    { idle, gotHit }

    public enum EGhostState
    { disappear, appear, idle, gotHit }

    public enum EPigState
    { idle, patrol, attackGreen, gotHitGreen, attackRed,  gotHitRed }

    public enum EGeckoState
    { idle, patrol, attack, gotHit, hide  }

    public enum ETrunkState
    { idle, patrol, withdrawn, attack, gotHit }

    public enum EBossState
    { idleShield, shieldRunOut, idleNoShield, hitShieldOn, hitShieldOff, teleport }

    public enum ERockMove { Left, Top, Right, Bottom }

    // ======================================================================== //
    #endregion

    #region EnumBuffs

    public enum EBuffs
    { None, Speed, Jump, Invisible, Shield, Absorb }

    #endregion

    #region EnumEvents

    public enum EEvents
    {
        BulletOnHit,
        BulletOnReceiveInfo,
        PlayerOnTakeDamage,
        PlayerOnJumpPassive,
        PlayerOnInteractWithNPCs,
        PlayerOnStopInteractWithNPCs,
        PlayerOnBeingPushedBack,
        PlayerOnUpdateRespawnPosition,
        OnLevelCompleted, //bắn đi để HUD biết (win/lose) cho event OnHandleLevelCompleted
        OnHandleLevelCompleted, //bắn đi để xử lý các tác vụ khi xong level (save data, reset, display result)
        FanOnBeingDisabled,
        ObjectOnRestart,
        TutorOnDestroy,
        CameraOnShake,
        BossOnSummonMinion,
        BossGateOnClose,
        BossOnDie,
        LevelOnSelected,
        ShopItemOnClick,
        PlayerOnBuyShopItem,
        NotificationOnPopup,
        OnCollectCoin,
        OnSetupLevel, //đc bắn đi để biết thông tin level đó (thgian, skill đã mua,...)
        OnPopupLevelCanToggle, //event cho phép tween và tắt popupLevel (lúc chuyển scene)
        OnReturnMainMenu, //bắn khi player chọn thoát level, để kill tween timer
        OnUnlockSkill,
        OnLockLimitedSkills, //lock những limited skill khi xong 1 level nào đó
        OnCollectFruit,
        OnItemEligibleCheck,
        OnResetLevel, //bắn đi để reset các data cũ từ level trước
        OnUpdateLevel, //bắn đi khi hoàn thành level này và update data level này và level sau
        OnSavePlayerData, //bắn đi khi cần save player data (end level, mua item, ...)
        OnValidatePlayerBuffs, //bắn đi khi play 1 level để xác thực player buff và xử lý
        OnMagnetizeCoins,
        OnBountyMarked,
        OnBeingCursed,
        OnCooldownSkill,
        OnUseSkill,
        OnRewardCoin, //reward coin <=> diệt quái
        OnAidForPlayer, //viện trợ player khi cùng đường (0 máu, 0 còn cắc bạc)
        OnPurchaseSuccess,
        OnChangeHP, //xử lý +, -, mất hoàn toàn hp qua event này
        OnHandlePlayerHP, //xử lý liên quan đến hp player(hiển thị, count timer temp hp)

    }

    #endregion

    #region EnumVfxs, Bullets, Fruits

    public enum EPoolable
    {
        Dashable,
        HitShield,
        GeckoAppear,
        GeckoDisappear,
        CollectFruits,
        CollectDiamond,
        BountyAppearVfx,
        BrownExplosion,
        PlantBullet,
        BeeBullet,
        TrunkBullet,
        Saw,
        RedExplode,
        BossTeleVfx,
        BossAppearVfx,
        BossDeadVfx,
        PlayerDeShieldVfx,
        Apple,
        Banana,
        Cherry,
        Kiwi,
        Melon,
        Orange,
        Pineapple,
        Strawberry,
        EnemyDeathSkullVfx,
        EnemyDeathStarVfx,
        EnemyDeathDustVfx,

    }

    #endregion

    #region Sound

    public enum ESoundName
    {
        StartMenuTheme,
        Level1Theme,
        Level2Theme,
        Level3Theme,
        Level4Theme,
        BossTheme,
        CollectFruitSfx,
        BountyAppearVfxSfx,
        PlayerGotHitSfx,
        PlayerJumpSfx,
        PlayerDashSfx,
        PlayerDeadSfx,
        PlayerLandSfx,
        PlantShootSfx,
        TrunkShootSfx,
        EnemiesDeadSfx,
        GeckoAttackSfx,
        SceneEntrySfx,
        ButtonSelectedSfx,
        CloseButtonSfx,
        BeeShootSfx,
        ShieldBuffSfx,
        AbsorbBuffSfx,
        InvisibleBuffSfx,
        CheckpointSfx,
        NormalBuffUpSfx,
        NormalBuffDownSfx,
        TrampolineSfx,
        RhinoWallHitSfx,
        MushroomScreamSfx,
        GreenPortalSfx,
        SpecialBuffDebuffSfx,
        RhinoAttackSfx,
        HitShieldSfx,
        BeeAngrySfx,
        BoxGotHitSfx,
        BoxBrokeSfx,
        SwitchActivatedSfx,
        DoubleJumpSfx,
        SkillsAchivedSfx,
        BossWallHitSfx,
        BossSummonSfx,
        BossShieldOnSfx,
        BossChargeSfx,
        BossParticleSfx,
        BossTeleSfx,
        BossDeadSfx,
        BossIntroduceSfx
    }

    #endregion

    #region SPECIAL STATES

    public enum ESpecialStates
    {
        Deleted,
        Disabled,
        Actived,
        SkillUnlocked,
        PlayerPositionUpdatedX,
        PlayerPositionUpdatedY,
        PlayerPositionUpdatedZ,
        PlayerSkillUnlockedLV1,
        PlayerSkillUnlockedLV2 //Full unlock
    }

    #endregion

    #region Boss Minions

    public enum EBossMinions
    { Plant, Trunk, Rhino, Chicken, Bunny }

    #endregion

    #region UI

    public enum EPopup
    {
        Level = 0,
        Option = 1,
        Credits = 2,
        Shop = 3,
        ItemShopDetail = 4,
        Notification = 5,
        Ability = 6,
        Result = 7,

    }

    public enum EToggleButton
    {
        Sound = 0,
        Sfx = 1,

    }

    #endregion

    #region Currency

    public enum ECurrency
    {
        Silver = 0,
        Gold = 1,

    }

    #endregion

    #region Skills & Fruits (Related)

    public enum ESkills
    {
        DoubleJump = 0,
        WallSlide = 1,
        Dash = 2,
        Shield = 3,
        Invisible = 4,
        BountyHunter = 5,
        FasterSpeed = 6,
        HigherJump = 7,
        Hourglass = 8,
        Magnetic = 9,
        Curse = 10,

    }

    public enum EFruits
    {
        Apple = 0,
        Banana = 1,
        Cherries = 2,
        Kiwi = 3,
        Melon = 4,
        Orange = 5,
        Pineapple = 6,
        Strawberry = 7
    }

    #endregion

    #region Popup Result

    public enum EButtonName
    {
        NextLevel,
        Replay,
        Home,
        Shop,
        ChooseLevel
    }

    public enum ELevelResult
    {
        Failed,
        Completed
    }

    #endregion

    #region HP
    
    public enum EHPStatus
    {
        AddOneHP,
        MinusOneHP,
        AddOneTempHP,
        MinusOneTempHP,
        LooseAll
    }

    #endregion
}
