using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;


[RequireComponent(typeof(ScrollRect))]
public class PooledProducts : UIBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    private ScrollRect _scrollRect;
    private RectTransform _content;
    private GridLayoutGroup _gridLayoutGroup;

    private ProductList _productList;
    private ObjectPool<GameObject> _productDetailsPool;
    private float _pageOffset = 0;
    private Vector2 _scrollVelocity = Vector2.zero;
    private bool _isPositionUpdated = false;


    private void GetRequiredComponents()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _content = _scrollRect.content;
        _gridLayoutGroup = _content.GetComponent<GridLayoutGroup>();
    }

    private void UpdateProductDisplay()
    {
        foreach (Transform child in _content)
        {
            if (child.gameObject.activeSelf) _productDetailsPool.Release(child.gameObject);
        }

        var products = _productList.products;
        if (products.Count == 0)
        {
            return;
        }

        foreach (var product in products)
        {
            var item = _productDetailsPool.Get();
            var productDetails = item.GetComponent<ProductDetails>();
            productDetails.SetDetails(product);
        }

        // Update the position of the content to the top
        Debug.Log($"Page: {_productList.page}, Page offset: {_pageOffset}");
        _content.localPosition = new Vector3(0, _productList.page == 0 ? 0 : _pageOffset, 0);
    }

    private void CalculateGrid()
    {
        // Find the number of items that can fit in the viewport
        var viewportHeight = _scrollRect.viewport.rect.height - _gridLayoutGroup.padding.vertical;
        var viewportWidth = _scrollRect.viewport.rect.width - _gridLayoutGroup.padding.horizontal;
        var itemHeight = _gridLayoutGroup.cellSize.y + _gridLayoutGroup.spacing.y;
        var itemWidth = _gridLayoutGroup.cellSize.x + _gridLayoutGroup.spacing.x;
        var numColumns = Math.Max(1, Mathf.FloorToInt((viewportWidth + _gridLayoutGroup.spacing.x) / itemWidth));
        var numRows = Mathf.CeilToInt((viewportHeight + _gridLayoutGroup.spacing.y) / itemHeight);

        _productList.SetPageSize(numColumns * numRows);
        _pageOffset = itemHeight * numRows;
    }

    private void Initialize()
    {
        _productList = new ProductList(100);
        _productDetailsPool = new ObjectPool<GameObject>(
            () => Instantiate(_itemPrefab, _content),
            (item) =>
            {
                item.SetActive(true);
                item.transform.SetAsLastSibling();
            },
            (item) => item.SetActive(false),
            (item) => Destroy(item)
        );
    }

    private IEnumerator WaitForSecondFrame()
    {
        yield return new WaitForEndOfFrame();
        CalculateGrid();
        UpdateProductDisplay();
    }

    protected override void Awake()
    {
        base.Awake();
        GetRequiredComponents();
        Initialize();
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitForSecondFrame());
    }

    protected void Update()
    {
        if (_content == null || _pageOffset == 0) return;

        if (_isPositionUpdated)
        {
            _scrollRect.velocity = _scrollVelocity;
            _isPositionUpdated = false;
        }

        if ((_productList.page == 0 && _content.localPosition.y >= _pageOffset) ||
            (_productList.page < _productList.maxPage && _content.localPosition.y >= 2 * _pageOffset))
        {
            Debug.Log("Next page");
            _productList.NextPage();
            UpdateProductDisplay();
            _scrollVelocity = _scrollRect.velocity;
            _isPositionUpdated = true;
        } else if (_productList.page > 0 && _content.localPosition.y < 0)
        {
            Debug.Log("Previous page");
            _productList.PreviousPage();
            UpdateProductDisplay();
            _scrollVelocity = _scrollRect.velocity;
            _isPositionUpdated = true;
        }
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (_content == null) return;
        CalculateGrid();
        UpdateProductDisplay();
    }

    public void OnWatchToggle(bool value)
    {
        _productList.SetWatchShown(value);
        UpdateProductDisplay();
    }

    public void OnClothesToggle(bool value)
    {
        _productList.SetClothesShown(value);
        UpdateProductDisplay();
    }

    public void OnJewelryToggle(bool value)
    {
        _productList.SetJewelryShown(value);
        UpdateProductDisplay();
    }

    public void OnMaleToggle(bool value)
    {
        _productList.SetMaleShown(value);
        UpdateProductDisplay();
    }

    public void OnFemaleToggle(bool value)
    {
        _productList.SetFemaleShown(value);
        UpdateProductDisplay();
    }

    public void OnKidsToggle(bool value)
    {
        _productList.SetKidsShown(value);
        UpdateProductDisplay();
    }

    public void OnSearchStringChanged(string text)
    {
        _productList.SetSearchString(text);
        UpdateProductDisplay();
    }
}
