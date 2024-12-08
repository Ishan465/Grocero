using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Grocero.Services;
using System.Diagnostics;

namespace Grocero.ViewModels
{
    public partial class WelcomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string productName;

        [ObservableProperty]
        private string productImage;

        [ObservableProperty]
        private string walmartPrice;

        [ObservableProperty]
        private string superstorePrice;

        [ObservableProperty]
        private string foodBasicsPrice;

        [ObservableProperty]
        private string priceTip;

        [ObservableProperty]
        private string selectedProduct;

        [ObservableProperty]
        private double convertedWalmartPrice;

        [ObservableProperty]
        private double convertedSuperstorePrice;

        [ObservableProperty]
        private double convertedFoodBasicsPrice;

        public ObservableCollection<string> Products { get; } = new ObservableCollection<string> { "Tomatoes", "Potatoes", "Onions" };

        private readonly ExchangeRateService _exchangeRateService;

        // Dictionary to store prices for each product
        private readonly Dictionary<string, (double Walmart, double Superstore, double FoodBasics)> _productPrices = new();

        public WelcomeViewModel()
        {
            _exchangeRateService = new ExchangeRateService();
            SelectProductCommand = new RelayCommand<string>(OnProductSelected);
            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedProduct))
                {
                    if (!string.IsNullOrWhiteSpace(SelectedProduct))
                    {
                        OnProductSelected(SelectedProduct);
                        await FetchInrExchangeRateAsync();
                    }
                }
            };
        }

        public IRelayCommand SelectProductCommand { get; }

        private void OnProductSelected(string selectedProduct)
        {
            // Check if prices for the product are already generated
            if (!_productPrices.ContainsKey(selectedProduct))
            {
                double walmartPrice = PriceGenerator();
                double superstorePrice = PriceGenerator();
                double foodBasicsPrice = PriceGenerator();

                _productPrices[selectedProduct] = (walmartPrice, superstorePrice, foodBasicsPrice);
            }

            var prices = _productPrices[selectedProduct];
            ProductName = $"Product Selected: {selectedProduct}";
            ProductImage = ProductImageLocation(selectedProduct);
            WalmartPrice = $"Walmart Price: ${prices.Walmart:F2}";
            SuperstorePrice = $"Superstore Price: ${prices.Superstore:F2}";
            FoodBasicsPrice = $"Food Basics Price: ${prices.FoodBasics:F2}";

            PriceTip = $"Tip: {PriceComparer(prices.Walmart, prices.Superstore, prices.FoodBasics)}";
        }

        private async Task FetchInrExchangeRateAsync()
        {
            try
            {
                double inrRate = await _exchangeRateService.GetInrExchangeRateAsync();

                var prices = _productPrices[selectedProduct];
                double walmartPriceInCad = prices.Walmart;
                double superstorePriceInCad = prices.Superstore;
                double foodBasicsPriceInCad = prices.FoodBasics;

                Debug.WriteLine($"INR Rate: {inrRate}");
                Debug.WriteLine($"Walmart Price in CAD: {walmartPriceInCad}");
                Debug.WriteLine($"Superstore Price in CAD: {superstorePriceInCad}");
                Debug.WriteLine($"Food Basics Price in CAD: {foodBasicsPriceInCad}");

                ConvertedWalmartPrice = walmartPriceInCad * inrRate;
                OnPropertyChanged(nameof(ConvertedWalmartPrice));
                Debug.WriteLine($"Converted Walmart Price: {ConvertedWalmartPrice}");

                ConvertedSuperstorePrice = superstorePriceInCad * inrRate;
                OnPropertyChanged(nameof(ConvertedSuperstorePrice));
                Debug.WriteLine($"Converted Superstore Price: {ConvertedSuperstorePrice}");

                ConvertedFoodBasicsPrice = foodBasicsPriceInCad * inrRate;
                OnPropertyChanged(nameof(ConvertedFoodBasicsPrice));
                Debug.WriteLine($"Converted Food Basics Price: {ConvertedFoodBasicsPrice}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error fetching INR exchange rate: {e.Message}");
            }
        }

        private double PriceGenerator()
        {
            Random random = new Random();
            return Math.Round(random.NextDouble() * 2 + 1, 2); // Generate a price between 1 and 3
        }

        private string ProductImageLocation(string selectedProduct)
        {
            return selectedProduct switch
            {
                "Tomatoes" => "tomatoes.jpeg",
                "Potatoes" => "potatoes.png",
                "Onions" => "onions.png",
                _ => "empty.png",
            };
        }

        private string PriceComparer(double walmartPrice, double superstorePrice, double foodBasicsPrice)
        {
            double[] prices = { walmartPrice, superstorePrice, foodBasicsPrice };
            Array.Sort(prices);

            return prices[0] switch
            {
                double price when price == walmartPrice => "Walmart is the cheapest among all",
                double price when price == superstorePrice => "Superstore is the cheapest among all",
                _ => "Food Basics is the cheapest among all",
            };
        }
    }
}
