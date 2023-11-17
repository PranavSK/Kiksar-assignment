using System;

[Serializable]
public struct Product {
    public enum ProductType
    {
        Watch,
        Clothes,
        Jewelry,
    }

    public enum ProductGroup
    {
        Male,
        Female,
        Kids,
    }
    
    public string name;
    public string price;
    public ProductType type;
    public ProductGroup group;
}