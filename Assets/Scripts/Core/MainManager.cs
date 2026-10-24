// Assets/Scripts/Core/MainManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいで生き続ける唯一の存在にする
    }

    public GamePhase CurrentPhase { get; private set; } = GamePhase.Title;

    private IGamePhaseController currentPhaseController;

    public void ChangePhase(GamePhase newPhase, string sceneName)
    {
        currentPhaseController?.ExitPhase();
        CurrentPhase = newPhase;

        SceneManager.LoadScene(sceneName);
        // シーンロード完了後にExhibitionPhaseController等をFindしてEnterPhase()を呼ぶ
        // (SceneManager.sceneLoadedイベントを購読して行うのが一般的)
    }

    public void RegisterPhaseController(IGamePhaseController controller)
    {
        currentPhaseController = controller;
        currentPhaseController.EnterPhase();
    }
}