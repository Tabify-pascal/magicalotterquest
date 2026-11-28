
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private Image shieldIcon;
    [SerializeField] private TMP_Text blockText;

    private void OnEnable()
    {
        combatant.OnBlockChanged += Refresh;
        Refresh(combatant.CurrentBlock);
    }

    private void Start()
    {
        Refresh(combatant.CurrentBlock);
    }

    private void OnDisable()
    {
        combatant.OnBlockChanged -= Refresh;
    }

    private void Refresh(int block)
{
    Debug.Log($"BlockUI: {combatant.name} has {block} Block.");

    bool hasBlock = block > 0;

    shieldIcon.enabled = hasBlock;
    blockText.enabled = hasBlock;

    blockText.text = block.ToString();
}
}
