using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition Health;
    public Condition Hunger;
    public Condition Stamina;

    private void Start()
    {
        CharacterManager.Instance.Player.Condition.UICondition = this;
    }
}
