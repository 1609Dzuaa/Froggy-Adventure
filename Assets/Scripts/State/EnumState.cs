public class EnumState
{
    //Tạo các enum của State để gán giá trị tương ứng cho Animations
    public enum EPlayerState
    { idle, run, jump, fall, wallSlide, doubleJump, gotHit }

    public enum ERhinoState
    { idle, run, wallHit, gotHit, patrol }

    public enum EMushroomState
    { idle, walk, run, gotHit }

    public enum EPlantState
    { idle, attack, gotHit }

    public enum EBatState
    { sleep, idle, ceilIn, ceilOut, fly, gotHit, chase, retreat }

    public enum EGeckoState
    { idle, patrol, attack, gotHit, hide  }

    public enum EBunnyState
    { idle, patrol, jump, fall, gotHit }
}
