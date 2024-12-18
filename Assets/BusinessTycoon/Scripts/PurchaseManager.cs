using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public static PurchaseManager instance = null;

    public List<StoreProduct> Products;

    private static IStoreController m_StoreController;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
#if !UNITY_WEBGL // Disable shop in WebGL version
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
#endif
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var product in Products)
        {
            builder.AddProduct(product.SKU, ProductType.Consumable, new IDs()
            {
                { product.SKU, new string[1]{ GooglePlay.Name } },
                { product.SKU, new string[1]{AppleAppStore.Name } }
            });
        }

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null;
    }

    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                //Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                //MessageBoxUI.instance.Show("Purchase", "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase", null, MessageBoxUI.ButtonOptions.OK, "OK");
            }
        }
        // Otherwise ...
        else
        {
            //Debug.Log("BuyProductID FAIL. Not initialized.");
            //MessageBoxUI.instance.Show("FAIL", "BuyProductID FAIL. Not initialized.", null, MessageBoxUI.ButtonOptions.OK, "OK");
        }
    }

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        //MessageBoxUI.instance.Show("Init failure", error.ToString(), null, MessageBoxUI.ButtonOptions.OK, "OK");
        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string? message)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        //MessageBoxUI.instance.Show("Init failure", error.ToString(), null, MessageBoxUI.ButtonOptions.OK, "OK");
        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var item = GameManager.instance.ProductItems.FirstOrDefault(t => t.product.SKU == args.purchasedProduct.definition.id);

        if (item != null && String.Equals(args.purchasedProduct.definition.id, item.product.SKU, StringComparison.Ordinal))
        {
            //Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            MainUIController.instance.InAppPurchased(item.product);
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            //MessageBoxUI.instance.Show("Init failure", "ProcessPurchase: FAIL. Unrecognized product: '{0}'", null, MessageBoxUI.ButtonOptions.OK, "OK");
            //Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        //Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        //MessageBoxUI.instance.Show("Purchase failure", "OnPurchaseFailed: FAIL.", null, MessageBoxUI.ButtonOptions.OK, "OK");
    }
}