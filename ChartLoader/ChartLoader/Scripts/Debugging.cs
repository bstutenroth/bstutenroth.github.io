using UnityEngine;

public class Debugging : MonoBehaviour {

    private void OnGUI()
    {
        string content = ChartLoaderTest.Chart.ToString();
        GUI.color = Color.white;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), content);
    }

}
