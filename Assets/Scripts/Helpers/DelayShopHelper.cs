using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;
using static GameEnums;

public static class DelayShopHelper
{
    public static IEnumerator DelayOpenShop()
    {
        yield return new WaitForSeconds(DELAY_OPEN_SHOP);

        UIManager.Instance.TogglePopup(EPopup.Shop, true);
    }
}
