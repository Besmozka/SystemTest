using System.Collections;
using System.Collections.Generic;
using Dogs;
using UnityEngine;
using UnityEngine.UI;

public class BlockPanel : MonoBehaviour, IBlockPanel
{
    public void Block()
    {
        gameObject.SetActive(true);
    }

    public void Unblock()
    {
        gameObject.SetActive(false);
    }
}
