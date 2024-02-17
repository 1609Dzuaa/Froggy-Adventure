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
        PlayerOnUnlockSkills,
        PlayerOnWinGame,
        FanOnBeingDisabled,
        ObjectOnRestart,
        TutorOnDestroy,
        CameraOnShake,
        BossOnSummonMinion,
        BossGateOnClose,
        BossOnDie
    }

    #endregion

    #region EnumVfxs&Bullets

    public enum EPoolable
    {
        Dashable,
        HitShield,
        GeckoAppear,
        GeckoDisappear,
        CollectFruits,
        CollectDiamond,
        CollectHP,
        BrownExplosion,
        PlantBullet,
        BeeBullet,
        TrunkBullet,
        Saw,
        RedExplode,
        BossTeleVfx,
        BossAppearVfx,
        BossDeadVfx
    }

    #endregion

    #region Sound

    public enum ESoundName
    {
        StartMenuTheme,
        Level1Theme,
        Level2Theme,
        BossTheme,
        CollectFruitSfx,
        CollectHPSfx,
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
}
