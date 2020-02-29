using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    HeroController heroController;

    void Start()
    {
        heroController = GetComponentInParent<HeroController>();
    }

    void OnAttack()
    {
        if (heroController)
        {
            heroController.OnAttack();
        }
    }
}
