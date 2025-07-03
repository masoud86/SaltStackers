using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SaltStackers.Application.Filters;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Api;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using SaltStackers.Domain.Interfaces;
using SaltStackers.Domain.Models.Nutrition;
using System.Text.Json;

namespace SaltStackers.Application.Services
{
    public class NutritionService : INutritionService
    {
        private readonly IGenericRepository<Package> _packageRepository;
        private readonly INutritionRepository _nutritionRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IUploadService _uploadService;
        private readonly ILogService _logService;
        private readonly IMapper _iMapper;

        public NutritionService(IGenericRepository<Package> packageRepository, INutritionRepository nutritionRepository,
            IOperationRepository operationRepository, ILogService logService, IUploadService uploadService)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            _iMapper = config.CreateMapper();
            _packageRepository = packageRepository;
            _nutritionRepository = nutritionRepository;
            _operationRepository = operationRepository;
            _uploadService = uploadService;
            _logService = logService;
        }

        #region Package

        public async Task<List<PackageDto>> GetPackagesAsync(PackageFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            //var model = await _nutritionRepository.GetPackagesAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);
            var model = await _packageRepository.GetAsync(start: filter.Start, pageSize: filter.PageSize, sortBy: filter.Sort, direction: filter.Direction,
                predicate: predicate, include: i => i.Include(p => p.Attachments));

            return _iMapper.Map<List<Package>, List<PackageDto>>(model);
        }

        public async Task<Packages> GetPackagesModelAsync(PackageFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            //var recordTotal = await _nutritionRepository.GetPackagesCountAsync();
            var recordTotal = await _packageRepository.CountAsync();

            //var recordsFilters = await _nutritionRepository.GetPackagesCountAsync(predicate);
            var recordsFilters = await _packageRepository.CountAsync(predicate);

            return new Packages
            {
                Items = await GetPackagesAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page
            };
        }

        public async Task<int> CountPackagesAsyc(PackageFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);
            //return await _nutritionRepository.GetPackagesCountAsync(predicate);
            return await _packageRepository.CountAsync(predicate);
        }

        public async Task<(bool succeeded, int id)> CreatePackageAsync(string title)
        {
            var package = new Package
            {
                Code = "PKG-".GenerateCode(),
                Title = title,
                Price = (decimal)0.00,
                IsActive = false
            };
            var id = await _nutritionRepository.CreatePackageAsync(package);
            return (succeeded: id > 0, id);
        }

        public async Task<PackageDto?> GetPackageAsync(int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            //var package = await _nutritionRepository.GetPackageAsync(id.Value);
            var package = await _packageRepository.FirstAsync(p => p.Id == id.Value,
                include: i => i
                .Include(p => p.Groups)
                .ThenInclude(p => p.Items)
                .ThenInclude(p => p.Recipe)
                .ThenInclude(p => p.Food));
            return _iMapper.Map<PackageDto>(package);
        }

        public async Task<PackageDetails?> GetPackageAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }
            //var package = await _nutritionRepository.GetPackageAsync(code);
            var package = await _packageRepository.FirstAsync(p => p.Code == code,
                include: i => i
                .Include(p => p.Groups)
                .ThenInclude(p => p.Items)
                .ThenInclude(p => p.Recipe)
                .ThenInclude(p => p.Food)
                .ThenInclude(p => p.Attachments));
            var packageDetails = _iMapper.Map<PackageDetails>(package);
            if (packageDetails != null && (packageDetails.Attachments == null || !packageDetails.Attachments.Any()))
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

                var root = builder.Build();
                var publicUrl = root.GetSection("PublicUrl").Get<string>();
                packageDetails.Attachments =
                [
                    new() {
                        FileName = "default-small.png",
                        Url = $"{publicUrl}/package/default-small.png",
                        IsMain = true
                    }
                ];
            }
            return packageDetails;
        }

        public async Task<ServiceResult> EditPackageAsync(EditPackage model)
        {
            //var package = await _nutritionRepository.GetPackageAsync(model.Id);
            var package = await _packageRepository.FirstAsync(p => p.Id == model.Id);
            if (package != null)
            {
                package.Title = model.Title;
                package.Subtitle = model.Subtitle;
                package.Description = model.Description;
                package.Price = model.Price;
                package.IsActive = model.IsActive;
                var succeeded = await _nutritionRepository.EditPackageAsync(package);
                if (succeeded)
                {
                    await _logService.AddUserLogAsync(model.LogUserId, "Package", "UpdatePackage", model.Title,
                        package.Id.ToString(), model, model.LogInfo);
                    return new ServiceResult(true);
                }
            }
            return new ServiceResult(false);
        }

        public async Task<ServiceResult> AddPackageGroupAsync(CreateGroup model)
        {
            var id = await _nutritionRepository.AddPackageGroupAsync(new PackageGroup
            {
                PackageId = model.PackageId,
                Title = model.Title
            });
            return new ServiceResult(id > 0);
        }

        public async Task<ServiceResult> EditPackageGroupAsync(EditGroup model)
        {
            return new ServiceResult(await _nutritionRepository.EditPackageGroupAsync(new PackageGroup
            {
                Id = model.Id,
                Title = model.Title
            }));
        }

        public async Task<ServiceResult> AddPackageGroupItemAsync(PackageGroupItemDto model)
        {
            var id = await _nutritionRepository.AddPackageGroupItemAsync(new PackageGroupItem
            {
                GroupId = model.GroupId,
                RecipeId = model.RecipeId,
                Label = model.Label
            });
            return new ServiceResult(id > 0);
        }

        public async Task<ServiceResult> EditPackageGroupItemAsync(EditGroupItem model)
        {
            return new ServiceResult(await _nutritionRepository.EditPackageGroupItemAsync(new PackageGroupItem
            {
                Id = model.Id,
                Label = model.Label
            }));
        }

        public async Task<ServiceResult> DeletePackageGroupItemAsync(int id)
        {
            var item = await _nutritionRepository.GetPackageGroupItemAsync(id);
            if (item == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Item not found" } });
            }
            var succeeded = await _nutritionRepository.DeletePackageGroupItemAsync(item);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        #endregion Package

        #region Food

        public async Task<Tuple<ServiceResult, int>> CreateFoodAsync(FoodDto model)
        {
            var sameFood = await _nutritionRepository.GetFoodByTitleAsync(model.Title.ToLower());
            if (sameFood != null)
            {
                return new Tuple<ServiceResult, int>(new ServiceResult(false, new List<ServiceError>
                {
                    new ServiceError { Code = "Duplicate Title", Description = "Title is exist" }
                }), 0);
            }
            var food = _iMapper.Map<Food>(model);
            var id = await _nutritionRepository.CreateFoodAsync(food);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "Food", "CreateFood", model.Title,
                    id.ToString(), model, model.LogInfo);
            }

            return new Tuple<ServiceResult, int>(new ServiceResult(succeeded), id);
        }

        public async Task<List<FoodDto>> GetFoodsAsync(FoodFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetFoodsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Food>, List<FoodDto>>(model);
        }

        public async Task<Foods> GetFoodsModelAsync(FoodFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetFoodsCountAsync();

            var recordsFilters = await _nutritionRepository.GetFoodsCountAsync(predicate);

            return new Foods
            {
                Items = await GetFoodsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page
            };
        }

        public async Task<FoodDto> GetFoodAsync(int? id)
        {
            var food = await _nutritionRepository.GetFoodAsync(id);
            return _iMapper.Map<FoodDto>(food);
        }

        public async Task<DeleteFood> GetFoodForDeleteAsync(int? id)
        {
            var food = await _nutritionRepository.GetFoodAsync(id);
            return _iMapper.Map<DeleteFood>(food);
        }

        public async Task<ServiceResult> UpdateFoodAsync(FoodDto model)
        {
            var sameFood = await _nutritionRepository.GetFoodByTitleAsync(model.Title.ToLower());
            if (sameFood != null && sameFood.Id != model.Id)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Duplicate Title", Description = "Title is exist" } });
            }
            var food = await _nutritionRepository.GetFoodAsync(model.Id);
            food.Title = model.Title;
            food.ProfitMargin = model.ProfitMargin;
            var succeeded = await _nutritionRepository.EditFoodAsync(food);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "Food", "UpdateFood", model.Title,
                    food.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteFoodAsync(int? id)
        {
            //TODO: check relations
            var food = await _nutritionRepository.GetFoodAsync(id);
            if (food == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Food not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteFoodAsync(food);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        public async Task<int> CountFoodsAsyc(FoodFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);
            return await _nutritionRepository.GetFoodsCountAsync(predicate);
        }

        public async Task<bool> TransferFoodAsync(TransferFood model)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(model.RecipeId);
            if (recipe != null)
            {
                recipe.FoodId = model.FoodId;
                return await _nutritionRepository.UpdateRecipeAsync(recipe);
            }
            return false;
        }

        #endregion Food

        #region Recipe

        public async Task<MenuItems> GetMenuItemsAsync(MenuItemFilters filter)
        {
            var result = new MenuItems { Page = filter.Page };

            if (!string.IsNullOrWhiteSpace(filter.Diet) && string.Compare(filter.Diet, "package", true) == 0)
            {
                var packages = await _nutritionRepository.GetPackagesAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction,
                    include: p => p
                        .Include(q => q.Attachments.Where(r => r.IsMain && r.MediaType == MediaType.Image)),
                    NutritionFilter.ToExpression(new PackageFilters
                    {
                        Query = filter.Query
                    }));
                result.Items = _iMapper.Map<List<MenuItem>>(packages);
            }
            else
            {
                var recipes = await _nutritionRepository.GetRecipesAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction,
                    include: p => p
                        .Include(q => q.Food)
                        .ThenInclude(q => q.Attachments.Where(r => r.IsMain && r.MediaType == MediaType.Image))
                        .Include(q => q.RecipeTags.Where(r => r.Tag.Category == TagCategory.Nutrition).Take(2))
                        .ThenInclude(q => q.Tag)
                        .Include(q => q.RecipeDiets)
                        .Include(q => q.RecipeOwners),
                    NutritionFilter.ToExpression(filter));

                result.Items = _iMapper.Map<List<MenuItem>>(recipes);
            }

            return result;
        }

        public async Task<ServiceResult> CreateRecipeAsync(RecipeDto model)
        {
            var recipe = _iMapper.Map<Recipe>(model);
            var foodRecipies = await _nutritionRepository.GetFoodRecipesAsync(model.FoodId);
            if (!foodRecipies.Any() && model.RecipeType == RecipeType.MealPrep)
            {
                recipe.MainMenu = true;
            }
            recipe.Code = "RSP-".GenerateCode();
            if (recipe.PersonalChefId == "null")
            {
                recipe.PersonalChefId = null;
            }
            var id = await _nutritionRepository.CreateRecipeAsync(recipe);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "Recipe", "CreateRecipe", model.Title,
                    id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<List<RecipeDto>> GetRecipesAsync(RecipeFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetRecipesAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction,
                include: p => p
                    .Include(q => q.Food)
                    .Include(q => q.RecipeOwners)
                    .ThenInclude(q => q.User)
                    .Include(q => q.RecipeDiets)
                    .ThenInclude(q => q.Diet)
                    .Include(q => q.PersonalChef),
                predicate);

            var recipes = _iMapper.Map<List<RecipeDto>>(model);

            return recipes;
        }

        public async Task<Recipes> GetRecipesModelAsync(RecipeFilters filter)
        {
            var predicateTotal = NutritionFilter.ToExpression(filter.FoodId);
            var predicateFilter = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetRecipesCountAsync(predicateTotal);

            var recordsFilters = await _nutritionRepository.GetRecipesCountAsync(predicateFilter);

            return new Recipes
            {
                Items = await GetRecipesAsync(filter),
                Food = filter.FoodId != 0 ? await GetFoodAsync(filter.FoodId) : null,
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                FoodId = filter.FoodId
            };
        }

        public async Task<RecipeDto> GetRecipeAsync(int? id)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(id,
                include: p => p
                    .Include(q => q.Food)
                    .ThenInclude(q => q.Attachments)
                    .Include(q => q.PersonalChef));
            return _iMapper.Map<RecipeDto>(recipe);
        }

        public async Task<DeleteRecipe> GetRecipeForDeleteAsync(int? id)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(id);
            return _iMapper.Map<DeleteRecipe>(recipe);
        }

        public async Task<ServiceResult> UpdateRecipeAsync(RecipeDto model)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(model.Id);
            recipe.Title = model.Title;
            recipe.Description = model.Description;
            recipe.RecipeDetails = model.RecipeDetails;
            recipe.RecipeType = model.RecipeType;
            recipe.RecipeSize = model.RecipeSize;
            recipe.Skill = model.Skill;
            recipe.PackagingTime = model.PackagingTime;
            recipe.Priority = model.Priority;
            recipe.IsOption = model.IsOption;
            recipe.HeatingInstruction = model.HeatingInstruction;
            recipe.IsRoutine = model.IsRoutine;
            recipe.Orderable = true;
            recipe.IsActive = model.IsActive;
            recipe.IsNew = model.IsNew;
            recipe.IsTwoStepCooking = model.IsTwoStepCooking;
            recipe.MainMenu = model.MainMenu;
            recipe.DefaultInCategory = model.DefaultInCategory;
            recipe.PersonalChefId = model.PersonalChefId;

            var succeeded = await _nutritionRepository.EditRecipeAsync(recipe);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "Recipe", "UpdateRecipe", model.Title,
                    recipe.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> CloneRecipeAsync(RecipeDto model)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(model.Id,
                include: p => p
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.IngredientTypeUnit)
                    .ThenInclude(q => q.IngredientType)
                    .ThenInclude(q => q.Ingredient)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.IngredientTypeUnit.Unit)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.Substitutes)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.OtherAmounts)
                    .Include(q => q.RecipeOverheadCosts)
                    .ThenInclude(q => q.OverheadCost)
                    .Include(q => q.RecipeDiets)
                    .Include(q => q.RecipeTags)
                    .Include(q => q.Food)
                    .Include(q => q.Customizations));
            if (recipe == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Recipe not exist" } });
            }
            var cloneRecipe = new Recipe
            {
                FoodId = model.FoodId,
                Description = model.Description,
                PackagingTime = model.PackagingTime,
                RecipeType = model.RecipeType,
                Skill = model.Skill,
                Title = model.Title,
                Price = recipe.Price,
                Score = 0,
                CalculateDateTime = DateTime.UtcNow,
                AllowNoSalt = recipe.AllowNoSalt,
                AllowNoPepper = recipe.AllowNoPepper,
                AllowNoAppleCider = recipe.AllowNoAppleCider,
                AllowNoSalmonSkin = recipe.AllowNoSalmonSkin,
                Code = "RSP-".GenerateCode(),
                RecipeOverheadCosts = recipe.RecipeOverheadCosts?.Select(p => new RecipeOverheadCost
                {
                    Amount = p.Amount,
                    OverheadCostId = p.OverheadCostId
                }).ToList(),
                RecipeIngredientTypeUnits = recipe.RecipeIngredientTypeUnits?.Select(p => new RecipeIngredientTypeUnit
                {
                    Amount = p.Amount,
                    IsAddOn = p.IsAddOn,
                    Order = p.Order,
                    IngredientTypeUnitId = p.IngredientTypeUnitId,
                    Substitutes = p.Substitutes?.Select(q => new RecipeIngredientTypeSubstitute
                    {
                        IngredientTypeUnitId = q.IngredientTypeUnitId,
                        ProcessFee = q.ProcessFee
                    }).ToList(),
                    OtherAmounts = p.OtherAmounts?.Select(q => new RecipeIngredientTypeAmount
                    {
                        Amount = q.Amount,
                        ProcessFee = q.ProcessFee
                    }).ToList()
                }).ToList(),
                RecipeDiets = recipe.RecipeDiets?.Select(p => new RecipeDiet
                {
                    DietId = p.DietId
                }).ToList(),
                RecipeTags = recipe.RecipeTags?.Select(p => new Domain.Models.Nutrition.RecipeTag
                {
                    TagId = p.TagId
                }).ToList(),
                Customizations = recipe.Customizations?.Where(q => q.IsDefault)
                .Select(q => new Customization
                {
                    Price = q.Price,
                    IsDefault = q.IsDefault,
                    Iron = q.Iron,
                    Protein = q.Protein,
                    Carbohydrate = q.Carbohydrate,
                    Cholesterol = q.Cholesterol,
                    DietaryFiber = q.DietaryFiber,
                    Energy = q.Energy,
                    SaturatedFat = q.SaturatedFat,
                    Sudium = q.Sudium,
                    Sugars = q.Sugars,
                    TotalFat = q.TotalFat,
                    TransFat = q.TransFat,
                    VitaminA = q.VitaminA,
                    VitaminC = q.VitaminC,
                    Zinc = q.Zinc
                }).ToList()
            };

            var succeeded = await _nutritionRepository.CreateRecipeAsync(cloneRecipe);

            return new ServiceResult(true);
        }

        public async Task<ServiceResult> DeleteRecipeAsync(int? id)
        {
            if (!id.HasValue)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Recipe not exist" } });
            }

            var recipe = await _nutritionRepository.GetRecipeAsync(id);
            if (recipe == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Recipe not exist" } });
            }

            var succeeded = await _nutritionRepository.DeleteRecipeAsync(id.Value);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        public async Task<ServiceResult> SetDefaultRecipeAsync(int? id)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(id);
            if (recipe == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Recipe not exist" } });
            }
            var foodRecipies = await _nutritionRepository.GetFoodRecipesAsync(recipe.FoodId);
            foreach (var item in foodRecipies.Where(p => p.RecipeType == recipe.RecipeType && p.MainMenu))
            {
                item.MainMenu = false;
                await _nutritionRepository.EditRecipeAsync(item);
            }
            recipe.MainMenu = true;
            var succeeded = await _nutritionRepository.EditRecipeAsync(recipe);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        public async Task<RecipeDetails> GetRecipeDetailsAsync(int? id)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(id,
                include: p => p.Include(q => q.Food));
            if (recipe == null)
            {
                return null;
            }

            var model = new RecipeDetails
            {
                RecipeId = recipe.Id,
                FoodId = recipe.FoodId,
                Title = string.IsNullOrWhiteSpace(recipe.Title)
                    ? recipe.Food?.Title
                    : $"{recipe.Food.Title} ({recipe.Title})",
                Images = _uploadService.GetFilesFromDirectory("food", recipe.FoodId.ToString()),
                MainSize = recipe.IsOption,
                IsDefault = recipe.MainMenu
            };

            return model;
        }

        public async Task<int> CountRecipesAsyc(RecipeFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);
            return await _nutritionRepository.GetRecipesCountAsync(predicate);
        }

        public decimal? CalculateNutritionFact(List<RecipeIngredientTypeUnitDto> ingredients, string item)
        {
            var ingredient = ingredients.Sum(p => p.Amount * (decimal)p.IngredientTypeUnit.GetType().GetProperty(item).GetValue(p.IngredientTypeUnit));

            return Math.Round(ingredient, 0);
        }

        public decimal? CalculateNutritionFactMain(List<RecipeIngredientTypeUnit> ingredients, string item)
        {
            var ingredient = ingredients.Sum(p => p.Amount * (decimal)p.IngredientTypeUnit.GetType().GetProperty(item).GetValue(p.IngredientTypeUnit));

            return Math.Round(ingredient, 0);
        }

        public async Task<List<NutritionFact>> GetNutritionFactsAsync(int recipeId)
        {
            var customization = await _nutritionRepository.GetDefaultCustomizationAsync(recipeId);
            if (customization != null)
            {
                return customization.CalculateNutritionFacts();
            }

            var recipeIngredients = await _nutritionRepository.GetIngredientTypeUnitsByRecipeAsync(recipeId);
            recipeIngredients = recipeIngredients.Where(p => !p.IsAddOn).ToList();
            return RecipeHelper.CalculateNutritionFacts(recipeIngredients);
        }

        public async Task<List<RecipeIngredientTypeAmountDto>> GetIngredientOtherAmountsAsync(int recipeIngredientTypeId)
        {
            var otherAmounts = await _nutritionRepository.GetIngredientOtherAmountsAsync(recipeIngredientTypeId);
            return _iMapper.Map<List<RecipeIngredientTypeAmountDto>>(otherAmounts);
        }

        public async Task<ServiceResult> CreateRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmountDto model)
        {
            var recipeIngredientTypeAmount = _iMapper.Map<RecipeIngredientTypeAmount>(model);
            var id = await _nutritionRepository.CreateRecipeIngredientTypeAmountAsync(recipeIngredientTypeAmount);
            var succeeded = id > 0;

            if (succeeded)
            {
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> EditRecipeIngredientTypeAmountAsync(RecipeIngredientTypeAmountDto model)
        {
            var amount = await _nutritionRepository.GetRecipeIngredientTypeAmountAsync(model.Id);
            amount.Amount = model.Amount;
            amount.ProcessFee = model.ProcessFee;

            var succeeded = await _nutritionRepository.EditRecipeIngredientTypeAmountAsync(amount);
            if (succeeded)
            {
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteRecipeIngredientTypeAmountAsync(int id)
        {
            var amount = await _nutritionRepository.GetRecipeIngredientTypeAmountAsync(id);
            if (amount == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "IngredientType not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteRecipeIngredientTypeAmountAsync(amount);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        public async Task<List<RecipeIngredientTypeSubstituteDto>> GetIngredientSubstitutesAsync(int recipeIngredientTypeId)
        {
            var substitutes = await _nutritionRepository.GetIngredientSubstitutesAsync(recipeIngredientTypeId);
            return _iMapper.Map<List<RecipeIngredientTypeSubstituteDto>>(substitutes);
        }

        public async Task<ServiceResult> CreateRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstituteDto model)
        {
            var recipeIngredientTypeSubstitute = _iMapper.Map<RecipeIngredientTypeSubstitute>(model);
            var id = await _nutritionRepository.CreateRecipeIngredientTypeSubstituteAsync(recipeIngredientTypeSubstitute);
            var succeeded = id > 0;

            if (succeeded)
            {
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> EditRecipeIngredientTypeSubstituteAsync(RecipeIngredientTypeSubstituteDto model)
        {
            var amount = await _nutritionRepository.GetRecipeIngredientTypeSubstituteAsync(model.Id);
            amount.RecipeIngredientTypeUnitId = model.RecipeIngredientTypeUnitId;
            amount.ProcessFee = model.ProcessFee;

            var succeeded = await _nutritionRepository.EditRecipeIngredientTypeSubstituteAsync(amount);
            if (succeeded)
            {
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteRecipeIngredientTypeSubstituteAsync(int id)
        {
            var substitute = await _nutritionRepository.GetRecipeIngredientTypeSubstituteAsync(id);
            if (substitute == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Substitute not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteRecipeIngredientTypeSubstituteAsync(substitute);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        public async Task<List<OtherSize>> GetFoodSizesAsync(int foodId, int? recipeId = null)
        {
            var filtered = new List<OtherSize>();
            var recipes = await _nutritionRepository.GetRecipesByFoodIdAsync(foodId);

            if (recipeId.HasValue)
            {
                var currentRecipe = recipes.FirstOrDefault(p => p.Id == recipeId);

                if (currentRecipe != null)
                {
                    foreach (var recipe in recipes.Where(p => p.IsActive && p.IsOption))
                    {
                        if (recipe.Id == currentRecipe.Id)
                        {
                            var mapped = _iMapper.Map<OtherSize>(recipe);
                            mapped.Selected = true;
                            filtered.Add(mapped);
                        }
                        else if (recipe.RecipeOwners == null || !recipe.RecipeOwners.Any())
                        {
                            var mapped = _iMapper.Map<OtherSize>(recipe);
                            filtered.Add(mapped);
                        }
                    }
                }
            }
            else
            {
                filtered = _iMapper.Map<List<OtherSize>>(recipes);
            }

            return filtered.OrderByDescending(p => p.Selected).ToList();
        }

        public async Task<ItemDetails> GetRecipeDetailsApi(string code, int kitchenId)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(code.ToLower().Trim(),
                include: p => p
                    .Include(q => q.PersonalChef)
                    .Include(q => q.Food)
                    .ThenInclude(q => q.Attachments.Where(q => !q.IsMain)));

            if (recipe == null || !recipe.IsActive)
            {
                return new ItemDetails();
            }

            var fullRecipeIngredients = await _nutritionRepository.GetIngredientTypeUnitsByRecipeAsync(recipe.Id);

            var recipeIngredients = fullRecipeIngredients
                .Where(p => !p.IsAddOn).ToList();

            var ingredients = recipeIngredients
                    .Select(p => new IngredientSort
                    {
                        Order = p.Order,
                        Title = p.IngredientTypeUnit.IngredientType.DisplayTitle,
                        Description = p.IngredientTypeUnit.IngredientType.MixDescription
                    })
                    .ToList();

            var nutritionFacts = new List<NutritionFact>();

            nutritionFacts = RecipeHelper.CalculateNutritionFacts(recipeIngredients);

            var sizes = await GetFoodSizesAsync(recipe.FoodId, recipe.Id);

            var diets = await _nutritionRepository.GetDietsByRecipeAsync(recipe.Id);
            var tags = await _nutritionRepository.GetTagsByRecipeAsync(recipe.Id);

            var combos = await _nutritionRepository.GetAllCombos();

            var customizeItems = new List<CustomizeItem>();
            customizeItems.AddRange(recipeIngredients
                    .Where(p => p.Substitutes.Any() || p.OtherAmounts.Any())
                    .Select(p => new CustomizeItem
                    {
                        Id = p.Id,
                        Title = p.IngredientTypeUnit.IngredientType.DisplayTitle,
                        Order = p.Order,
                        DefaultSize = p.Amount,
                        Unit = p.IngredientTypeUnit.Unit.Sign,
                        Sizes = p.OtherAmounts.Select(q => q.Amount)
                            .Concat(new List<decimal> { p.Amount })
                            .OrderBy(p => p).ToList(),
                        Substitues = p.Substitutes.Select(q => new Substitue
                        {
                            Id = q.IngredientTypeUnitId,
                            Title = q.IngredientTypeUnit.IngredientType.DisplayTitle
                        })
                        .Concat(new List<Substitue> {
                            new Substitue
                            {
                                Id = p.IngredientTypeUnitId,
                                Title = p.IngredientTypeUnit.IngredientType.DisplayTitle,
                                IsDefault = true
                            }
                        })
                        .OrderByDescending(p => p.IsDefault)
                        .ToList()
                    }).ToList());

            var flags = new List<Flag>();
            if (recipe.AllowNoSalt)
            {
                flags.Add(new Flag("NoSalt", "No Salt Added", false));
            }
            if (recipe.AllowNoPepper)
            {
                flags.Add(new Flag("NoPepper", "No Pepper Added", false));
            }
            if (recipe.AllowNoAppleCider)
            {
                flags.Add(new Flag("NoAppleCider", "No Apple Cider", false));
            }
            if (recipe.AllowNoSalmonSkin)
            {
                flags.Add(new Flag("NoSalmonSkin", "No Salmon Skin", false));
            }

            var customize = new Customize
            {
                Ingredients = customizeItems.OrderBy(p => p.Order).ToList(),
                AddOns = fullRecipeIngredients.Where(p => p.IsAddOn)
                    .OrderBy(p => p.Order)
                    .Select(p => new CustomizeItem
                    {
                        Id = p.Id,
                        Title = p.IngredientTypeUnit.IngredientType.DisplayTitle,
                        DefaultSize = p.Amount,
                        Unit = p.IngredientTypeUnit.Unit.Sign,
                        Sizes = p.OtherAmounts.Select(q => q.Amount)
                            .Concat(new List<decimal> { p.Amount })
                            .OrderBy(p => p).ToList()
                    }).ToList(),
                Converts = _iMapper.Map<List<RecipeConvert>>(await _nutritionRepository.GetRecipeConvertsAsync(recipe.Id)),
                Flags = flags,
                Combos = _iMapper.Map<List<ComboApi>>(combos)
            };

            if (recipe.Food.Attachments == null || !recipe.Food.Attachments.Any())
            {
                recipe.Food.Attachments = new List<FoodAttachment>
                {
                    new FoodAttachment
                    {
                        Id = 0,
                        FileName = "default-large.jpg",
                        FoodId = recipe.FoodId,
                        IsMain = true,
                        MediaType = MediaType.Image,
                        UploadDateTime = DateTime.Now
                    }
                };
            }

            var kitchen = await _operationRepository.GetKitchenAsync(kitchenId);
            var by = recipe.PersonalChef == null
                    ? kitchen.Title
                    : $"innuchef {recipe.PersonalChef.Name}";


            return new ItemDetails
            {
                Id = recipe.Id,
                Code = recipe.Code,
                Title = recipe.Food.Title,
                Type = recipe.Title,
                Orderable = recipe.Orderable,
                DefaultPrice = recipe.Price,
                PayablePrice = recipe.Price,
                By = by,
                Attachments = _iMapper.Map<List<FoodAttachmentApi>>(recipe.Food.Attachments),
                Ingredients = ingredients.OrderBy(p => p.Order)
                    .Select(p => p.Title + (string.IsNullOrEmpty(p.Description) ? "" : $" ({p.Description})"))
                    .ToList(),
                NutritionFacts = nutritionFacts,
                Sizes = sizes,
                Diets = _iMapper.Map<List<DietApi>>(diets),
                Tags = _iMapper.Map<List<TagApi>>(tags),
                Customize = customize
            };
        }

        public async Task<Tuple<Recipe, decimal, string>> CustomizeRecipeAsync(string code, RecipeChanges changes)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(code.Trim().ToLower(),
                include: p => p
                    .Include(q => q.Food)
                    .Include(q => q.RecipeOverheadCosts)
                    .ThenInclude(q => q.OverheadCost)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.IngredientTypeUnit)
                    .ThenInclude(q => q.Unit)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.IngredientTypeUnit)
                    .ThenInclude(q => q.IngredientType)
                    .ThenInclude(q => q.Ingredient)
                    .ThenInclude(q => q.Unit)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.IngredientTypeUnit)
                    .ThenInclude(q => q.IngredientType)
                    .ThenInclude(q => q.AllergenAlerts)
                    );

            decimal processFee = 0;
            var changeSizeCount = 0;
            var substituteCount = 0;
            var addOnCount = 0;
            var comboCount = 0;
            var changeDescription = new List<string>();

            if (changes != null)
            {
                if (changes.IngredientChanges != null)
                {
                    foreach (var item in changes.IngredientChanges)
                    {
                        var current = recipe.RecipeIngredientTypeUnits.FirstOrDefault(p => p.Id == item.Id);
                        if (current != null && current.Amount != (decimal)item.Size)
                        {
                            var otherAmount = await _nutritionRepository
                                .GetRecipeIngredientTypeAmountAsync(item.Id, (decimal)item.Size);
                            if (otherAmount != null)
                            {
                                processFee += otherAmount.ProcessFee;
                                current.Amount = (decimal)item.Size;
                                changeSizeCount++;
                            }
                        }

                        if (item.SubstituteId != null && current.IngredientTypeUnitId != item.SubstituteId.Value)
                        {
                            var substitue = await _nutritionRepository
                                .GetRecipeIngredientTypeSubstituteAsync(item.Id, item.SubstituteId.Value);
                            if (substitue != null)
                            {
                                processFee += substitue.ProcessFee;
                                current.IngredientTypeUnit = substitue.IngredientTypeUnit;
                                current.IngredientTypeUnitId = substitue.IngredientTypeUnitId;
                                substituteCount++;
                            }
                        }
                    }
                }

                if (changes.RecipeAddOns != null)
                {
                    foreach (var item in changes.RecipeAddOns)
                    {
                        var current = recipe.RecipeIngredientTypeUnits.FirstOrDefault(p => p.Id == item.Id);
                        if (current != null)
                        {
                            current.IsAddOn = false;
                            current.Amount = (decimal)item.Size;
                            addOnCount++;
                        }
                    }
                }

                if (changes.Combos != null)
                {
                    foreach (var item in changes.Combos)
                    {
                        comboCount++;
                    }
                }

                if (changeSizeCount > 0)
                {
                    changeDescription.Add(changeSizeCount == 1
                        ? "1 size changed"
                        : $"{changeSizeCount} sizes changed");
                }

                if (substituteCount > 0)
                {
                    changeDescription.Add(substituteCount == 1
                        ? "1 substitute"
                        : $"{substituteCount} substitutes");
                }

                if (addOnCount > 0)
                {
                    changeDescription.Add(addOnCount == 1
                        ? "1 add-on"
                        : $"{addOnCount} add-ons");
                }

                if (comboCount > 0)
                {
                    changeDescription.Add(comboCount == 1
                        ? "1 combo"
                        : $"{addOnCount} combos");
                }
            }

            return new Tuple<Recipe, decimal, string>(recipe, processFee, string.Join(" - ", changeDescription));
        }

        public async Task<RecipeVariables> CalculateRecipeAsync(string code, RecipeChanges? changes, bool includeNutritionFacts = false)
        {
            var customize = await CustomizeRecipeAsync(code, changes);
            var recipe = customize.Item1;
            var processFee = customize.Item2;
            var changeDescription = customize.Item3;

            var ingredients = recipe.RecipeIngredientTypeUnits.Where(p => !p.IsAddOn).ToList();

            var nutritionFacts = new List<NutritionFact>();

            if (includeNutritionFacts)
            {
                nutritionFacts = RecipeHelper.CalculateNutritionFacts(ingredients);
            }

            var recipePrice = recipe.CalculatePrice();

            decimal combosPrice = 0;
            if (changes != null && changes.Combos != null && changes.Combos.Any())
            {
                combosPrice = await _nutritionRepository.GetCombosPrice(changes.Combos);
            }

            return new RecipeVariables
            {
                NutritionFacts = nutritionFacts,
                Price = Math.Round(recipePrice + processFee + combosPrice, 2),
                ChangeDescription = changeDescription,
                Changes = await AnalyzeIngredientChanges(changes)
            };
        }

        public async Task<List<RecipeHistory>> GetRecipeHistoriesAsync(string code, string userId)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(code,
                include: p => p
                    .Include(q => q.Food)
                    .ThenInclude(q => q.Attachments.Where(q => !q.IsMain)));
            var customizations = await _nutritionRepository.GetRecipeUserCustomizationsAsync(recipe.Id, userId);
            var result = new List<RecipeHistory>();
            foreach (var customization in customizations)
            {
                result.Add(new RecipeHistory
                {
                    Price = customization.Price,
                    NutritionFacts = customization.CalculateNutritionFacts(),
                    RecipeChanges = JsonSerializer.Deserialize<RecipeChanges>(customization.Changes)
                });
            }
            return result;
        }

        public async Task<ServiceResult> ChangeOrderAsync(UpdateOrder model)
        {
            await _nutritionRepository.UpdateRecipeIngredientOrderAsync(model.Id, model.Order);
            return new ServiceResult(true);
        }

        public async Task<List<IngredientChangeAnalyzed>> AnalyzeIngredientChanges(RecipeChanges? changes)
        {
            var result = new List<IngredientChangeAnalyzed>();
            if (changes == null)
            {
                return result;
            }

            if (changes.IngredientChanges != null && changes.IngredientChanges.Any())
            {
                foreach (var change in changes.IngredientChanges)
                {
                    var recipeIngredient = await _nutritionRepository.GetRecipeIngredientTypeUnitAsync(change.Id);
                    if (recipeIngredient != null)
                    {
                        var title = recipeIngredient.IngredientTypeUnit.IngredientType.DisplayTitle;
                        var substituteTitle = "";
                        if (change.SubstituteId.HasValue && recipeIngredient.IngredientTypeUnitId != change.SubstituteId)
                        {
                            substituteTitle = recipeIngredient.Substitutes.FirstOrDefault(p => p.IngredientTypeUnitId == change.SubstituteId)?.IngredientTypeUnit?.IngredientType?.DisplayTitle;
                        }
                        else if (change.SubstituteId.HasValue)
                        {
                            substituteTitle = title;
                        }
                        var unit = recipeIngredient.IngredientTypeUnit.Unit.Sign;
                        var changeType = RecipeHelper.GetRecipeChangeType(recipeIngredient.Amount, change.Size, change.SubstituteId);

                        result.Add(new IngredientChangeAnalyzed
                        {
                            Id = change.Id,
                            Title = title,
                            SubstituteId = change.SubstituteId,
                            SubstituteTitle = substituteTitle,
                            Size = change.Size,
                            DefaultSize = recipeIngredient.Amount,
                            Unit = unit,
                            ChangeType = changeType,
                            Description = RecipeHelper.AnalyzeDescription(changeType, title, substituteTitle, change.Size, unit)
                        });
                    }
                }
            }

            if (changes.RecipeAddOns != null && changes.RecipeAddOns.Any())
            {
                foreach (var addOn in changes.RecipeAddOns)
                {
                    var recipeIngredient = await _nutritionRepository.GetRecipeIngredientTypeUnitAsync(addOn.Id);
                    if (recipeIngredient != null)
                    {
                        var title = recipeIngredient.IngredientTypeUnit.IngredientType.DisplayTitle;
                        var unit = recipeIngredient.IngredientTypeUnit.Unit.Sign;
                        result.Add(new IngredientChangeAnalyzed
                        {
                            Id = addOn.Id,
                            Title = title,
                            Size = addOn.Size,
                            Unit = unit,
                            ChangeType = RecipeChangeType.AddIngredient,
                            Description = RecipeHelper.AnalyzeDescription(RecipeChangeType.AddIngredient, title, "", addOn.Size, unit)
                        });
                    }
                }
            }

            if (changes.RecipeFlags != null && changes.RecipeFlags.Any())
            {
                foreach (var flag in changes.RecipeFlags)
                {
                    var title = flag.Key.GetResource(typeof(Resources.Health));
                    result.Add(new IngredientChangeAnalyzed
                    {
                        Id = 0,
                        Title = title,
                        Size = 0,
                        ChangeType = RecipeChangeType.AddRemark,
                        Description = RecipeHelper.AnalyzeDescription(RecipeChangeType.AddRemark, title, "", 0, "")
                    });
                }
            }

            return result;
        }

        public async Task<ServiceResult> UpdateDefaultRecipeVariablesAsync(int recipeId)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(recipeId);
            if (recipe != null)
            {
                var customization = await _nutritionRepository.GetDefaultCustomizationAsync(recipeId);
                if (customization != null)
                {
                    var customizedRecipe = await CalculateRecipeAsync(recipe.Code, null, true);

                    customization.Price = customizedRecipe.Price;
                    customization.Energy = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Cal")?.Value;
                    customization.Protein = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Pro")?.Value;
                    customization.TotalFat = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Fat")?.Value;
                    customization.Carbohydrate = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Carbs")?.Value;
                    customization.Cholesterol = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Chol")?.Value;
                    customization.DietaryFiber = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Fi")?.Value;
                    customization.Sugars = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Sugars")?.Value;
                    customization.Sudium = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Sod")?.Value;
                    customization.TransFat = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Trans Fat")?.Value;
                    customization.SaturatedFat = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Saturated Fat")?.Value;
                    customization.Iron = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Iron")?.Value;
                    customization.VitaminA = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Vitamin A")?.Value;
                    customization.VitaminC = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Vitamin C")?.Value;
                    customization.Zinc = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Zinc")?.Value;
                    customization.CalculateTime = DateTime.UtcNow;
                    customization.Recipe = null;
                    var result = await _nutritionRepository.UpdateCustomizationAsync(customization);
                    return new ServiceResult(result);
                }
            }
            return new ServiceResult(false);
        }

        public async Task<List<UserDto>> GetRecipeOwnersAsync(int recipeId)
        {
            var users = await _nutritionRepository.GetRecipeOwnersAsync(recipeId);
            return _iMapper.Map<List<UserDto>>(users);
        }

        public async Task<ServiceResult> AddRecipeOwnerAsync(int recipeId, string ownerId)
        {
            var recipeOwner = await _nutritionRepository.GetRecipeOwnerAsync(recipeId, ownerId);

            if (recipeOwner != null)
            {
                return new ServiceResult(true);
            }

            var id = await _nutritionRepository.AddRecipeOwnerAsync(new RecipeOwner
            {
                RecipeId = recipeId,
                UserId = ownerId
            });
            return new ServiceResult(id > 0);
        }

        public async Task<ServiceResult> RemoveRecipeOwnerAsync(int recipeId, string ownerId)
        {
            var recipeOwner = await _nutritionRepository.GetRecipeOwnerAsync(recipeId, ownerId);
            if (recipeOwner == null)
            {
                return new ServiceResult(false);
            }
            return new ServiceResult(await _nutritionRepository.RemoveRecipeOwnerAsync(recipeOwner));
        }

        #endregion Recipe

        #region Ingredient

        public async Task<ServiceResult> CreateIngredientAsync(IngredientDto model)
        {
            var sameIngredient = await _nutritionRepository.GetIngredientByTitleAsync(model.Title.Trim().ToLower());
            if (sameIngredient != null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "", Description = "Title exists" } });
            }

            var ingredient = _iMapper.Map<Ingredient>(model);
            var id = await _nutritionRepository.CreateIngredientAsync(ingredient);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "Ingredient", "CreateIngredient", model.Title,
                    id.ToString(), model, model.LogInfo);
                return new ServiceResult(true, new Dictionary<string, string> { { "Id", id.ToString() } });
            }

            return new ServiceResult(false);
        }

        public async Task<List<IngredientDto>> GetIngredientsAsync(IngredientFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetIngredientsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Ingredient>, List<IngredientDto>>(model);
        }

        public async Task<Ingredients> GetIngredientsModelAsync(IngredientFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetIngredientsCountAsync();

            var recordsFilters = await _nutritionRepository.GetIngredientsCountAsync(predicate);

            return new Ingredients
            {
                Items = await GetIngredientsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<IngredientDto> GetIngredientAsync(int? id)
        {
            var Ingredient = await _nutritionRepository.GetIngredientAsync(id);
            return _iMapper.Map<IngredientDto>(Ingredient);
        }

        public async Task<DeleteIngredient> GetIngredientForDeleteAsync(int? id)
        {
            var Ingredient = await _nutritionRepository.GetIngredientAsync(id);
            return _iMapper.Map<DeleteIngredient>(Ingredient);
        }

        public async Task<ServiceResult> UpdateIngredientAsync(IngredientDto model)
        {
            var sameIngredient = await _nutritionRepository.GetIngredientByTitleAsync(model.Title.ToLower());
            if (sameIngredient != null && sameIngredient.Id != model.Id)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "", Description = "Title is exist" } });
            }
            var ingredient = await _nutritionRepository.GetIngredientAsync(model.Id);
            ingredient.EditDateTime = DateTime.UtcNow;
            ingredient.Title = model.Title;
            ingredient.OrderPeriod = model.OrderPeriod;
            ingredient.UnitId = model.UnitId;

            var succeeded = await _nutritionRepository.EditIngredientAsync(ingredient);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "Ingredient", "UpdateIngredient", model.Title,
                    ingredient.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteIngredientAsync(int? id)
        {
            try
            {
                var Ingredient = await _nutritionRepository.GetIngredientAsync(id);
                if (Ingredient == null)
                {
                    return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Ingredient not exist" } });
                }

                var succeeded = await _nutritionRepository.DeleteIngredientAsync(Ingredient);

                if (succeeded)
                {
                    return new ServiceResult(true);
                }

                return new ServiceResult(false);
            }
            catch (Exception e)
            {
                return new ServiceResult(false, new List<ServiceError>
                {
                    new ServiceError {
                        Level = ErrorLevel.Blocker,
                        Description = string.IsNullOrEmpty(e.InnerException.Message)
                            ? e.Message
                            : e.InnerException.Message
                    }
                });
            }

        }

        public async Task<int> CountIngredientsAsyc(IngredientFilters? filter = null)
        {
            var predicate = NutritionFilter.ToExpression(filter);
            return await _nutritionRepository.GetIngredientsCountAsync(predicate);
        }

        #endregion Ingredient

        #region IngredientType

        public async Task<ServiceResult> CreateIngredientTypeAsync(IngredientTypeDto model)
        {
            var ingredientType = _iMapper.Map<IngredientType>(model);
            if (model.Allergens?.Any() == true)
            {
                foreach (var allergen in model.Allergens)
                {
                    ingredientType?.AllergenAlerts.Add(new IngredientTypeAllergenAlert
                    {
                        AllergenAlert = allergen
                    });
                }
            }
            var id = await _nutritionRepository.CreateIngredientTypeAsync(ingredientType);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "IngredientType", "CreateIngredientType", model.Title,
                    id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<List<IngredientTypeDto>> GetIngredientTypesAsync(IngredientTypeFilters filter)
        {
            var predicate = NutritionFilter.ToExpressionIngredientType(filter);

            var model = await _nutritionRepository.GetIngredientTypesAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<IngredientType>, List<IngredientTypeDto>>(model);
        }

        public async Task<IngredientTypes> GetIngredientTypesModelAsync(IngredientTypeFilters filter)
        {
            var predicateTotal = NutritionFilter.ToExpressionIngredientType(filter.IngredientId);
            var predicateFilter = NutritionFilter.ToExpressionIngredientType(filter);

            var recordTotal = await _nutritionRepository.GetIngredientTypesCountAsync(predicateTotal);

            var recordsFilters = await _nutritionRepository.GetIngredientTypesCountAsync(predicateFilter);

            return new IngredientTypes
            {
                Items = await GetIngredientTypesAsync(filter),
                Ingredient = filter.IngredientId != 0 ? await GetIngredientAsync(filter.IngredientId) : null,
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                IngredientId = filter.IngredientId
            };
        }

        public async Task<IngredientTypeDto> GetIngredientTypeAsync(int? id)
        {
            var IngredientType = await _nutritionRepository.GetIngredientTypeAsync(id);
            return _iMapper.Map<IngredientTypeDto>(IngredientType);
        }

        public async Task<DeleteIngredientType> GetIngredientTypeForDeleteAsync(int? id)
        {
            var IngredientType = await _nutritionRepository.GetIngredientTypeAsync(id);
            return _iMapper.Map<DeleteIngredientType>(IngredientType);
        }

        public async Task<ServiceResult> UpdateIngredientTypeAsync(IngredientTypeDto model)
        {
            var ingredientType = await _nutritionRepository.GetIngredientTypeAsync(model.Id);
            ingredientType.EditDateTime = DateTime.UtcNow;
            ingredientType.Title = model.Title;
            ingredientType.DisplayTitle = model.DisplayTitle;
            ingredientType.BasePrice = model.BasePrice;
            ingredientType.MixDescription = model.MixDescription;
            ingredientType.Pchef = model.Pchef;
            ingredientType.NeedsPrep = model.NeedsPrep;
            var allergenAlerts = model.Allergens?.Select(p => new IngredientTypeAllergenAlert
            {
                AllergenAlert = p,
                IngredientTypeId = model.Id
            }).ToList();


            var succeeded = await _nutritionRepository.EditIngredientTypeAsync(ingredientType, allergenAlerts);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "IngredientType", "UpdateIngredientType", model.Title,
                    ingredientType.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteIngredientTypeAsync(int? id)
        {
            //TODO: check relations
            var IngredientType = await _nutritionRepository.GetIngredientTypeAsync(id);
            if (IngredientType == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "IngredientType not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteIngredientTypeAsync(IngredientType);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }


        #endregion IngredientType

        #region IngredientTypeUnit

        public async Task<ServiceResult> CreateIngredientTypeUnitAsync(IngredientTypeUnitDto model)
        {
            var ingredientTypeUnit = _iMapper.Map<IngredientTypeUnit>(model);
            if (await _nutritionRepository.GetIngredientTypeUnitByTypeAndUnitId(model.IngredientTypeId, model.UnitId) != null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "", Description = "this ingredient type and diet insert befor." } });
            }

            var id = await _nutritionRepository.CreateIngredientTypeUnitAsync(ingredientTypeUnit);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "IngredientTypeUnit", "CreateIngredientTypeUnit", model.IngredientTypeId.ToString(),
                    id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<List<IngredientTypeUnitDto>> GetIngredientTypeUnitsAsync(IngredientTypeUnitFilters filter)
        {
            var predicate = NutritionFilter.ToExpressionIngredientTypeUnit(filter);

            var model = await _nutritionRepository.GetIngredientTypeUnitsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<IngredientTypeUnit>, List<IngredientTypeUnitDto>>(model);
        }

        public async Task<IngredientTypeUnits> GetIngredientTypeUnitsModelAsync(IngredientTypeUnitFilters filter)
        {
            var predicateTotal = NutritionFilter.ToExpressionIngredientTypeUnit(filter.IngredientTypeId);
            var predicateFilter = NutritionFilter.ToExpressionIngredientTypeUnit(filter);

            var recordTotal = await _nutritionRepository.GetIngredientTypeUnitsCountAsync(predicateTotal);

            var recordsFilters = await _nutritionRepository.GetIngredientTypeUnitsCountAsync(predicateFilter);
            var ingredientType = await _nutritionRepository.GetIngredientTypeAsync(filter.IngredientTypeId);

            return new IngredientTypeUnits
            {
                Items = await GetIngredientTypeUnitsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                IngredientTypeId = filter.IngredientTypeId,
                IngredientType = _iMapper.Map<IngredientTypeDto>(ingredientType)
            };
        }

        public async Task<IngredientTypeUnitDto> GetIngredientTypeUnitAsync(int? id)
        {
            var IngredientTypeUnit = await _nutritionRepository.GetIngredientTypeUnitAsync(id);
            return _iMapper.Map<IngredientTypeUnitDto>(IngredientTypeUnit);
        }

        public async Task<DeleteIngredientTypeUnit> GetIngredientTypeUnitForDeleteAsync(int? id)
        {
            var IngredientTypeUnit = await _nutritionRepository.GetIngredientTypeUnitAsync(id);
            return _iMapper.Map<DeleteIngredientTypeUnit>(IngredientTypeUnit);
        }

        public async Task<ServiceResult> UpdateIngredientTypeUnitAsync(IngredientTypeUnitDto model)
        {
            var ingredientTypeUnit = await _nutritionRepository.GetIngredientTypeUnitAsync(model.Id);
            ingredientTypeUnit.EditDateTime = DateTime.UtcNow;
            ingredientTypeUnit.IngredientTypeId = model.IngredientTypeId;
            ingredientTypeUnit.IsPercent = model.IsPercent;
            ingredientTypeUnit.UnitId = model.UnitId;
            ingredientTypeUnit.PriceOperator = model.PriceOperator;
            ingredientTypeUnit.PriceFactor = model.PriceFactor;
            ingredientTypeUnit.ConversionFactor = model.ConversionFactor;
            ingredientTypeUnit.AmountOperator = model.AmountOperator;
            ingredientTypeUnit.AmountFactor = model.AmountFactor;
            ingredientTypeUnit.MakeYourOwn = model.MakeYourOwn;
            ingredientTypeUnit.ProfitMargin = model.ProfitMargin;
            ingredientTypeUnit.Amounts = model.Amounts;
            ingredientTypeUnit.Energy = model.Energy;
            ingredientTypeUnit.Protein = model.Protein;
            ingredientTypeUnit.TotalFat = model.TotalFat;
            ingredientTypeUnit.TransFat = model.TransFat;
            ingredientTypeUnit.SaturatedFat = model.SaturatedFat;
            ingredientTypeUnit.Cholesterol = model.Cholesterol;
            ingredientTypeUnit.Carbohydrate = model.Carbohydrate;
            ingredientTypeUnit.DietaryFiber = model.DietaryFiber;
            ingredientTypeUnit.Sugars = model.Sugars;
            ingredientTypeUnit.Sudium = model.Sudium;
            ingredientTypeUnit.Iron = model.Iron;
            ingredientTypeUnit.VitaminA = model.VitaminA;
            ingredientTypeUnit.VitaminC = model.VitaminC;
            ingredientTypeUnit.Zinc = model.Zinc;

            var succeeded = await _nutritionRepository.EditIngredientTypeUnitAsync(ingredientTypeUnit);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "IngredientTypeUnit", "UpdateIngredientTypeUnit", model.IngredientTypeId.ToString(),
                    ingredientTypeUnit.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteIngredientTypeUnitAsync(int? id)
        {
            //TODO: check relations
            var IngredientTypeUnit = await _nutritionRepository.GetIngredientTypeUnitAsync(id);
            if (IngredientTypeUnit == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "IngredientTypeUnit not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteIngredientTypeUnitAsync(IngredientTypeUnit);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        #endregion IngredientTypeUnit

        #region RecipeIngredientTypeUnit

        public async Task<ServiceResult> CreateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnitDto model)
        {
            var RecipeIngredientTypeUnit = _iMapper.Map<RecipeIngredientTypeUnit>(model);
            var id = await _nutritionRepository.CreateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "RecipeIngredientTypeUnit", "CreateRecipeIngredientTypeUnit", model.IngredientTypeUnitId.ToString(),
                    id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<List<RecipeIngredientTypeUnitDto>> GetRecipeIngredientTypeUnitsAsync(RecipeIngredientTypeUnitFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetRecipeIngredientTypeUnitsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<RecipeIngredientTypeUnitDto>>(model);
        }

        public async Task<RecipeIngredientTypeUnits> GetRecipeIngredientTypeUnitsModelAsync(RecipeIngredientTypeUnitFilters filter)
        {
            var predicateTotal = NutritionFilter.ToExpressionRecipeIngredientTypeUnit(filter.RecipeId);
            var predicateFilter = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetRecipeIngredientTypeUnitsCountAsync(predicateTotal);

            var recordsFilters = await _nutritionRepository.GetRecipeIngredientTypeUnitsCountAsync(predicateFilter);
            var recipe = await _nutritionRepository.GetRecipeAsync(filter.RecipeId,
                include: p => p.Include(q => q.Food));

            return new RecipeIngredientTypeUnits
            {
                Items = await GetRecipeIngredientTypeUnitsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                RecipeId = filter.RecipeId,
                Recipe = _iMapper.Map<RecipeDto>(recipe)
            };
        }

        public async Task<RecipeIngredientTypeUnitDto> GetRecipeIngredientTypeUnitAsync(int? id)
        {
            var RecipeIngredientTypeUnit = await _nutritionRepository.GetRecipeIngredientTypeUnitAsync(id);
            var model = _iMapper.Map<RecipeIngredientTypeUnitDto>(RecipeIngredientTypeUnit);
            var ingredientType = await _nutritionRepository.GetIngredientTypeUnitAsync(model.IngredientTypeUnitId);
            model.IngredientTypeId = ingredientType.IngredientTypeId;
            model.IngredientId = ingredientType.IngredientType.IngredientId;
            return model;
        }

        public async Task<DeleteRecipeIngredientTypeUnit> GetRecipeIngredientTypeUnitForDeleteAsync(int? id)
        {
            var RecipeIngredientTypeUnit = await _nutritionRepository.GetRecipeIngredientTypeUnitAsync(id);
            return _iMapper.Map<DeleteRecipeIngredientTypeUnit>(RecipeIngredientTypeUnit);
        }

        public async Task<ServiceResult> UpdateRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnitDto model)
        {
            var RecipeIngredientTypeUnit = await _nutritionRepository.GetRecipeIngredientTypeUnitAsync(model.Id);
            RecipeIngredientTypeUnit.EditDateTime = DateTime.UtcNow;
            RecipeIngredientTypeUnit.IngredientTypeUnitId = model.IngredientTypeUnitId;
            RecipeIngredientTypeUnit.Amount = model.Amount;
            RecipeIngredientTypeUnit.Order = model.Order;
            RecipeIngredientTypeUnit.IsAddOn = model.IsAddOn;
            RecipeIngredientTypeUnit.IsDressing = model.IsDressing;

            var succeeded = await _nutritionRepository.EditRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "RecipeIngredientTypeUnit", "UpdateRecipeIngredientTypeUnit", model.IngredientTypeUnitId.ToString(),
                    RecipeIngredientTypeUnit.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteRecipeIngredientTypeUnitAsync(int? id)
        {
            //TODO: check relations
            var RecipeIngredientTypeUnit = await _nutritionRepository.GetRecipeIngredientTypeUnitAsync(id);
            if (RecipeIngredientTypeUnit == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "", Description = "RecipeIngredientTypeUnit not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteRecipeIngredientTypeUnitAsync(RecipeIngredientTypeUnit);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }


        #endregion RecipeIngredientTypeUnit

        #region RecipeOverheadCost

        public async Task<ServiceResult> CreateRecipeOverheadCostAsync(RecipeOverheadCostDto model)
        {
            var RecipeOverheadCost = _iMapper.Map<RecipeOverheadCost>(model);
            RecipeOverheadCost.OverheadCost = null;
            var id = await _nutritionRepository.CreateRecipeOverheadCostAsync(RecipeOverheadCost);
            var succeeded = id > 0;

            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "RecipeOverheadCost", "CreateRecipeOverheadCost", model.OverheadCostId.ToString(),
                    id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<List<RecipeOverheadCostDto>> GetRecipeOverheadCostsAsync(RecipeOverheadCostFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetRecipeOverheadCostsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<RecipeOverheadCost>, List<RecipeOverheadCostDto>>(model);
        }

        public async Task<RecipeOverheadCosts> GetRecipeOverheadCostsModelAsync(RecipeOverheadCostFilters filter)
        {
            var predicateTotal = NutritionFilter.ToExpressionRecipeOverheadCost(filter.RecipeId.Value);
            var predicateFilter = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetRecipeOverheadCostsCountAsync(predicateTotal);

            var recordsFilters = await _nutritionRepository.GetRecipeOverheadCostsCountAsync(predicateFilter);
            var recipe = await _nutritionRepository.GetRecipeAsync(filter.RecipeId,
                include: p => p.Include(q => q.Food));

            return new RecipeOverheadCosts
            {
                Items = await GetRecipeOverheadCostsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                RecipeId = filter.RecipeId.Value,
                Recipe = _iMapper.Map<RecipeDto>(recipe)
            };
        }

        public async Task<RecipeOverheadCostDto> GetRecipeOverheadCostAsync(int? id)
        {
            var RecipeOverheadCost = await _nutritionRepository.GetRecipeOverheadCostAsync(id);
            var model = _iMapper.Map<RecipeOverheadCostDto>(RecipeOverheadCost);
            return model;
        }

        public async Task<DeleteRecipeOverheadCost> GetRecipeOverheadCostForDeleteAsync(int? id)
        {
            var RecipeOverheadCost = await _nutritionRepository.GetRecipeOverheadCostAsync(id);
            return _iMapper.Map<DeleteRecipeOverheadCost>(RecipeOverheadCost);
        }

        public async Task<ServiceResult> UpdateRecipeOverheadCostAsync(RecipeOverheadCostDto model)
        {
            var RecipeOverheadCost = await _nutritionRepository.GetRecipeOverheadCostAsync(model.Id);
            RecipeOverheadCost.EditDateTime = DateTime.UtcNow;
            RecipeOverheadCost.OverheadCostId = model.OverheadCostId;
            RecipeOverheadCost.Amount = model.Amount;

            var succeeded = await _nutritionRepository.EditRecipeOverheadCostAsync(RecipeOverheadCost);
            if (succeeded)
            {
                await _logService.AddUserLogAsync(model.LogUserId, "RecipeOverheadCost", "UpdateRecipeOverheadCost", model.OverheadCostId.ToString(),
                    RecipeOverheadCost.Id.ToString(), model, model.LogInfo);
                return new ServiceResult(true);
            }

            return new ServiceResult(false);
        }

        public async Task<ServiceResult> DeleteRecipeOverheadCostAsync(int? id)
        {
            //TODO: check relations
            var RecipeOverheadCost = await _nutritionRepository.GetRecipeOverheadCostAsync(id);
            if (RecipeOverheadCost == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "", Description = "RecipeOverheadCost not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteRecipeOverheadCostAsync(RecipeOverheadCost);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }


        #endregion RecipeOverheadCost

        #region Units
        public async Task<List<UnitDto>> GetUnitsAsync(UnitFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetUnitsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Unit>, List<UnitDto>>(model);
        }

        public async Task<Units> GetUnitsModelAsync(UnitFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetUnitsCountAsync();

            var recordsFilters = await _nutritionRepository.GetUnitsCountAsync(predicate);

            return new Units
            {
                Items = await GetUnitsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }
        #endregion Units

        #region Diets
        public async Task<List<DietApi>> GetDietsApiAsync(DietFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetDietsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Diet>, List<DietApi>>(model.Where(p => p.IsActive).ToList());
        }

        public async Task<List<DietDto>> GetDietsAsync(DietFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetDietsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Diet>, List<DietDto>>(model);
        }

        public async Task<Diets> GetDietsModelAsync(DietFilters filter)
        {
            var predicate = NutritionFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetDietsCountAsync();

            var recordsFilters = await _nutritionRepository.GetDietsCountAsync(predicate);

            return new Diets
            {
                Items = await GetDietsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<List<DietDto>> GetDietsByRecipeAsync(int recipeId)
        {
            var diets = await _nutritionRepository.GetDietsByRecipeAsync(recipeId);
            return _iMapper.Map<List<Diet>, List<DietDto>>(diets);
        }

        public async Task<ServiceResult> AddDietToRecipeAsync(RecipeDietDto model)
        {
            var recipeDiet = _iMapper.Map<RecipeDiet>(model);
            var result = await _nutritionRepository.AddDietToRecipeAsync(recipeDiet);
            return new ServiceResult(result > 0);
        }

        public async Task<ServiceResult> RemoveDietFromRecipeAsync(RecipeDietDto model)
        {
            var recipeDiet = _iMapper.Map<RecipeDiet>(model);
            var result = await _nutritionRepository.RemoveDietFromRecipeAsync(recipeDiet);
            return new ServiceResult(result);
        }
        #endregion Diets

        #region Tags
        public async Task<List<TagApi>> GetTagsApiAsync(TagFilters filter)
        {
            var predicate = MarketingFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetTagsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Tag>, List<TagApi>>(model);
        }

        public async Task<List<TagDto>> GetTagsAsync(TagFilters filter)
        {
            var predicate = MarketingFilter.ToExpression(filter);

            var model = await _nutritionRepository.GetTagsAsync(filter.Start, filter.PageSize, filter.Sort, filter.Direction, predicate);

            return _iMapper.Map<List<Tag>, List<TagDto>>(model);
        }

        public async Task<Tags> GetTagsModelAsync(TagFilters filter)
        {
            var predicate = MarketingFilter.ToExpression(filter);

            var recordTotal = await _nutritionRepository.GetTagsCountAsync();

            var recordsFilters = await _nutritionRepository.GetTagsCountAsync(predicate);

            return new Tags
            {
                Items = await GetTagsAsync(filter),
                TotalCount = recordTotal,
                FilteredCount = recordsFilters,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<List<TagDto>> GetTagsByRecipeAsync(int recipeId)
        {
            var diets = await _nutritionRepository.GetTagsByRecipeAsync(recipeId);
            return _iMapper.Map<List<Tag>, List<TagDto>>(diets);
        }

        public async Task<List<string>> GetRecipeAllergenAllertsAsync(int recipeId)
        {
            var allergenAllerts = await _nutritionRepository.GetRecipeAllergenAllertsAsync(recipeId);
            return allergenAllerts.Select(p => EnumHelper<AllergenAlert>.GetDisplayValue(p)).ToList();
        }

        public async Task<ServiceResult> AddTagToRecipeAsync(RecipeTagDto model)
        {
            var recipeTag = _iMapper.Map<RecipeTag>(model);
            var result = await _nutritionRepository.AddTagToRecipeAsync(recipeTag);
            return new ServiceResult(result > 0);
        }

        public async Task<ServiceResult> RemoveTagFromRecipeAsync(RecipeTagDto model)
        {
            var recipeTag = _iMapper.Map<RecipeTag>(model);
            var result = await _nutritionRepository.RemoveTagFromRecipeAsync(recipeTag);
            return new ServiceResult(result);
        }
        #endregion Tags

        public async Task<List<string>> GetAllRecipesCodeAsync()
        {
            return await _nutritionRepository.GetAllRecipesCodeAsync();
        }

        public void UpdateRecipesPrice()
        {
            var recipeCodes = GetAllRecipesCodeAsync().Result;
            foreach (var code in recipeCodes)
            {
                var recipe = _nutritionRepository.GetRecipeAsync(code.Trim().ToLower(),
                include: p => p
                    .Include(q => q.Food)
                    .Include(q => q.RecipeOverheadCosts)
                    .ThenInclude(q => q.OverheadCost)
                    .Include(q => q.RecipeIngredientTypeUnits)
                    .ThenInclude(q => q.IngredientTypeUnit)
                    .ThenInclude(q => q.IngredientType)
                    .ThenInclude(q => q.Ingredient)).Result;

                var price = recipe.CalculatePrice();
                _nutritionRepository.UpdateRecipePrice(recipe.Id, price);
            }
        }

        public decimal UpdateRecipePrice(string code)
        {
            var recipe = _nutritionRepository.GetRecipeAsync(code.Trim().ToLower(),
            include: p => p
                .Include(q => q.Food)
                .Include(q => q.RecipeOverheadCosts)
                .ThenInclude(q => q.OverheadCost)
                .Include(q => q.RecipeIngredientTypeUnits)
                .ThenInclude(q => q.IngredientTypeUnit)
                .ThenInclude(q => q.IngredientType)
                .ThenInclude(q => q.Ingredient)).Result;

            var price = recipe.CalculatePrice();
            var updated = _nutritionRepository.UpdateRecipePrice(recipe.Id, price);
            return updated ? price : 0;
        }

        public void CalculateRecipes()
        {
            var customizations = _nutritionRepository.GetAllCustomizationIdsAsync().Result;

            foreach (var customizationId in customizations)
            {
                var customization = _nutritionRepository.GetCustomizationAsync(customizationId).Result;
                if (customization != null)
                {
                    var recipe = GetRecipeAsync(customization.RecipeId).Result;
                    var customize = new RecipeChanges();
                    if (!string.IsNullOrEmpty(customization.Changes))
                    {
                        customize = JsonSerializer.Deserialize<RecipeChanges>(customization.Changes);
                    }
                    var customizedRecipe = CalculateRecipeAsync(recipe.Code, customize, true).Result;

                    customization.Price = customizedRecipe.Price;
                    customization.Energy = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Cal")?.Value;
                    customization.Protein = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Pro")?.Value;
                    customization.TotalFat = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Fat")?.Value;
                    customization.Carbohydrate = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Carbs")?.Value;
                    customization.Cholesterol = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Chol")?.Value;
                    customization.DietaryFiber = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Fi")?.Value;
                    customization.Sugars = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Sugars")?.Value;
                    customization.Sudium = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Sod")?.Value;
                    customization.TransFat = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Trans Fat")?.Value;
                    customization.SaturatedFat = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Saturated Fat")?.Value;
                    customization.Iron = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Iron")?.Value;
                    customization.VitaminA = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Vitamin A")?.Value;
                    customization.VitaminC = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Vitamin C")?.Value;
                    customization.Zinc = customizedRecipe.NutritionFacts.FirstOrDefault(p => p.Title == "Zinc")?.Value;
                    customization.CalculateTime = DateTime.UtcNow;
                    customization.Recipe = null;
                    var result = _nutritionRepository.UpdateCustomizationAsync(customization).Result;
                }
            }
        }

        public async Task<List<SubstituteSearch>> SearchIngredientForSubstituteAsync(string query, string unit)
        {
            var result = new List<SubstituteSearch>();
            var ingredients = await _nutritionRepository.SearchIngredientsByUnitAsync(query, unit);
            result = ingredients.Select(p => new SubstituteSearch
            {
                Id = p.Id,
                Title = p.IngredientType.DisplayTitle
            }).ToList();

            return result;
        }

        public async Task<ServiceResult> UpdateRecipeRemarksAsync(RecipeRemarks model)
        {
            var recipe = await _nutritionRepository.GetRecipeAsync(model.RecipeId);
            if (recipe == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Recipe not exist" } });
            }

            recipe.AllowNoSalt = model.AllowNoSalt;
            recipe.AllowNoPepper = model.AllowNoPepper;
            recipe.AllowNoAppleCider = model.AllowNoAppleCider;
            recipe.AllowNoSalmonSkin = model.AllowNoSalmonSkin;
            var updated = await _nutritionRepository.UpdateRecipeAsync(recipe);
            return new ServiceResult(updated);
        }

        public async Task<ServiceResult> DeleteIngredientTypeSubCategoryAsync(int id)
        {
            var model = await _nutritionRepository.GetIngredientTypeSubCategoryAsync(id);
            if (model == null)
            {
                return new ServiceResult(false, new List<ServiceError> { new ServiceError { Code = "Not found", Description = "Item not exist" } });
            }
            var succeeded = await _nutritionRepository.DeleteIngredientTypeSubCategoryAsync(model);

            if (succeeded)
            {
                return new ServiceResult(true);
            }
            return new ServiceResult(false);
        }

        public async Task<List<IngredientCategoryDto>> GetIngreidnetCategoriesAsync()
        {
            var categories = await _nutritionRepository.GetIngreidnetCategoriesAsync();
            return _iMapper.Map<List<IngredientCategoryDto>>(categories);
        }

        public async Task<List<IngredientCategoryApi>> GetIngreidnetCategoriesApiAsync()
        {
            var categories = await _nutritionRepository.GetIngreidnetCategoriesAsync();
            return _iMapper.Map<List<IngredientCategoryApi>>(categories);
        }

        public async Task<List<IngredientSubCategoryApi>> GetIngreidnetSubCategoriesApiAsync(string permalink)
        {
            var subCategories = await _nutritionRepository.GetIngreidnetSubCategoriesAsync(permalink);
            return _iMapper.Map<List<IngredientSubCategoryApi>>(subCategories);
        }

        public async Task<ServiceResult> CreateIngredientTypeSubCategoryAsync(IngredientTypeSubCategoryDto model)
        {
            var duplicate = await _nutritionRepository.GetIngredientTypeSubCategoryAsync(model.IngredientTypeId, model.IngredientSubCategoryId);
            if (duplicate != null)
            {
                return new ServiceResult(false, new List<ServiceError>
                {
                    new ServiceError { Code = "Duplicate", Description = "Title is exist" }
                });
            }
            var id = await _nutritionRepository.CreateIngredientTypeSubCategoryAsync(_iMapper.Map<IngredientTypeSubCategory>(model));
            var succeeded = id > 0;

            return new ServiceResult(succeeded);
        }

        public async Task<List<PlateIngredientApi>> GetPlateIngreidnetsApiAsync(string subCategory)
        {
            var ingredients = await _nutritionRepository.GetPlateIngredientsAsync(subCategory);
            return _iMapper.Map<List<PlateIngredientApi>>(ingredients);
        }
    }
}
