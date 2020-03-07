using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    HeroController heroController;
    EnemyController enemyController;

    void Start()
    {
        heroController = GetComponentInParent<HeroController>();
        enemyController = GetComponentInParent<EnemyController>();
    }

    void OnAttack()
    {
        if (heroController)
            heroController.OnAttack();
        if (enemyController)
            enemyController.OnAttack();
    }
}
