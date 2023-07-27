public class Define
{
    public enum CameraState
    {
        Player,
        None 
    }
    
    // 새로 만드는 씬들은 모두 여기에 업데이트해주기
    public enum Scene
    {
        MainScene,
        SnowWhiteMap,
        None
    }

    public enum PlayerState
    {
        Jump,
        Damaged,
        Walk,
        Idle
    }
    
    public enum Tags
    {
        Untagged,
        Enemy, 
        Player,
        Platform,
        PlayerDied,
        Flag,
        Item,
        Border,
        FallingBlock,
        Transparent
    }
    
    public enum RotateDelta
    {
        Left = -90,
        Right = 90,
        UpSideDown = 180
    }
}
