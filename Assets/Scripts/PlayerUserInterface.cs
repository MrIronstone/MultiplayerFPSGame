using UnityEngine;

public class PlayerUserInterface : MonoBehaviour
{
    [SerializeField]
    RectTransform thrushterFuelAmount;

    private PlayerController controller;


    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

     void Update()
    {
        SetFuelAmount(controller.GetThrusterFuelAmount());
    }

    void SetFuelAmount( float _amount)
    { 
        thrushterFuelAmount.localScale = new Vector3(1f, _amount, 1f); 
    }

}
