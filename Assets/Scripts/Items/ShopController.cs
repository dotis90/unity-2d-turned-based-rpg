using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopState { Menu, Buying, Selling, Busy }

public class ShopController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ShopUI shopUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;

    public event Action OnStart;
    public event Action OnFinish;

    ShopState state;

    Merchant merchant;

    public static ShopController i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    public IEnumerator StartTrading(Merchant merchant)
    {
        this.merchant = merchant;

        OnStart?.Invoke();
        yield return StartMenuState();
    }

    IEnumerator StartMenuState()
    {
        state = ShopState.Menu;

        int selectedChoice = 0;
        yield return DialogueManager.Instance.ShowDialogText("Welcome. I've got some good things on sale stranger.", waitForInput: false,
            choices: new List<string>() { "Buy", "Sell", "Quit" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Buy
            state = ShopState.Buying;
            walletUI.Show();
            shopUI.Show(merchant.AvailableItems, (item) => StartCoroutine(BuyItem(item)), OnBackFromBuying);
        }
        else if (selectedChoice == 1)
        {
            // Sell
            state = ShopState.Selling;
            inventoryUI.gameObject.SetActive(true);
        }
        else if (selectedChoice == 2)
        {
            // Quit
            OnFinish?.Invoke();
            yield break;
        }
    }

    public void HandleUpdate()
    {
        if (state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
        else if (state == ShopState.Buying)
        {
            shopUI.HandleUpdate();
        }
    }

    void OnBackFromSelling()
    {
        inventoryUI.gameObject.SetActive(false);
        StartCoroutine(StartMenuState());
    }

    IEnumerator SellItem(ItemBase item)
    {
        state = ShopState.Busy;

        if (!item.IsSellable)
        {
            yield return DialogueManager.Instance.ShowDialogText("You can't sell that item.");
            state = ShopState.Selling;
            yield break;
        }

        walletUI.Show();

        float sellingPrice = Mathf.Round(item.Price / 2);
        int countToSell = 1;

        int itemCount = inventory.GetItemCount(item);
        if (itemCount > 1)
        {
            yield return DialogueManager.Instance.ShowDialogText($"How many would you like to sell?", waitForInput: false, autoClose: false);

            yield return countSelectorUI.ShowSelector(itemCount, sellingPrice, (selectedCount) => countToSell = selectedCount);

            DialogueManager.Instance.CloseDialog();
        }

        sellingPrice = sellingPrice * countToSell;

        int selectedChoice = 0;
        yield return DialogueManager.Instance.ShowDialogText($"I can give { sellingPrice } for that. Deal?", waitForInput: false,
            choices: new List<string>() { "Yes", "No"},
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Yes
            inventory.RemoveItem(item, countToSell);
            Wallet.i.AddMoney(sellingPrice);
            yield return DialogueManager.Instance.ShowDialogText($"Sold { item.Name } for { sellingPrice} money.");

        }

        walletUI.Close();

        state = ShopState.Selling;
    }

    IEnumerator BuyItem(ItemBase item)
    {
        state = ShopState.Busy;

        yield return DialogueManager.Instance.ShowDialogText($"How many would you like to buy?", waitForInput: false, autoClose: false);

        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(100, item.Price, (selectedCount) => countToBuy = selectedCount);

        DialogueManager.Instance.CloseDialog();

        float totalPrice = item.Price * countToBuy;

        if (Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            yield return DialogueManager.Instance.ShowDialogText($"That will be { totalPrice }.", waitForInput: false,
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if (selectedChoice == 0)
            {
                // Selected Yes
                inventory.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                yield return DialogueManager.Instance.ShowDialogText($"Heh heh heh, thank ya.");
            }
        }
        else
        {
            yield return DialogueManager.Instance.ShowDialogText($"You don't have enough money for that!");
        }

        state = ShopState.Buying;
    }

    void OnBackFromBuying()
    {
        shopUI.Close();
        walletUI.Close();
        StartCoroutine(StartMenuState());
    }
}
