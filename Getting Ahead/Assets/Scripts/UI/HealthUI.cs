using MyUILibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour
{
    RadialProgress p1Health;
    RadialProgress p2Health;
    RadialProgress p3Health;
    RadialProgress p4Health;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        p1Health = root.Q<RadialProgress>("P1RadialProgress");
        p2Health = root.Q<RadialProgress>("P2RadialProgress");
        p3Health = root.Q<RadialProgress>("P3RadialProgress");
        p4Health = root.Q<RadialProgress>("P4RadialProgress");
        p1Health.progress = 100;
        p2Health.progress = 100;
        p3Health.progress = 100;
        p4Health.progress = 100;

    }

    private void Update()
    {
        
        
    }
}
