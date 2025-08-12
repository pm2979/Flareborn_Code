using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public RuneCrafting RuneGenaratorManager;

    void Start()
    {

        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(1001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(1001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(1001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(1001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(1001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(2001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(2201));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(2003));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4002));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4002));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4002));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4003));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4003));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(4003));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(5001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(5001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(5001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(6001));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(6002));
        GameManager.Instance.PartyManager.Party.Inventory.AddItem(ItemFactory.CreateItem(6003));
    }

}
