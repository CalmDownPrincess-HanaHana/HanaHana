public class Define
{
    /// <summary>
    /// 보스맵 4에서 쓰는 이넘
    /// </summary>
    public enum SnowBoss4State
    {
        pattern4_1,
        pattern4_2,
        pattern4_3,
        pattern4_4,
        pattern4_5
    }

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
        SnowWhite,
        SnowBoss123,
        SnowBoss4,
        ClosetScene,
        StageScene
        , SnowBossClear,
        EndingScene,
        MerMaid
        ,
        None
    }

    public enum PlayerState
    {
        Jump,
        Damaged,
        Walk,
        Idle,
        Fly
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
        Transparent,
        SlowingObj
    }

    public enum RotateDelta
    {
        Left = 90,
        Right = -90,
        UpSideDown = 180
    }
}
