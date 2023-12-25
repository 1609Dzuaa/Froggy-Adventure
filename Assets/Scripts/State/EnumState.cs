public class EnumState
{
    //Tạo các enum của State để gán giá trị tương ứng cho Animations

    //Sử dụng Dictionary với key là các Enum để bindings KEY
    //https://stackoverflow.com/questions/70252468/how-does-the-command-pattern-solve-the-problem-of-hard-wired-commands-requests

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
    { appear, idle, wander, disappear, gotHit }

    public enum EPigState
    { idle, patrol, attackGreen, gotHitGreen, attackRed,  gotHitRed }

    public enum EGeckoState
    { idle, patrol, attack, gotHit, hide  }

    public enum ETrunkState
    { idle, patrol, withdrawn, attack, gotHit }
}
