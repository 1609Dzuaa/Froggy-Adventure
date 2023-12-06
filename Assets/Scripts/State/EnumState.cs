public class EnumState
{
    //Tạo các enum của State để gán giá trị tương ứng cho Animations
    public enum EPlayerState
    { idle, run, jump, fall, wallSlide, doubleJump, gotHit, wallJump }

    public enum EMEnemiesState
    { idle, patrol, attack, gotHit }

    public enum ERhinoState
    { idle, patrol, attack, gotHit, wallHit }

    public enum EBatState
    { idle, patrol, attack, gotHit, sleep, ceilIn, ceilOut, retreat }

    public enum EBunnyState
    { idle, patrol, attackJump, gotHit, attackFall }

    public enum ESnailState
    { idle, patrol, attack, gotHit, shellHit}

    public enum EPlantState
    { idle, attack, gotHit }

    public enum EGeckoState
    { idle, patrol, attack, gotHit, hide  }
}
