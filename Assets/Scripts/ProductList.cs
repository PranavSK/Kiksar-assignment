using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductList
{
    private readonly bool[] _isTypeShown = { true, true, true };
    private readonly bool[] _isGroupShown = { true, true, true };
    private int _page = 0;
    private int _pageSize = 0;
    private int _maxPage = 0;
    private readonly List<Product> _products = new();
    private string _searchText = "";
    private readonly List<Product> _filteredProducts = new();

    public int page => _page;
    public int pageSize => _pageSize;
    public int maxPage => _maxPage;

    public List<Product> products => _filteredProducts;

    public ProductList(int numProducts)
    {
        for (var i = 0; i < numProducts; i++)
        {
            var product = RandomProductGenerator.GetRandomProductDetails();
            product.price = $"{i}";
            _products.Add(product);

        }

        UpdateFilteredProducts();
    }

    private bool Filter(Product product)
    {
        var type = (int)product.type;
        var group = (int)product.group;

        var name = product.name.ToLower();
        var searchToken = _searchText.ToLower();
        return _isTypeShown[type] && _isGroupShown[group] && name.Contains(searchToken);
    }

    private void UpdateFilteredProducts()
    {
        _filteredProducts.Clear();
        var filtered = _products.FindAll(Filter);
        if (filtered.Count == 0)
        {
            return;
        }

        if (filtered.Count <= _pageSize)
        {
            _filteredProducts.AddRange(filtered);
            return;
        }

        var start = Math.Max(0, (_page - 1) * _pageSize);
        var end = start + 3 * _pageSize;
        _filteredProducts.AddRange(filtered.GetRange(start, Math.Min(end, filtered.Count) - start));
        _maxPage = (int)Math.Ceiling((double)filtered.Count / _pageSize) - 1;
        _page = Math.Min(_page, _maxPage);
    }

    public void SetPageSize(int pageSize)
    {
        _pageSize = pageSize;
        UpdateFilteredProducts();
    }

    public void NextPage()
    {
        _page = Math.Min(_page + 1, _maxPage);
        UpdateFilteredProducts();
    }

    public void PreviousPage()
    {
        _page = Math.Max(_page - 1, 0);
        UpdateFilteredProducts();
    }

    public void SetWatchShown(bool value)
    {
        _isTypeShown[(int)Product.ProductType.Watch] = value;
        UpdateFilteredProducts();
    }

    public void SetClothesShown(bool value)
    {
        _isTypeShown[(int)Product.ProductType.Clothes] = value;
        UpdateFilteredProducts();
    }

    public void SetJewelryShown(bool value)
    {
        _isTypeShown[(int)Product.ProductType.Jewelry] = value;
        UpdateFilteredProducts();
    }

    public void SetMaleShown(bool value)
    {
        _isGroupShown[(int)Product.ProductGroup.Male] = value;
        UpdateFilteredProducts();
    }

    public void SetFemaleShown(bool value)
    {
        _isGroupShown[(int)Product.ProductGroup.Female] = value;
        UpdateFilteredProducts();
    }

    public void SetKidsShown(bool value)
    {
        _isGroupShown[(int)Product.ProductGroup.Kids] = value;
        UpdateFilteredProducts();
    }

    public void SetSearchString(string text)
    {
        _searchText = text;
        UpdateFilteredProducts();
    }
}