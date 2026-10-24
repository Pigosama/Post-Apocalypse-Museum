// Assets/Scripts/Core/IGamePhaseController.cs
public interface IGamePhaseController
{
    void EnterPhase();   // シーンロード後、このフェーズが始まる時の初期化
    void ExitPhase();    // 別フェーズに切り替わる前のクリーンアップ
}