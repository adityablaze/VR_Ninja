using UnityEngine;

public class SwordSwitch : MonoBehaviour
{
    public GameObject SwordObject;
    public GameObject SwordObjectLeft;
    public GameObject DefaultHands;
    public GameObject DefaultHandsleft;
    public void SwitchToSword(){
        SwordObject.SetActive(true);
        DefaultHands.SetActive(false);
    }
    public void SwitchToHand(){
        SwordObject.SetActive(false);
        DefaultHands.SetActive(true);
    }
    public void SwitchtoSwordLeft(){
        SwordObjectLeft.SetActive(true);
        DefaultHandsleft.SetActive(false);
    }
}
