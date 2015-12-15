using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    public int Health = 3;

    [SerializeField]
    public Image Heart1;

    [SerializeField]
    public Image Heart2;

    [SerializeField]
    public Image Heart3;

    static Color FadedWhite = new Color(1.0f, 1.0f, 1.0f, 0.35f);
    static Color White = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    // Use this for initialization
    void Start () {
        Heart1.color = White;
        Heart2.color = White;
        Heart3.color = White;
    }

    // Update is called once per frame
    void Update () {
    }

    public bool TakeDamage(int value)
    {
        if(Health <= 0)
        {
            return (true);
        }

        Health -= value;
        switch(Health)
        {
            case 2:
                Heart3.color = FadedWhite;
                break;
            case 1:
                Heart2.color = FadedWhite;
                break;
            case 0:
                Heart1.color = FadedWhite;
                break;
            default:
                break;
        }
        return (Health <= 0);
    }
}
