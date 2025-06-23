namespace SaltStackers.Common.Helper
{
    public static class PricingHelper
    {
        public static decimal CalculatePartialPrice(decimal basePrice, string priceOperator, bool isPercent, double priceFactor)
        {
            decimal partialPrice = 0;
            switch (priceOperator)
            {
                case "+":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice + (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice + (decimal)priceFactor;
                        }
                    }
                    break;
                case "x":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice * (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice * (decimal)priceFactor;
                        }
                    }
                    break;
                case "-":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice - (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice - (decimal)priceFactor;
                        }
                    }
                    break;
                case "/":
                    {
                        if (isPercent)
                        {
                            partialPrice = basePrice / (basePrice * (decimal)priceFactor) / 100;
                        }
                        else
                        {
                            partialPrice = basePrice / (decimal)priceFactor;
                            // partialPrice = partialPrice * item.Amount;
                        }
                    }
                    break;
            }

            return partialPrice;
        }
    }
}
