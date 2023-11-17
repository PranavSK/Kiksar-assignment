using UnityEngine;

public class RandomProductGenerator
{
    public static readonly string[] WatchBrands = {
        "OMEGA",
        "Patek Philippe",
        "Seiko",
        "Blancpain",
        "A. Lange & Söhne",
        "Breitling",
        "Audemars Piguet",
        "Cartier ",
        "Jaeger‑LeCoultre",
    };

    public static readonly string[] ClothingTypes = {
        "Trousers",
        "Jacket",
        "T-shirt",
        "Sweater",
        "Dress",
        "Jeans",
        "Sportswear",
        "Shorts",
        "Skirt",
        "Shirt",
        "Suit",
        "Sock",
        "Underwear",
        "Coat",
        "Swimsuit",
        "Polo shirt",
        "Cardigan",
        "Blouse",
        "Leggings",
        "Waistcoat",
        "Sleeveless shirt",
        "Tops",
        "Scarf",
        "Overalls",
    };

    public static readonly string[] ClothingBrands = {
        "Ralph Lauren",
        "Adidas",
        "Levi's",
        "H&M",
        "Chanel",
        "Nike",
        "Prada",
        "Calvin Klein",
        "Dior",
        "Louis Vuitton",
        "Dolce & Gabbana",
        "Tommy Hilfiger",
        "Gucci",
        "Burberry",
        "Lacoste",
        "Puma",
        "Zara",
        "GAP",
        "Versace",
        "Armani",
        "Valentino",
        "Balenciaga",
        "Abercrombie & Fitch",
    };

    public static readonly string[] JewelryTypes = {
        "Necklace",
        "Ring",
        "Earrings",
        "Bracelet",
        "Pendant",
        "Brooch",
        "Cufflink",
        "Bangle",
        "Anklet",
        "Choker",
        "Locket",
        "Nose ring",
    };

    public static readonly string[] JewelryBrands = {
        "Tiffany & Co.",
        "Cartier",
        "Harry Winston",
        "Chopard",
        "Van Cleef & Arpels",
        "Graff",
        "David Yurman",
        "Bvlgari",
        "Piaget",
        "Mikimoto",
        "Chaumet",
        "Boucheron",
        "Garrard",
        "Mellerio dits Meller",
        "Messika",
        "De Beers",
        "FRED",
        "Pomellato",
        "Stephen Webster",
        "Roberto Coin",
        "Repossi",
        "Verdura",
        "Buccellati",
    };

    public static string GetRandomWatchName()
    {
        var brand = WatchBrands[Random.Range(0, WatchBrands.Length)];
        return $"{brand} Watch";
    }

    public static string GetRandomWatchPrice()
    {
        return $"{Random.Range(100, 1000)}$";
    }

    public static string GetRandomClothingName()
    {
        var type = ClothingTypes[Random.Range(0, ClothingTypes.Length)];
        var brand = ClothingBrands[Random.Range(0, ClothingBrands.Length)];
        return $"{brand} {type}";
    }

    public static string GetRandomClothingPrice()
    {
        return $"{Random.Range(10, 100)}$";
    }

    public static string GetRandomJewelryName()
    {
        var type = JewelryTypes[Random.Range(0, JewelryTypes.Length)];
        var brand = JewelryBrands[Random.Range(0, JewelryBrands.Length)];
        return $"{brand} {type}";
    }

    public static string GetRandomJewelryPrice()
    {
        return $"{Random.Range(1000, 10000)}$";
    }

    public static Product GetRandomProductDetails() {
        var randomType = (Product.ProductType)Random.Range(0, 3);
        var randomGroup = (Product.ProductGroup)Random.Range(0, 3);
        var name = "";
        var price = "";

        switch (randomType)
        {
            case Product.ProductType.Watch:
                name = GetRandomWatchName();
                price = GetRandomWatchPrice();
                break;
            case Product.ProductType.Clothes:
                name = GetRandomClothingName();
                price = GetRandomClothingPrice();
                break;
            case Product.ProductType.Jewelry:
                name = GetRandomJewelryName();
                price = GetRandomJewelryPrice();
                break;
        }

        return new Product {
            name = name,
            price = price,
            type = randomType,
            group = randomGroup,
        };
    }
}