public interface IAnomalyEntity
{
    AnomalyState State { get; }
    string AnomalyId { get; }
    string AnomalyName { get; }
    // 鎮圧処理呼び出し用のメソッド等をここに生やす
}

public enum AnomalyState { Normal, Unstable, Breached, Suppressing }