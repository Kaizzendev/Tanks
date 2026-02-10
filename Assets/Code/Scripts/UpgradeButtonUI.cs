using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
     public Button button;
     public TextMeshProUGUI nameText;
     public TextMeshProUGUI descriptionText;

    private Upgrade currentUpgrade;

    public void SetUpgrade(Upgrade upgrade)
    {
        currentUpgrade = upgrade;

        nameText.text = upgrade.upgradeName;
        descriptionText.text = upgrade.description;
    }
}