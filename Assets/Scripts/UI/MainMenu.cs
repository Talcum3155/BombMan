using Manger;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartNewGame() =>
            UIManager.Instance.StartNewGame();

        public void ContinueGame() =>
            UIManager.Instance.ContinueGameExecutor();
        
        public void QuitGame() => Application.Quit();
    }
}