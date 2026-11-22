
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CombatAnimator : MonoBehaviour
{
    private Animator animator;

    private static readonly int SpellTrigger =
        Animator.StringToHash("spell");

    private static readonly int HurtTrigger =
        Animator.StringToHash("hurt");

    private static readonly int BuffTrigger =
        Animator.StringToHash("buff");

    private static readonly int VictoryTrigger =
        Animator.StringToHash("victory");

    private static readonly int DefeatTrigger =
        Animator.StringToHash("defeat");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlaySpell()
    {
        animator.SetTrigger(SpellTrigger);
    }

    public void PlayHurt()
    {
        animator.SetTrigger(HurtTrigger);
    }

    public void PlayBuff()
    {
        animator.SetTrigger(BuffTrigger);
    }

    public void PlayVictory()
    {
        animator.SetTrigger(VictoryTrigger);
    }

    public void PlayDefeat()
    {
        animator.SetTrigger(DefeatTrigger);
    }
}
