using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Nutrition;
using SaltStackers.Domain.Models.Operation;
using System.Text.Json;

namespace SaltStackers.Data.Seed
{
    public partial class Seeder
    {
        private List<Unit> _units;
        private List<Ingredient> _ingredients;
        private List<Diet> _diets;
        private List<Tag> _tags;
        private List<OverheadCost> _overheadCosts;

        public void Nutrition()
        {
            //Kitchens();

            Unit();

            //Diets();

            //OverheadCost();

            //Tags();

            //Ingredients();

            //Foods();

            //Combos();
        }

        private void Kitchens()
        {
            _modelBuilder.Entity<Kitchen>().HasData(
                new Kitchen
                {
                    Id = 1,
                    Title = "Default"
                }
            );
        }

        private int GetIngredient(string title, string type, int unit)
        {
            var ingredient = _ingredients
                    .FirstOrDefault(p => p.Title.ToLower() == title.ToLower());

            var ingredientType = ingredient.IngredientTypes
                .FirstOrDefault(p => p.Title.ToLower() == type.ToLower());

            var ingredientTypeUnit = ingredientType.IngredientTypeUnits
                .FirstOrDefault(p => p.UnitId == unit);

            return ingredientTypeUnit.Id;
        }

        private int GetIngredientByName(string displayName, string unit, string unitCategory = "Solids")
        {
            var ingredientType = _ingredients.SelectMany(p => p.IngredientTypes)
                .FirstOrDefault(p => p.DisplayTitle == displayName);

            var unitId = _units.FirstOrDefault(p => p.Sign == unit && p.Category == unitCategory).Id;

            var ingredientTypeUnit = ingredientType.IngredientTypeUnits
                .FirstOrDefault(p => p.UnitId == unitId);

            return ingredientTypeUnit.Id;
        }

        private void Unit()
        {
            _units = new List<Unit>
            {
                new() {
                    Id = 1,
                    Category = "Solids",
                    Title = "Gram",
                    Sign = "g",
                    ConversionFactor = 1
                },
                new() {
                    Id = 2,
                    Category = "Solids",
                    Title = "Kilogram",
                    Sign = "kg",
                    ConversionFactor = 1000
                },
                new() {
                    Id = 3,
                    Category = "Solids",
                    Title = "Pound",
                    Sign = "lb",
                    ConversionFactor = 454
                },
                new() {
                    Id = 4,
                    Category = "Solids",
                    Title = "Ounce",
                    Sign = "oz",
                    ConversionFactor = 28.35
                },
                new() {
                    Id = 5,
                    Category = "Solids",
                    Title = "Cup",
                    Sign = "cup",
                    HasCustomConversionFactor = true
                },
                new() {
                    Id = 6,
                    Category = "Solids",
                    Title = "Table Spoon",
                    Sign = "tbsp",
                    HasCustomConversionFactor = true
                },
                new() {
                    Id = 7,
                    Category = "Solids",
                    Title = "Tea Spoon",
                    Sign = "tsp",
                    HasCustomConversionFactor = true
                },
                new() {
                    Id = 8,
                    Category = "Solids",
                    Title = "Each",
                    Sign = "ea",
                    HasCustomConversionFactor = true
                },
                new() {
                    Id = 9,
                    Category = "Solids",
                    Title = "Pinch",
                    Sign = "pinch",
                    ConversionFactor = 0.35
                },
                new() {
                    Id = 10,
                    Category = "Solids",
                    Title = "Bunch",
                    Sign = "bunch",
                    HasCustomConversionFactor = true
                },
                new() {
                    Id = 11,
                    Category = "Liquids",
                    Title = "Milliliter",
                    Sign = "ml",
                    ConversionFactor = 1
                },
                new() {
                    Id = 12,
                    Category = "Liquids",
                    Title = "Liter",
                    Sign = "l",
                    ConversionFactor = 1000
                },
                new() {
                    Id = 13,
                    Category = "Liquids",
                    Title = "Fluid Ounce",
                    Sign = "fl-oz",
                    ConversionFactor = 29.57
                },
                new() {
                    Id = 14,
                    Category = "Liquids",
                    Title = "Cup",
                    Sign = "cup",
                    ConversionFactor = 236.56
                },
                new() {
                    Id = 15,
                    Category = "Liquids",
                    Title = "Table Spoon",
                    Sign = "tbsp",
                    ConversionFactor = 15
                },
                new() {
                    Id = 16,
                    Category = "Liquids",
                    Title = "Tea Spoon",
                    Sign = "tsp",
                    ConversionFactor = 5
                }
            };
            _modelBuilder.Entity<Unit>().HasData(
                _units
            );
        }

        private void OverheadCost()
        {
            _overheadCosts = new List<OverheadCost>
            {
                new OverheadCost
                {
                    Id = 1,
                    Title = "Rent",
                    DefaultValue = (decimal?)1.6,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 2,
                    Title = "Employee",
                    DefaultValue = (decimal?)1.5,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 3,
                    Title = "Packaging Fee Personel",
                    DefaultValue = null,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 4,
                    Title = "Cooking",
                    DefaultValue = null,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 5,
                    Title = "Label",
                    DefaultValue = (decimal?)0.20,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 6,
                    Title = "Dish",
                    DefaultValue = (decimal?)0.30,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 8,
                    Title = "Marketing",
                    DefaultValue = null,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 9,
                    Title = "Sales",
                    DefaultValue = null,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 10,
                    Title = "Office",
                    DefaultValue = null,
                    Category = OverheadCategory.Recipe
                },
                new OverheadCost
                {
                    Id = 11,
                    Title = "Other Cost",
                    DefaultValue = null,
                    Category = OverheadCategory.Recipe
                }
            };

            _modelBuilder.Entity<OverheadCost>().HasData(
               _overheadCosts
           );
        }

        private void Ingredients()
        {
            var gram = _units.FirstOrDefault(p => p.Sign == "g").Id;
            var kilogram = _units.FirstOrDefault(p => p.Sign == "kg").Id;
            var pound = _units.FirstOrDefault(p => p.Sign == "lb").Id;
            var pinch = _units.FirstOrDefault(p => p.Sign == "pinch").Id;
            var each = _units.FirstOrDefault(p => p.Sign == "ea").Id;
            var bunch = _units.FirstOrDefault(p => p.Sign == "bunch").Id;
            var liter = _units.FirstOrDefault(p => p.Sign == "l").Id;
            var mililiter = _units.FirstOrDefault(p => p.Sign == "ml").Id;
            var cup = _units.FirstOrDefault(p => p.Sign == "cup" && p.Category == "Liquids").Id;
            var tableSpoon = _units.FirstOrDefault(p => p.Sign == "tbsp" && p.Category == "Liquids").Id;
            var tableSpoonSolid = _units.FirstOrDefault(p => p.Sign == "tbsp" && p.Category == "Solids").Id;
            var teaSpoon = _units.FirstOrDefault(p => p.Sign == "tsp" && p.Category == "Liquids").Id;
            var teaSpoonSolid = _units.FirstOrDefault(p => p.Sign == "tsp" && p.Category == "Solids").Id;

            _ingredients = new List<Ingredient>
            {
                new Ingredient {
                    Title = "Organic Chicken Breast",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "For BBQ",
                            DisplayTitle = "Organic BBQ Chicken Breast",
                            BasePrice = (decimal)0.025,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.009,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 35,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.4,
                                    Protein = (decimal)0.322,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0028,
                                    Cholesterol = (decimal)0.7,
                                    Carbohydrate = (decimal)0.014,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.49,
                                    Iron = (decimal)0.008,
                                    VitaminA = (decimal)0.18,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.009
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "For Turmeric",
                            DisplayTitle = "Organic Turmeric Chicken Breast",
                            BasePrice = (decimal)0.025,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.006,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 25,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.4,
                                    Protein = (decimal)0.322,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0028,
                                    Cholesterol = (decimal)0.7,
                                    Carbohydrate = (decimal)0.014,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.49,
                                    Iron = (decimal)0.008,
                                    VitaminA = (decimal)0.18,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.009
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Chicken Breast",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "For BBQ",
                            DisplayTitle = "BBQ Chicken Breast",
                            BasePrice = (decimal)0.014,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.004,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 35,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.4,
                                    Protein = (decimal)0.322,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0028,
                                    Cholesterol = (decimal)0.7,
                                    Carbohydrate = (decimal)0.014,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.49,
                                    Iron = (decimal)0.008,
                                    VitaminA = (decimal)0.18,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.009
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "For Turmeric",
                            DisplayTitle = "Turmeric Chicken Breast",
                            BasePrice = (decimal)0.025,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.003,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 25,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.4,
                                    Protein = (decimal)0.322,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0028,
                                    Cholesterol = (decimal)0.7,
                                    Carbohydrate = (decimal)0.014,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.49,
                                    Iron = (decimal)0.008,
                                    VitaminA = (decimal)0.18,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.009
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Yam",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Cut with Skin",
                            DisplayTitle = "Cut Yam with Skin",
                            BasePrice = (decimal)0.0037,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0002,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 5,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.18,
                                    Protein = (decimal)0.0153,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0004,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.2788,
                                    DietaryFiber = (decimal)0.041,
                                    Sugars = (decimal)0.005,
                                    Sudium = (decimal)0.09,
                                    Iron = (decimal)0.0054,
                                    VitaminA = (decimal)1.38,
                                    VitaminC = (decimal)0.171,
                                    Zinc = (decimal)0.0024
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Potato",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Cut with Skin",
                            DisplayTitle = "Cut Potato with Skin",
                            BasePrice = (decimal)0.0015,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0007,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 42,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.93,
                                    Protein = (decimal)0.0249,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.00034,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.2107,
                                    DietaryFiber = (decimal)0.022,
                                    Sugars = (decimal)0.0118,
                                    Sudium = (decimal)1.64,
                                    Iron = (decimal)0.0108,
                                    VitaminA = (decimal)0.01,
                                    VitaminC = (decimal)0.096,
                                    Zinc = (decimal)0.0036
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Broccoli",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Chopped Steamer",
                            DisplayTitle = "Steamed Broccoli",
                            BasePrice = (decimal)0.006,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.015,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 15,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.34,
                                    Protein = (decimal)0.0282,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.00114,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0664,
                                    DietaryFiber = (decimal)0.026,
                                    Sugars = (decimal)0.017,
                                    Sudium = (decimal)0.33,
                                    Iron = (decimal)0.0073,
                                    VitaminA = (decimal)0.31,
                                    VitaminC = (decimal)0.892,
                                    Zinc = (decimal)0.0041
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Baby Carrot",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Ready in Costco",
                            DisplayTitle = "Steamed Organic Baby Carrot",
                            BasePrice = (decimal)0.0044,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.012,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.41,
                                    Protein = (decimal)0.0093,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0004,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0958,
                                    DietaryFiber = (decimal)0.028,
                                    Sugars = (decimal)0.0454,
                                    Sudium = (decimal)0.0007,
                                    Iron = (decimal)0.003,
                                    VitaminA = (decimal)8.35,
                                    VitaminC = (decimal)0.059,
                                    Zinc = (decimal)0.0024
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Zucchini",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Chopped Oven Roasted",
                            DisplayTitle = "Oven Roasted Zucchini",
                            BasePrice = (decimal)0.0022,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.014,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 20,
                                    ConversionFactor = 0,
                                    Energy = (decimal)0.17,
                                    Protein = (decimal)0.012,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.001,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.031,
                                    DietaryFiber = (decimal)0.01,
                                    Sugars = (decimal)0.025,
                                    Sudium = (decimal)0.08,
                                    Iron = (decimal)0.0036,
                                    VitaminA = (decimal)0.36,
                                    VitaminC = (decimal)0.3,
                                    Zinc = (decimal)0.005
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Olive Oil",
                    UnitId = liter,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Virgin",
                            DisplayTitle = "Virgin Olive Oil",
                            BasePrice = (decimal)0.09,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = tableSpoon,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 80,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 1,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = (decimal)0.6,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = (decimal)10.8
                                },
                                new IngredientTypeUnit
                                {
                                    UnitId = teaSpoon,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 5,
                                    Energy = 40,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.5,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = (decimal)0.28,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = (decimal)10.8
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Spice",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Himalyan Pink Salt",
                            DisplayTitle = "Himalyan Pink Salt",
                            BasePrice = (decimal)0.002,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = pinch,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)163.3,
                                    Iron = (decimal)0.142,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Black Pepper",
                            DisplayTitle = "Black Pepper",
                            BasePrice = (decimal)0.0157,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = pinch,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.7987,
                                    Protein = (decimal)0.0388,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0035,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.2301,
                                    DietaryFiber = (decimal)0.0941,
                                    Sugars = (decimal)0.0023,
                                    Sudium = (decimal)0.0001,
                                    Iron = (decimal)0.1024,
                                    VitaminA = (decimal)0.0532,
                                    VitaminC = (decimal)0.0745,
                                    Zinc = (decimal)0.005
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Garlic Powder",
                            DisplayTitle = "Garlic Powder",
                            BasePrice = (decimal)0.02,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = pinch,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = (decimal)0.1,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 90,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 4,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "BBQ Chicken",
                            DisplayTitle = "BBQ Spice for Chicken",
                            BasePrice = (decimal)0.0645,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = teaSpoonSolid,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 1,
                                    Energy = 0,
                                    Protein = (decimal)0.4,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 760,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = (decimal)14.4,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Dried Parsley",
                            DisplayTitle = "Dried Parsley",
                            BasePrice = (decimal)0.03,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.5,
                                    Protein = (decimal)0.2,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.4,
                                    DietaryFiber = (decimal)0.3,
                                    Sugars = (decimal)0.1,
                                    Sudium = 3,
                                    Iron = (decimal)0.8,
                                    VitaminA = (decimal)13.5,
                                    VitaminC = (decimal)0.9,
                                    Zinc = (decimal)0.1
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "BBQ Steak",
                            DisplayTitle = "BBQ Spice for Steak",
                            BasePrice = (decimal)0.0121,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = teaSpoonSolid,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 1,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = (decimal)0.1,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 200,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = (decimal)14.4,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Lime",
                    UnitId = each,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Side Meal",
                            DisplayTitle = "Slice Lime",
                            BasePrice = (decimal)0.083,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0253,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 15,
                                    Energy = (decimal)0.25,
                                    Protein = 5,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)1.8,
                                    DietaryFiber = (decimal)0.5,
                                    Sugars = (decimal)0.3,
                                    Sudium = (decimal)0.5,
                                    Iron = (decimal)0.2,
                                    VitaminA = (decimal)0.4,
                                    VitaminC = (decimal)6.3,
                                    Zinc = (decimal)11.8
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Brown Rice",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Australian Calrose",
                            DisplayTitle = "Australian Calrose Brown Rice",
                            BasePrice = (decimal)0.0007,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "-",
                                    AmountFactor = 50,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.78,
                                    Protein = (decimal)0.033,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.356,
                                    DietaryFiber = (decimal)0.022,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = (decimal)0.0048,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.011
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Cabbage",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Red",
                            DisplayTitle = "Red Cabbage",
                            BasePrice = (decimal)0.0029,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.00216,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 25,
                                    ConversionFactor = null,
                                    Energy = (decimal)9.3,
                                    Protein = (decimal)0.4,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)2.3,
                                    DietaryFiber = (decimal)0.6,
                                    Sugars = (decimal)1.1,
                                    Sudium = 8,
                                    Iron = (decimal)0.3,
                                    VitaminA = (decimal)18.6,
                                    VitaminC = 19,
                                    Zinc = (decimal)0.1
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Chopped White",
                            DisplayTitle = "White Cabbage",
                            BasePrice = (decimal)0.0022,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0019,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 23,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.478,
                                    Protein = (decimal)0.0217,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = (decimal)0.0475,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = (decimal)0.0117,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Avocado",
                    UnitId = each,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Avocado Puree",
                            DisplayTitle = "Avocado Puree",
                            BasePrice = (decimal)1.2,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)2.105,
                                    Protein = (decimal)0.0175,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.026,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0877,
                                    DietaryFiber = (decimal)0.0526,
                                    Sugars = 0,
                                    Sudium = (decimal)0.004,
                                    Iron = (decimal)0.0056,
                                    VitaminA = (decimal)0.063,
                                    VitaminC = (decimal)0.0912,
                                    Zinc = (decimal)0.0063
                                },
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 57,
                                    Energy = 120,
                                    Protein = 1,
                                    TransFat = 0,
                                    SaturatedFat = 2,
                                    Cholesterol = 0,
                                    Carbohydrate = 5,
                                    DietaryFiber = 3,
                                    Sugars = 0,
                                    Sudium = (decimal)0.2,
                                    Iron = (decimal)0.3,
                                    VitaminA = (decimal)3.6,
                                    VitaminC = (decimal)5.2,
                                    Zinc = (decimal)0.4
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Carrot",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Grated",
                            DisplayTitle = "Organic Grated Carrot",
                            BasePrice = (decimal)0.00176,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0033,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 25,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.333,
                                    Protein = (decimal)0.0095,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.076,
                                    DietaryFiber = (decimal)0.019,
                                    Sugars = (decimal)0.0476,
                                    Sudium = (decimal)0.619,
                                    Iron = (decimal)0.0018,
                                    VitaminA = (decimal)5.064,
                                    VitaminC = (decimal)0.0357,
                                    Zinc = (decimal)0.0014
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Kale",
                    UnitId = bunch,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Green",
                            DisplayTitle = "Green Kale",
                            BasePrice = (decimal)0.5625,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = cup,
                                    PriceOperator = "+",
                                    PriceFactor = 0.225,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 50,
                                    Energy = (decimal)17.5,
                                    Protein = (decimal)1.5,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)2.2,
                                    DietaryFiber = (decimal)2.1,
                                    Sugars = (decimal)0.4,
                                    Sudium = (decimal)0.5,
                                    Iron = (decimal)0.8,
                                    VitaminA = (decimal)122.5,
                                    VitaminC = (decimal)46.7,
                                    Zinc = (decimal)0.0014
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Sauce",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "BBQ",
                            DisplayTitle = "BBQ Sauce",
                            BasePrice = (decimal)0.053,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = tableSpoon,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 10,
                                    Energy = 20,
                                    Protein = (decimal)0.1,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)4.67,
                                    DietaryFiber = 0,
                                    Sugars = 4,
                                    Sudium = 100,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = (decimal)1.2,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Ranch",
                            DisplayTitle = "Sauce Ranch",
                            BasePrice = (decimal)0.0058,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)4.67,
                                    Protein = (decimal)0.013,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.1,
                                    Cholesterol = (decimal)0.33,
                                    Carbohydrate = (decimal)0.067,
                                    DietaryFiber = 0,
                                    Sugars = (decimal)0.067,
                                    Sudium = (decimal)8.67,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Hot",
                            DisplayTitle = "Hot Sauce",
                            BasePrice = (decimal)0.0084,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 38,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Mayonnaise",
                            DisplayTitle = "Mayonnaise Sauce",
                            BasePrice = (decimal)0.049,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = tableSpoon,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 10,
                                    Energy = 90,
                                    Protein = (decimal)0.2,
                                    TransFat = 0,
                                    SaturatedFat = 1,
                                    Cholesterol = 10,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 70,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Organic Pumpkin Seed",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Graines De Citrouille",
                            DisplayTitle = "Organic Pumpkin Seed",
                            BasePrice = (decimal)0.11,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = tableSpoonSolid,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 10,
                                    Energy = (decimal)57.4,
                                    Protein = 3,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.9,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)1.5,
                                    DietaryFiber = (decimal)0.7,
                                    Sugars = (decimal)0.1,
                                    Sudium = (decimal)1.8,
                                    Iron = (decimal)0.8,
                                    VitaminA = 0,
                                    VitaminC = (decimal)0.2,
                                    Zinc = (decimal)0.8
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Dried Cranberry",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Ocean Spray",
                            DisplayTitle = "Dried Cranberry",
                            BasePrice = (decimal)0.083,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = tableSpoonSolid,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 10,
                                    Energy = (decimal)30.8,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)8.2,
                                    DietaryFiber = (decimal)0.6,
                                    Sugars = (decimal)6.5,
                                    Sudium = (decimal)0.3,
                                    Iron = 0,
                                    VitaminA = (decimal)0.3,
                                    VitaminC = (decimal)1.4,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Vinegar",
                    UnitId = liter,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Organic Apple Cider",
                            DisplayTitle = "Organic Apple Cider Vinegar",
                            BasePrice = (decimal)0.0042,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = mililiter,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                },
                                new IngredientTypeUnit
                                {
                                    UnitId = tableSpoon,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 10,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Balsamic Vinegar of Modena",
                            DisplayTitle = "Balsamic Vinegar",
                            BasePrice = (decimal)0.0175,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = mililiter,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 2,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.467,
                                    DietaryFiber = 0,
                                    Sugars = (decimal)0.4,
                                    Sudium = (decimal)0.34,
                                    Iron = (decimal)0.0013,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Aspargus",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Oven-Roasted",
                            DisplayTitle = "Oven-Roasted Asparagus",
                            BasePrice = (decimal)0.0121,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0109,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 40,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.2,
                                    Protein = (decimal)0.0267,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.04,
                                    DietaryFiber = (decimal)0.02,
                                    Sugars = (decimal)0.02,
                                    Sudium = (decimal)0.02,
                                    Iron = (decimal)0.012,
                                    VitaminA = (decimal)1.2,
                                    VitaminC = (decimal)0.06,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Barley",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Organic Red Mill",
                            DisplayTitle = "Organic Barley",
                            BasePrice = (decimal)0.004,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 4,
                                    Protein = (decimal)0.171,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.11,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.657,
                                    DietaryFiber = (decimal)0.114,
                                    Sugars = (decimal)0.043,
                                    Sudium = 0,
                                    Iron = (decimal)0.051,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.1
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Bellpepper",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Chopped",
                            DisplayTitle = "Chopped Bellpepper",
                            BasePrice = (decimal)0.0076,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0026,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.26,
                                    Protein = (decimal)0.0099,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0603,
                                    DietaryFiber = (decimal)0.021,
                                    Sugars = (decimal)0.042,
                                    Sudium = (decimal)0.04,
                                    Iron = (decimal)0.0043,
                                    VitaminA = (decimal)1.57,
                                    VitaminC = (decimal)1.277,
                                    Zinc = (decimal)0.0025
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Cauliflower",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Steamed",
                            DisplayTitle = "Steamed Cauliflower",
                            BasePrice = (decimal)0.0077,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0068,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 38,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.23,
                                    Protein = (decimal)0.0184,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0045,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0411,
                                    DietaryFiber = (decimal)0.027,
                                    Sugars = (decimal)0.0141,
                                    Sudium = (decimal)0.001,
                                    Iron = (decimal)0.018,
                                    VitaminA = (decimal)0.02,
                                    VitaminC = (decimal)0.74,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Celery",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Chopped",
                            DisplayTitle = "Chopped Celery",
                            BasePrice = (decimal)0.0045,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.002,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.14,
                                    Protein = (decimal)0.0069,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0004,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0297,
                                    DietaryFiber = (decimal)0.016,
                                    Sugars = (decimal)0.0183,
                                    Sudium = (decimal)0.8,
                                    Iron = (decimal)0.002,
                                    VitaminA = (decimal)4.49,
                                    VitaminC = (decimal)0.031,
                                    Zinc = (decimal)0.0013
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Dish",
                    UnitId = each,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "For Dressing",
                            DisplayTitle = "Dish Dressing",
                            BasePrice = (decimal)0.12,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "For Salad",
                            DisplayTitle = "Dish Salad",
                            BasePrice = (decimal)0.3,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "For Soup",
                            DisplayTitle = "Dish Soup",
                            BasePrice = (decimal)0.5,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = 0,
                                    Protein = 0,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = 0,
                                    Iron = 0,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Fruit",
                    UnitId = each,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Organic Banana",
                            DisplayTitle = "Organic Banana",
                            BasePrice = (decimal)0.4,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 150,
                                    Energy = 105,
                                    Protein = (decimal)1.3,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 27,
                                    DietaryFiber = (decimal)3.1,
                                    Sugars = (decimal)14.4,
                                    Sudium = (decimal)0.2,
                                    Iron = (decimal)0.3,
                                    VitaminA = (decimal)3.6,
                                    VitaminC = (decimal)5.2,
                                    Zinc = (decimal)0.4
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Organic Apple",
                            DisplayTitle = "Organic Apple",
                            BasePrice = (decimal)0.6,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 100,
                                    Energy = 71,
                                    Protein = (decimal)0.4,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)19.1,
                                    DietaryFiber = (decimal)3.3,
                                    Sugars = (decimal)14.3,
                                    Sudium = (decimal)0.3,
                                    Iron = (decimal)0.3,
                                    VitaminA = (decimal)3.6,
                                    VitaminC = (decimal)4.6,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Green Bean",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Steamed",
                            DisplayTitle = "Steamed Green Bean",
                            BasePrice = (decimal)0.0066,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0024,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 12,
                                    ConversionFactor = 150,
                                    Energy = (decimal)0.31,
                                    Protein = (decimal)0.0182,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0003,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0713,
                                    DietaryFiber = (decimal)0.034,
                                    Sugars = (decimal)0.014,
                                    Sudium = (decimal)0.0001,
                                    Iron = (decimal)0.0104,
                                    VitaminA = (decimal)0.38,
                                    VitaminC = (decimal)0.163,
                                    Zinc = (decimal)0.0024
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Juice",
                    UnitId = each,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Oasis Classic 100%",
                            DisplayTitle = "Oasis Classic 100% Orange Juice",
                            BasePrice = (decimal)0.75,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = each,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 300,
                                    Energy = 130,
                                    Protein = 1,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = 32,
                                    DietaryFiber = 0,
                                    Sugars = 30,
                                    Sudium = 25,
                                    Iron = (decimal)0.2,
                                    VitaminA = (decimal)0.2,
                                    VitaminC = 1,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Milk",
                    UnitId = liter,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "2% Fat",
                            DisplayTitle = "Milk 2% Fat",
                            BasePrice = (decimal)0.336,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = cup,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = 240,
                                    Energy = 124,
                                    Protein = 8,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)3.1,
                                    Cholesterol = (decimal)19.7,
                                    Carbohydrate = 12,
                                    DietaryFiber = 0,
                                    Sugars = 12,
                                    Sudium = (decimal)115.6,
                                    Iron = 0,
                                    VitaminA = 45,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.36
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Mushroom",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Steamed",
                            DisplayTitle = "Steamed Mushroom",
                            BasePrice = (decimal)0.0077,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0057,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.289,
                                    Protein = (decimal)0.0408,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0026,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.043,
                                    DietaryFiber = (decimal)0.0132,
                                    Sugars = (decimal)0.0263,
                                    Sudium = (decimal)0.0658,
                                    Iron = (decimal)0.0047,
                                    VitaminA = 0,
                                    VitaminC = (decimal)0.0355,
                                    Zinc = (decimal)0.0066
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Onion",
                    UnitId = pound,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Diced",
                            DisplayTitle = "Diced Onion",
                            BasePrice = (decimal)0.0033,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0024,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.47,
                                    Protein = (decimal)0.0129,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.11,
                                    DietaryFiber = (decimal)0.02,
                                    Sugars = (decimal)0.0497,
                                    Sudium = (decimal)1.41,
                                    Iron = (decimal)0.0025,
                                    VitaminA = 0,
                                    VitaminC = (decimal)0.074,
                                    Zinc = (decimal)0.002
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Parsley",
                    UnitId = bunch,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Fresh",
                            DisplayTitle = "Fresh Parsley",
                            BasePrice = (decimal)0.02,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.006,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.23,
                                    Protein = (decimal)0.023,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0367,
                                    DietaryFiber = (decimal)0.028,
                                    Sugars = (decimal)0.0087,
                                    Sudium = (decimal)0.46,
                                    Iron = (decimal)0.0177,
                                    VitaminA = (decimal)3.37,
                                    VitaminC = (decimal)0.27,
                                    Zinc = (decimal)0.005
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Quinoa",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Tri-Colour NorQuin",
                            DisplayTitle = "Cooked Tri-Colour Quinoa",
                            BasePrice = (decimal)0.0028,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "-",
                                    AmountFactor = 48,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.89,
                                    Protein = (decimal)0.059,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.3546,
                                    DietaryFiber = (decimal)0.0236,
                                    Sugars = (decimal)0.0355,
                                    Sudium = 0,
                                    Iron = (decimal)0.0236,
                                    VitaminA = 0,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.0247
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "100% Grass Fed",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Steak",
                            DisplayTitle = "100% Grass Fed Steak",
                            BasePrice = (decimal)0.032,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0156,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 40,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.78,
                                    Protein = (decimal)0.313,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.033,
                                    Cholesterol = (decimal)0.774,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.824,
                                    Iron = (decimal)0.0132,
                                    VitaminA = (decimal)0.296,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.0148
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Spring Mix",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Clamshell",
                            DisplayTitle = "Organic Spring Mix",
                            BasePrice = (decimal)0.016,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.177,
                                    Protein = (decimal)0.023,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0472,
                                    DietaryFiber = (decimal)0.0232,
                                    Sugars = (decimal)0.0061,
                                    Sudium = (decimal)0.823,
                                    Iron = (decimal)0.019,
                                    VitaminA = (decimal)11.64,
                                    VitaminC = (decimal)0.5296,
                                    Zinc = (decimal)0.0035
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Steak",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Sirloin Cap Off",
                            DisplayTitle = "Sirloin Steak Cap Off",
                            BasePrice = (decimal)0.022,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0088,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 40,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.78,
                                    Protein = (decimal)0.313,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.033,
                                    Cholesterol = (decimal)0.774,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.824,
                                    Iron = (decimal)0.0132,
                                    VitaminA = (decimal)0.296,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.0148
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Tomato",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Chopped",
                            DisplayTitle = "Chopped Tomato",
                            BasePrice = (decimal)0.0039,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.0019,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 15,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.18,
                                    Protein = (decimal)0.0088,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0003,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.039,
                                    DietaryFiber = (decimal)0.012,
                                    Sugars = (decimal)0.0263,
                                    Sudium = (decimal)0.05,
                                    Iron = (decimal)0.0027,
                                    VitaminA = (decimal)0.42,
                                    VitaminC = (decimal)0.137,
                                    Zinc = (decimal)0.0017
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Walnut",
                    UnitId = gram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Chopped",
                            DisplayTitle = "Chopped Walnuts",
                            BasePrice = (decimal)0.0155,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.001,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)6.54,
                                    Protein = (decimal)0.15,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.061,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.14,
                                    DietaryFiber = (decimal)0.067,
                                    Sugars = (decimal)0.026,
                                    Sudium = (decimal)0.02,
                                    Iron = (decimal)0.0291,
                                    VitaminA = (decimal)0.009,
                                    VitaminC = 0,
                                    Zinc = (decimal)0.031
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Turkey",
                    UnitId = kilogram,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "Breast",
                            DisplayTitle = "Turkey Breast",
                            BasePrice = (decimal)0.022,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = gram,
                                    PriceOperator = "+",
                                    PriceFactor = 0.011,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 30,
                                    ConversionFactor = null,
                                    Energy = (decimal)1.13,
                                    Protein = (decimal)0.233,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.003,
                                    Cholesterol = (decimal)0.525,
                                    Carbohydrate = 0,
                                    DietaryFiber = 0,
                                    Sugars = 0,
                                    Sudium = (decimal)0.735,
                                    Iron = (decimal)0.009,
                                    VitaminA = (decimal)0.33,
                                    VitaminC = (decimal)0.057,
                                    Zinc = (decimal)0.0133
                                }
                            }
                        }
                    }
                },
                new Ingredient {
                    Title = "Marinate",
                    UnitId = mililiter,
                    IngredientTypes = new List<IngredientType>
                    {
                        new IngredientType
                        {
                            Title = "BBQ Chicken Type One",
                            DisplayTitle = "Marinate BBQ Chicken Type One",
                            BasePrice = (decimal)0.0038,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = mililiter,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.2692,
                                    Protein = (decimal)0.0046,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0606,
                                    DietaryFiber = 0,
                                    Sugars = (decimal)0.0522,
                                    Sudium = (decimal)7.005,
                                    Iron = (decimal)0.0036,
                                    VitaminA = 0,
                                    VitaminC = (decimal)0.013,
                                    Zinc = (decimal)4.6057
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "Turmeric Type One",
                            DisplayTitle = "Marinate Turmeric Type One",
                            BasePrice = (decimal)0.0111,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = mililiter,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.579,
                                    Protein = (decimal)0.0193,
                                    TransFat = 0,
                                    SaturatedFat = (decimal)0.0029,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.1218,
                                    DietaryFiber = (decimal)0.0311,
                                    Sugars = (decimal)0.0126,
                                    Sudium = (decimal)11.091,
                                    Iron = (decimal)0.0443,
                                    VitaminA = (decimal)0.0035,
                                    VitaminC = (decimal)0.0475,
                                    Zinc = (decimal)0.0041
                                }
                            }
                        },
                        new IngredientType
                        {
                            Title = "BBQ Steak Type One",
                            DisplayTitle = "Marinate BBQ Steak Type One",
                            BasePrice = (decimal)0.0038,
                            IngredientTypeUnits = new List<IngredientTypeUnit>
                            {
                                new IngredientTypeUnit
                                {
                                    UnitId = mililiter,
                                    PriceOperator = "+",
                                    PriceFactor = 0,
                                    IsPercent = false,
                                    AmountOperator = "+",
                                    AmountFactor = 0,
                                    ConversionFactor = null,
                                    Energy = (decimal)0.2692,
                                    Protein = (decimal)0.0046,
                                    TransFat = 0,
                                    SaturatedFat = 0,
                                    Cholesterol = 0,
                                    Carbohydrate = (decimal)0.0606,
                                    DietaryFiber = 0,
                                    Sugars = (decimal)0.052,
                                    Sudium = (decimal)7.6572,
                                    Iron = (decimal)0.0004,
                                    VitaminA = 0,
                                    VitaminC = (decimal)0.013,
                                    Zinc = 0
                                }
                            }
                        }
                    }
                }
            };

            var ingredientCounter = 1;
            var ingredientTypeCounter = 1;
            var ingredientTypeUnitCounter = 1;

            foreach (var ingredient in _ingredients)
            {
                ingredient.Id = ingredientCounter;

                foreach (var ingredientType in ingredient.IngredientTypes)
                {
                    ingredientType.Id = ingredientTypeCounter;
                    ingredientType.IngredientId = ingredientCounter;

                    foreach (var ingredientTypeUnit in ingredientType.IngredientTypeUnits)
                    {
                        ingredientTypeUnit.Id = ingredientTypeUnitCounter;
                        ingredientTypeUnit.IngredientTypeId = ingredientTypeCounter;
                        ingredientTypeUnitCounter++;
                    }

                    ingredientTypeCounter++;
                }

                ingredientCounter++;
            }

            var ingredientsTemp = JsonSerializer.Serialize(_ingredients);

            foreach (var ingredient in _ingredients)
            {
                foreach (var ingredientType in ingredient.IngredientTypes)
                {
                    _modelBuilder.Entity<IngredientTypeUnit>().HasData(
                        ingredientType.IngredientTypeUnits
                    );

                    ingredientType.IngredientTypeUnits = null;

                    _modelBuilder.Entity<IngredientType>().HasData(
                        ingredientType
                    );
                }

                ingredient.IngredientTypes = null;

                _modelBuilder.Entity<Ingredient>().HasData(
                    ingredient
                );
            }

            _ingredients = JsonSerializer.Deserialize<List<Ingredient>>(ingredientsTemp);
        }

        private void Diets()
        {
            _diets = new List<Diet>
            {
                new Diet
                {
                    Id = 1,
                    Order = 1,
                    Title = "Balance",
                    Permalink = "balance",
                    Icon = "balance-diet.svg",
                    Color = "#05897B"
                },
                new Diet
                {
                    Id = 2,
                    Order = 2,
                    Title = "Keto",
                    Permalink = "keto",
                    Icon = "keto-diet.svg",
                    Color = "#FCB316"
                },
                new Diet
                {
                    Id = 3,
                    Order = 3,
                    Title = "Veg",
                    Permalink = "veg",
                    Icon = "veggie-diet.svg",
                    Color = "#42A047"
                },
                new Diet
                {
                    Id = 4,
                    Order = 4,
                    Title = "Gluten Free",
                    Permalink = "gluten-free",
                    Icon = "gluten-free-diet.svg",
                    Color = "#F78C1E"
                },
                new Diet
                {
                    Id = 5,
                    Order = 5,
                    Title = "Ideal Protein",
                    Permalink = "ideal-protein",
                    Icon = "ideal-protein-diet.svg",
                    Color = "#F15623"
                }
                #region Other Diets
                //new Diet
                //{
                //    Id = 0,
                //    Order = 0,
                //    Title = "Dairy Free",
                //    Permalink = "dairy-free",
                //    Icon = "dairy-free-diet.svg",
                //    Color = "#00ACC1"
                //},
                //new Diet
                //{
                //    Id = 0,
                //    Order = 0,
                //    Title = "Dash",
                //    Permalink = "dash",
                //    Icon = "dash-diet.svg",
                //    Color = "#C0CA33"
                //},
                //new Diet
                //{
                //    Id = 0,
                //    Order = 0,
                //    Title = "Gainer",
                //    Permalink = "gainer",
                //    Icon = "gainer-diet.svg",
                //    Color = "#F05223"
                //},
                //new Diet
                //{
                //    Id = 0,
                //    Order = 0,
                //    Title = "Intermittent Fasting",
                //    Permalink = "intermittent-fasting",
                //    Icon = "intermittent-fasting-diet.svg",
                //    Color = "#3F4DA1"
                //},
                //new Diet
                //{
                //    Id = 0,
                //    Order = 0,
                //    Title = "Mediterranean",
                //    Permalink = "mediterranean",
                //    Icon = "mediterranean-diet.svg",
                //    Color = "#2D98D4"
                //},
                //new Diet
                //{
                //    Id = 0,
                //    Order = 9,
                //    Title = "Paleo",
                //    Permalink = "paleo",
                //    Icon = "paleo-diet.svg",
                //    Color = "#D81B61"
                //},
                //new Diet
                //{
                //    Id = 0,
                //    Order = 0,
                //    Title = "Weight Loss",
                //    Permalink = "weight-loss",
                //    Icon = "weight-loss-diet.svg",
                //    Color = "#5F449B"
                //}
                #endregion Other Diets
            };
            _modelBuilder.Entity<Diet>().HasData(
                _diets
            );
        }

        private void Tags()
        {
            _tags = new List<Tag>
            {
                new Tag
                {
                    Id = 1,
                    Title = "High Protein",
                    Permalink = "high-protein",
                    Order = 100,
                    Category = TagCategory.Nutrition
                },
                new Tag
                {
                    Id = 2,
                    Title = "IBS Friendly",
                    Permalink = "ibs-friendly",
                    Order = 100,
                    Category = TagCategory.Nutrition
                },
                new Tag
                {
                    Id = 3,
                    Title = "Non-GMO",
                    Permalink = "non-gmo",
                    Order = 100,
                    Category = TagCategory.Nutrition
                }
            };

            _modelBuilder.Entity<Tag>().HasData(
                _tags
            );
        }

        private void Foods()
        {
            var foods = new List<Food>
            {
                new Food
                {
                    Title = "House Chicken Breast",
                    ProfitMargin = 20,
                    Attachments = new List<FoodAttachment>
                    {
                        new FoodAttachment
                        {
                            FileName = "house-chicken-breast-1.jpg",
                            IsMain = true,
                            MediaType = MediaType.Image
                        },
                        new FoodAttachment
                        {
                            FileName = "house-chicken-breast-2.jpg",
                            IsMain = false,
                            MediaType = MediaType.Image
                        }
                    },
                    Recipes = new List<Recipe>
                    {
                        new Recipe
                        {
                            Title = "Regular",
                            Code = "RSP-2588198",
                            Skill = SkillLevel.Easy,
                            PackagingTime = 3,
                            Description = "",
                            MainMenu = true,
                            IsOption = true,
                            RecipeType = RecipeType.MealPrep,
                            Score = 0,
                            Price = (decimal)15.79,
                            CalculateDateTime = new DateTime(2022, 2, 27),
                            AllowNoAppleCider = false,
                            AllowNoPepper = true,
                            AllowNoSalt = true,
                            RecipeDiets = new List<RecipeDiet>
                            {
                                new RecipeDiet
                                {
                                    DietId = _diets.FirstOrDefault(p => p.Permalink == "balance").Id,
                                }
                            },
                            RecipeTags = new List<RecipeTag>
                            {
                                new RecipeTag
                                {
                                    TagId = _tags.FirstOrDefault(p => p.Permalink == "high-protein").Id
                                },
                                new RecipeTag
                                {
                                    TagId = _tags.FirstOrDefault(p => p.Permalink == "ibs-friendly").Id
                                },
                                new RecipeTag
                                {
                                    TagId = _tags.FirstOrDefault(p => p.Permalink == "non-gmo").Id
                                }
                            },
                            RecipeOverheadCosts = new List<RecipeOverheadCost>
                            {
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.5,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Dish").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.1,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Label").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.9,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Packaging Fee Personel").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)1.6,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Rent").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)1.5,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Employee").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.45,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Marketing").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.45,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Sales").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.6,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Office").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.45,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Other Cost").Id
                                }
                            },
                            RecipeIngredientTypeUnits = new List<RecipeIngredientTypeUnit>
                            {
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Steamed Broccoli", "g"),
                                    Amount = 50,
                                    Order = 3,
                                    Substitutes = new List<RecipeIngredientTypeSubstitute>
                                    {
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Green Bean", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Mushroom", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven-Roasted Asparagus", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven Roasted Zucchini", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Organic Baby Carrot", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Cauliflower", "g"),
                                            ProcessFee = (decimal)0.5
                                        }
                                    },
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = 0
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 25,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 100,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 150,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 200,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Steamed Organic Baby Carrot", "g"),
                                    Amount = 25,
                                    Order = 4,
                                    Substitutes = new List<RecipeIngredientTypeSubstitute>
                                    {
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Green Bean", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Mushroom", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven-Roasted Asparagus", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven Roasted Zucchini", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Broccoli", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Cauliflower", "g"),
                                            ProcessFee = (decimal)0.5
                                        }
                                    },
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = 0
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 50,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 75,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 100,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Black Pepper", "pinch"),
                                    Amount = 1,
                                    Order = 7
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Himalyan Pink Salt", "pinch"),
                                    Amount = 1,
                                    Order = 8
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Organic Apple Cider Vinegar", "tbsp", "Liquids"),
                                    Amount = 1,
                                    Order = 9,
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Virgin Olive Oil", "tsp", "Liquids"),
                                    Amount = 2,
                                    Order = 6,
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 3,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 4,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Oasis Classic 100% Orange Juice", "ea"),
                                    Amount = 1,
                                    Order = 100,
                                    IsAddOn = true
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Organic Banana", "ea"),
                                    Amount = 1,
                                    Order = 101,
                                    IsAddOn = true
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Organic Apple", "ea"),
                                    Amount = 1,
                                    Order = 102,
                                    IsAddOn = true
                                }
                            }
                        }
                    }
                },
                new Food
                {
                    Title = "Sirloin Steak",
                    ProfitMargin = 0,
                    Attachments = new List<FoodAttachment>
                    {
                        new FoodAttachment
                        {
                            FileName = "sirloin-steak-1.jpg",
                            IsMain = true,
                            MediaType = MediaType.Image
                        },
                        new FoodAttachment
                        {
                            FileName = "sirloin-steak-2.jpg",
                            IsMain = false,
                            MediaType = MediaType.Image
                        }
                    },
                    Recipes = new List<Recipe>
                    {
                        new Recipe
                        {
                            Title = "Regular",
                            Code = "RSP-5754031",
                            Skill = SkillLevel.Easy,
                            PackagingTime = 3,
                            Description = "",
                            MainMenu = true,
                            IsOption = true,
                            RecipeType = RecipeType.MealPrep,
                            Score = 0,
                            Price = (decimal)12.59,
                            CalculateDateTime = new DateTime(2022, 3, 24),
                            AllowNoAppleCider = false,
                            AllowNoPepper = true,
                            AllowNoSalt = true,
                            RecipeDiets = new List<RecipeDiet>
                            {
                                new RecipeDiet
                                {
                                    DietId = _diets.FirstOrDefault(p => p.Permalink == "balance").Id,
                                }
                            },
                            RecipeTags = new List<RecipeTag>
                            {
                                new RecipeTag
                                {
                                    TagId = _tags.FirstOrDefault(p => p.Permalink == "high-protein").Id
                                },
                                new RecipeTag
                                {
                                    TagId = _tags.FirstOrDefault(p => p.Permalink == "ibs-friendly").Id
                                }
                            },
                            RecipeOverheadCosts = new List<RecipeOverheadCost>
                            {
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.5,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Dish").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.1,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Label").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.9,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Packaging Fee Personel").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)1.6,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Rent").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)1.5,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Employee").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.495,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Marketing").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.495,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Sales").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.66,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Office").Id
                                },
                                new RecipeOverheadCost
                                {
                                    Amount = (decimal)0.495,
                                    OverheadCostId = _overheadCosts.FirstOrDefault(p => p.Title == "Other Cost").Id
                                }
                            },
                            RecipeIngredientTypeUnits = new List<RecipeIngredientTypeUnit>
                            {
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Steamed Broccoli", "g"),
                                    Amount = 50,
                                    Order = 3,
                                    Substitutes = new List<RecipeIngredientTypeSubstitute>
                                    {
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Green Bean", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Mushroom", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven-Roasted Asparagus", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven Roasted Zucchini", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Organic Baby Carrot", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Cauliflower", "g"),
                                            ProcessFee = (decimal)0.5
                                        }
                                    },
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = 0
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 25,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 100,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 150,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 200,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Steamed Organic Baby Carrot", "g"),
                                    Amount = 25,
                                    Order = 4,
                                    Substitutes = new List<RecipeIngredientTypeSubstitute>
                                    {
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Green Bean", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Mushroom", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven-Roasted Asparagus", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven Roasted Zucchini", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Broccoli", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Cauliflower", "g"),
                                            ProcessFee = (decimal)0.5
                                        }
                                    },
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = 0
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 50,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 75,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 100,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Oven Roasted Zucchini", "g"),
                                    Amount = 25,
                                    Order = 5,
                                    Substitutes = new List<RecipeIngredientTypeSubstitute>
                                    {
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Green Bean", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Mushroom", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Oven-Roasted Asparagus", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Broccoli", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Organic Baby Carrot", "g"),
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeSubstitute
                                        {
                                            IngredientTypeUnitId = GetIngredientByName("Steamed Cauliflower", "g"),
                                            ProcessFee = (decimal)0.5
                                        }
                                    },
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = 0
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 50,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 75,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 100,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Black Pepper", "pinch"),
                                    Amount = 1,
                                    Order = 6
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Himalyan Pink Salt", "pinch"),
                                    Amount = 1,
                                    Order = 7
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Virgin Olive Oil", "tsp", "Liquids"),
                                    Amount = 3,
                                    Order = 8,
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = (decimal)0.5
                                        },
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 6,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Organic Apple Cider Vinegar", "tbsp", "Liquids"),
                                    Amount = 1,
                                    Order = 9,
                                    OtherAmounts = new List<RecipeIngredientTypeAmount>
                                    {
                                        new RecipeIngredientTypeAmount
                                        {
                                            Amount = 0,
                                            ProcessFee = (decimal)0.5
                                        }
                                    }
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Oasis Classic 100% Orange Juice", "ea"),
                                    Amount = 1,
                                    Order = 100,
                                    IsAddOn = true
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Organic Banana", "ea"),
                                    Amount = 1,
                                    Order = 101,
                                    IsAddOn = true
                                },
                                new RecipeIngredientTypeUnit
                                {
                                    IngredientTypeUnitId = GetIngredientByName("Organic Apple", "ea"),
                                    Amount = 1,
                                    Order = 102,
                                    IsAddOn = true
                                }
                            }
                        }
                    }
                }
            };

            var foodCounter = 1;
            var attachmentCounter = 1;
            var recipeCounter = 1;
            var recipeDietCounter = 1;
            var recipeTagCounter = 1;
            var recipeOverheadCostCounter = 1;
            var recipeIngredientTypeUnitCounter = 1;
            var recipeIngredientAmountCounter = 1;
            var recipeIngredientSubstituteCounter = 1;
            var kitchenCookingDaysCounter = 1;

            foreach (var food in foods)
            {
                foreach (var attachment in food.Attachments)
                {
                    attachment.Id = attachmentCounter;
                    attachment.FoodId = foodCounter;
                    _modelBuilder.Entity<FoodAttachment>().HasData(
                        attachment
                    );

                    attachmentCounter++;
                }

                foreach (var recipe in food.Recipes)
                {
                    recipe.Id = recipeCounter;
                    recipe.FoodId = foodCounter;

                    if (recipe.RecipeDiets != null)
                    {
                        foreach (var recipeDiet in recipe.RecipeDiets)
                        {
                            recipeDiet.Id = recipeDietCounter;
                            recipeDiet.RecipeId = recipeCounter;

                            _modelBuilder.Entity<RecipeDiet>().HasData(
                                recipeDiet
                            );

                            recipeDietCounter++;
                        }
                    }

                    foreach (var recipeTag in recipe.RecipeTags)
                    {
                        recipeTag.Id = recipeTagCounter;
                        recipeTag.RecipeId = recipeCounter;
                        _modelBuilder.Entity<RecipeTag>().HasData(
                            recipeTag
                        );
                        recipeTagCounter++;
                    }

                    foreach (var recipeOverheadCost in recipe.RecipeOverheadCosts)
                    {
                        recipeOverheadCost.Id = recipeOverheadCostCounter;
                        recipeOverheadCost.RecipeId = recipeCounter;

                        _modelBuilder.Entity<RecipeOverheadCost>().HasData(
                            recipeOverheadCost
                        );
                        recipeOverheadCostCounter++;
                    }

                    foreach (var recipeIngredientTypeUnit in recipe.RecipeIngredientTypeUnits)
                    {
                        recipeIngredientTypeUnit.Id = recipeIngredientTypeUnitCounter;
                        recipeIngredientTypeUnit.RecipeId = recipeCounter;

                        if (recipeIngredientTypeUnit.OtherAmounts != null)
                        {
                            foreach (var otherAmount in recipeIngredientTypeUnit.OtherAmounts)
                            {
                                otherAmount.Id = recipeIngredientAmountCounter;
                                otherAmount.RecipeIngredientTypeUnitId = recipeIngredientTypeUnitCounter;

                                _modelBuilder.Entity<RecipeIngredientTypeAmount>().HasData(
                                    otherAmount
                                );

                                recipeIngredientAmountCounter++;
                            }
                        }

                        if (recipeIngredientTypeUnit.Substitutes != null)
                        {
                            foreach (var substitute in recipeIngredientTypeUnit.Substitutes)
                            {
                                substitute.Id = recipeIngredientSubstituteCounter;
                                substitute.RecipeIngredientTypeUnitId = recipeIngredientTypeUnitCounter;

                                _modelBuilder.Entity<RecipeIngredientTypeSubstitute>().HasData(
                                    substitute
                                );

                                recipeIngredientSubstituteCounter++;
                            }
                        }

                        recipeIngredientTypeUnit.OtherAmounts = null;
                        recipeIngredientTypeUnit.Substitutes = null;

                        _modelBuilder.Entity<RecipeIngredientTypeUnit>().HasData(
                            recipeIngredientTypeUnit
                        );

                        recipeIngredientTypeUnitCounter++;
                    }


                    recipe.RecipeDiets = null;
                    recipe.RecipeTags = null;
                    recipe.RecipeOverheadCosts = null;
                    recipe.RecipeIngredientTypeUnits = null;

                    _modelBuilder.Entity<Recipe>().HasData(
                        recipe
                    );

                    recipeCounter++;
                }

                food.Id = foodCounter;

                food.Attachments = null;
                food.Recipes = null;

                _modelBuilder.Entity<Food>().HasData(
                    food
                );

                foodCounter++;
            }
        }

        private void Combos()
        {
            _modelBuilder.Entity<Combo>().HasData(
                new List<Combo>
                {
                    new Combo
                    {
                        Id = 1,
                        Code = "CMB-3882388",
                        Title = "Barley Soup Combo",
                        Description = "It’s healthy, filling, and you can’t stop eating it!",
                        Price = (decimal)4.75
                    },
                    new Combo
                    {
                        Id = 2,
                        Code = "CMB-9186445",
                        Title = "Spring Mix Salad",
                        Description = "",
                        Price = (decimal)2.50
                    },
                    new Combo
                    {
                        Id = 3,
                        Code = "CMB-2839361",
                        Title = "Banana Protein Muffins",
                        Description = "Banana Protein Muffins are the perfect healthy breakfast or snack that fills you up!",
                        Price = (decimal)4.50
                    }
                }
            );
        }
    }
}
