using UnityEngine;
using UnityEngine.InputSystem;

public class CombatDebug : MonoBehaviour
{
    [SerializeField] private Combatant enemy;

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            enemy.TakeDamage(2);

            Debug.Log(
                $"Ogre HP: {enemy.CurrentHealth}/{enemy.MaxHealth}"
            );
        }
    }
}
