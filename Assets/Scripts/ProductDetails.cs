using UnityEngine;
using UnityEngine.UI;

public class ProductDetails : MonoBehaviour
{
    [SerializeField] private Product _details;
    [SerializeField] private Image _productImage;
    [SerializeField] private TMPro.TMP_Text _productNameText;

    [SerializeField] private Sprite watchIcon;
    [SerializeField] private Sprite clothesIcon;
    [SerializeField] private Sprite jewelryIcon;

    private void Initialize()
    {
        if (_productNameText != null)
        {
            _productNameText.text = $"{_details.name} - {_details.price}";
        }

        if (_productImage)
        {
            switch (_details.type)
            {
                case Product.ProductType.Watch:
                    _productImage.sprite = watchIcon;
                    break;
                case Product.ProductType.Clothes:
                    _productImage.sprite = clothesIcon;
                    break;
                case Product.ProductType.Jewelry:
                    _productImage.sprite = jewelryIcon;
                    break;
            }
        }

    }

    public void SetDetails(Product details)
    {
        _details = details;
        Initialize();
    }

}
