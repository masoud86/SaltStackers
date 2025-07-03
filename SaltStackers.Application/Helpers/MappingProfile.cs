using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SaltStackers.Application.ViewModels.Api;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Customer;
using SaltStackers.Application.ViewModels.Financial;
using SaltStackers.Application.ViewModels.Log;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using SaltStackers.Application.ViewModels.Operation;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Application.ViewModels.Settings;
using SaltStackers.Application.ViewModels.Settings.Alert;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using SaltStackers.Domain.Models.Financial;
using SaltStackers.Domain.Models.Log;
using SaltStackers.Domain.Models.Membership;
using SaltStackers.Domain.Models.Nutrition;
using SaltStackers.Domain.Models.Operation;
using SaltStackers.Domain.Models.Setting;

namespace SaltStackers.Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            var publicUrl = root.GetSection("PublicUrl").Get<string>();

            #region Basic Informations

            CreateMap<Unit, UnitDto>().ReverseMap();
            CreateMap<OverheadCost, OverheadCostDto>().ReverseMap();

            #endregion Basic Informations

            #region Customer

            CreateMap<AspNetUser, CustomerDto>();
            CreateMap<CustomerDto, EditUser>();
            CreateMap<AspNetUser, EditUser>();
            CreateMap<AspNetUser, CustomerProfileApi>()
                .ForMember(dest => dest.EmailAddress, map => map.MapFrom(source => source.Email));
            CreateMap<AspNetUser, UserDto>();
            CreateMap<CustomerDto, UserDto>()
                .ForMember(dest => dest.Username, map => map.MapFrom(source => source.Email));
            CreateMap<AspNetUser, CustomerInformation>()
                .ForMember(dest => dest.EmailAddress, map => map.MapFrom(source => source.Email))
                .ReverseMap();

            #endregion Customer

            #region Log

            CreateMap<ApplicationLog, ApplicationLogDto>();
            CreateMap<ClientInformation, UserActivityLogDto>();
            CreateMap<UserActivityLog, UserActivityLogDto>()
                .ForMember(dest => dest.ContentString, map => map.MapFrom(source => source.Content))
                .ForMember(dest => dest.Description,
                    map => map.MapFrom(source =>
                        source.DescriptionResourceKey.GetResource(typeof(Resources.Activity), source.DescriptionParameters)));
            CreateMap<UserActivityLogDto, UserActivityLog>()
                .ForMember(dest => dest.Id, map => map.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Content,
                    map => map.MapFrom(source => JsonConvert.SerializeObject(source.Content)))
                .ForMember(dest => dest.CreateDateTime, map => map.MapFrom(_ => DateTime.UtcNow));

            #endregion Log

            #region Membership

            CreateMap<IdentityUser, AspNetUser>();
            CreateMap<CreateRole, AspNetRole>();
            CreateMap<EditRole, AspNetRole>().ReverseMap();
            CreateMap<DeleteRole, AspNetRole>().ReverseMap();
            CreateMap<CreateUser, AspNetUser>()
                .ForMember(dest => dest.UserName, map => map.MapFrom(source => source.Email));
            CreateMap<AspNetRole, RoleDto>().ReverseMap();

            #endregion Membership

            #region Nutrition

            CreateMap<DayOfWeek, Day>()
                .ForMember(dest => dest.Number, map => map.MapFrom(source => (int)source))
                .ForMember(dest => dest.Name, map => map.MapFrom(source => source.ToString()));
            CreateMap<Food, FoodDto>().ReverseMap();
            CreateMap<Food, DeleteFood>().ReverseMap();
            CreateMap<Recipe, RecipeDto>().ReverseMap();
            CreateMap<Recipe, DeleteRecipe>().ReverseMap();
            CreateMap<Ingredient, DeleteIngredient>().ReverseMap();
            CreateMap<Ingredient, IngredientDto>().ReverseMap();

            CreateMap<IngredientType, DeleteIngredientType>().ReverseMap();
            CreateMap<IngredientTypeDto, IngredientType>();
            CreateMap<IngredientType, IngredientTypeDto>()
                .ForMember(dest => dest.Allergens, map => map.MapFrom(source => source.AllergenAlerts.Select(p => p.AllergenAlert).ToList()));
            CreateMap<IngredientTypeAllergenAlert, IngredientTypeAllergenAlertDto>().ReverseMap();

            CreateMap<IngredientTypeUnit, DeleteIngredientTypeUnit>().ReverseMap();
            CreateMap<IngredientTypeUnit, IngredientTypeUnitDto>().ReverseMap();

            CreateMap<IngredientCategory, IngredientCategoryDto>().ReverseMap();
            CreateMap<IngredientCategory, IngredientCategoryApi>()
                .ForMember(dest => dest.ImageUrl, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.Image)
                        ? ""
                        : $"{publicUrl}/category/{source.Image}"));
            CreateMap<IngredientSubCategory, IngredientSubCategoryDto>().ReverseMap();
            CreateMap<IngredientSubCategory, IngredientSubCategoryApi>()
                .ForMember(dest => dest.ImageUrl, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.Image)
                        ? ""
                        : $"{publicUrl}/category/{source.Image}"));
            CreateMap<IngredientTypeSubCategory, IngredientTypeSubCategoryDto>().ReverseMap();

            CreateMap<RecipeIngredientTypeUnit, RecipeIngredientTypeUnitDto>()
            .ReverseMap();
            CreateMap<RecipeIngredientTypeUnit, DeleteRecipeIngredientTypeUnit>()
                .ForMember(dest => dest.Title, map => map.MapFrom(source => $"{source.IngredientTypeUnit.IngredientType.Ingredient.Title} {source.IngredientTypeUnit.IngredientType.Title} {source.IngredientTypeUnit.Unit.Title}"))
                .ReverseMap();
            CreateMap<RecipeIngredientTypeSubstitute, RecipeIngredientTypeSubstituteDto>().ReverseMap();
            CreateMap<RecipeIngredientTypeAmount, RecipeIngredientTypeAmountDto>().ReverseMap();

            CreateMap<RecipeOverheadCost, RecipeOverheadCostDto>()
                .ForMember(dest => dest.OverheadCostTitle, map => map.MapFrom(source => $"{source.OverheadCost.Title}"))
                .ReverseMap();
            CreateMap<RecipeOverheadCost, DeleteRecipeOverheadCost>()
                .ForMember(dest => dest.Title, map => map.MapFrom(source => $"{source.OverheadCost.Title}"))
                .ReverseMap();

            CreateMap<Diet, DietDto>().ReverseMap();
            CreateMap<Diet, DietApi>()
                .ForMember(dest => dest.IconUrl, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.Icon)
                        ? "" : $"{publicUrl}/diet/{source.Icon}"));

            CreateMap<FoodAttachment, FoodAttachmentDto>()
                .ForMember(dest => dest.Url, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.FileName) || source.Id == 0
                        ? $"{publicUrl}/food/{source.FileName}"
                        : $"{publicUrl}/food/{source.FoodId}/{source.FileName}"));
            CreateMap<FoodAttachment, FoodAttachmentApi>()
                .ForMember(dest => dest.Url, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.FileName) || source.Id == 0
                        ? $"{publicUrl}/food/{source.FileName}"
                        : $"{publicUrl}/food/{source.FoodId}/{source.FileName}"));

            CreateMap<Recipe, MenuItem>()
                .ForMember(dest => dest.Title, map => map.MapFrom(source => source.Food.Title))
                .ForMember(dest => dest.IsNew, map => map.MapFrom(source => source.IsNew))
                .ForMember(dest => dest.IsBulk, map => map.MapFrom(source => source.RecipeDiets.Any(p => p.DietId == 6)))
                .ForMember(dest => dest.PayablePrice, map => map.MapFrom(source => source.Price))
                .ForMember(dest => dest.Images, map => map.MapFrom(source =>
                    source.Food.Attachments != null && source.Food.Attachments.Any()
                        ? source.Food.Attachments
                        : new List<FoodAttachment>
                        {
                            new FoodAttachment
                            {
                                Id = 0,
                                FileName = "default-small.png",
                                FoodId = source.FoodId,
                                IsMain = true,
                                MediaType = MediaType.Image,
                                UploadDateTime = DateTime.Now
                            }
                        }))
                .ForMember(dest => dest.Tags, map => map.MapFrom(source => source.RecipeTags.Select(p => p.Tag)));

            CreateMap<Package, MenuItem>()
                .ForMember(dest => dest.PayablePrice, map => map.MapFrom(source => source.Price))
                .ForMember(dest => dest.Images, map => map.MapFrom(source =>
                    source.Attachments != null && source.Attachments.Any()
                        ? source.Attachments
                        : new List<PackageAttachment>
                        {
                            new PackageAttachment
                            {
                                Id = 0,
                                FileName = "default-small.png",
                                PackageId = source.Id,
                                IsMain = true,
                                MediaType = MediaType.Image,
                                UploadDateTime = DateTime.Now
                            }
                        }))
                .ForMember(dest => dest.Tags, map => map.MapFrom(_ => new List<TagApi> { new TagApi { Title = "Package", Icon = "", Permalink = "" } }));

            CreateMap<FoodAttachment, MenuItemAttachment>()
                .ForMember(dest => dest.Url, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.FileName) || source.Id == 0
                        ? $"{publicUrl}/food/{source.FileName}"
                        : $"{publicUrl}/food/{source.FoodId}/{source.FileName}"));
            CreateMap<PackageAttachment, MenuItemAttachment>()
                .ForMember(dest => dest.Url, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.FileName) || source.Id == 0
                        ? $"{publicUrl}/package/{source.FileName}"
                        : $"{publicUrl}/package/{source.PackageId}/{source.FileName}"));

            CreateMap<Recipe, OtherSize>()
                .ForMember(dest => dest.Title, map => map.MapFrom(source => string.IsNullOrWhiteSpace(source.Title) ? "Regular" : source.Title));
            CreateMap<Recipe, RecipeConvert>().ReverseMap();
            CreateMap<RecipeDiet, RecipeDietDto>().ReverseMap();
            CreateMap<RecipeTag, RecipeTagDto>().ReverseMap();

            CreateMap<Combo, ComboDto>().ReverseMap();
            CreateMap<Combo, ComboApi>().ReverseMap();
            CreateMap<ComboDto, RecipeCombo>().ReverseMap();
            CreateMap<ComboDto, ComboApi>().ReverseMap();

            CreateMap<IngredientTypeUnit, PlateIngredientApi>()
                .ForMember(dest => dest.Title, map => map.MapFrom(source => source.IngredientType.DisplayTitle))
                .ForMember(dest => dest.Price, map => map.MapFrom(source => Math.Round(IngredientHelper.CalculateMakePrice(source.IngredientType.BasePrice, source.PriceOperator, source.IsPercent, source.PriceFactor, source.Amounts, source.ProfitMargin), 2)))
                .ForMember(dest => dest.Unit, map => map.MapFrom(source => source.Unit.Sign))
                .ForMember(dest => dest.Amounts, map => map.MapFrom(source => IngredientHelper.ConvertAmounts(source.Amounts)))
                .ForMember(dest => dest.NutritionFacts, map => map.MapFrom(source => RecipeHelper.CalculateNutritionFact(source)));

            CreateMap<RecipeOwner, RecipeOwnerDto>().ReverseMap();

            CreateMap<Package, PackageDto>().ReverseMap();
            CreateMap<PackageAttachment, PackageAttachmentDto>().ReverseMap();
            CreateMap<PackageGroup, PackageGroupDto>().ReverseMap();
            CreateMap<PackageGroupItem, PackageGroupItemDto>().ReverseMap();

            CreateMap<Package, PackageDetails>()
                .ForMember(dest => dest.PayablePrice, map => map.MapFrom(source => source.Price))
                .ForMember(dest => dest.Price, map => map.MapFrom(source => source.Groups.SelectMany(p => p.Items).Sum(p => p.Recipe.Price)));
            CreateMap<PackageAttachment, PackageAttachmentApi>()
                .ForMember(dest => dest.Url, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.FileName) || source.Id == 0
                        ? $"{publicUrl}/package/{source.FileName}"
                        : $"{publicUrl}/package/{source.PackageId}/{source.FileName}"));
            CreateMap<PackageGroup, PackageGroupApi>();
            CreateMap<PackageGroupItem, PackageGroupItemApi>()
                .ForMember(dest => dest.Code, map => map.MapFrom(source => source.Recipe.Code))
                .ForMember(dest => dest.Title, map => map.MapFrom(source => source.Recipe.Food.Title))
                .ForMember(dest => dest.ImageUrl, map => map.MapFrom(source => source.Recipe.Food.Attachments.Any()
                        ? $"{publicUrl}/Food/{source.Recipe.FoodId}/{source.Recipe.Food.Attachments.FirstOrDefault(p => p.IsMain).FileName}"
                        : $"{publicUrl}/Food/default-small.png"));

            #endregion Nutrition

            #region Marketing

            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Tag, TagApi>().ReverseMap();

            #endregion Marketing

            #region Operation

            CreateMap<KitchenRecipe, KitchenRecipeDto>().ReverseMap();

            CreateMap<Kitchen, KitchenDto>().ReverseMap();
            CreateMap<Kitchen, KitchenApi>()
                .ForMember(dest => dest.Status, map => map.MapFrom(source => EnumHelper<PartnerStatus>.GetDisplayValue(source.Status)))
                .ForMember(dest => dest.Zone, map => map.MapFrom(source => source.Zone.Title))
                .ForMember(dest => dest.Logo, map => map.MapFrom(source => $"{publicUrl}/kitchen/{source.Logo}"));
            CreateMap<TaxProfile, TaxProfileDto>().ReverseMap();

            #endregion Operation

            #region Settings

            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryApi>().ReverseMap();
            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<Province, ProvinceApi>().ReverseMap();
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityApi>().ReverseMap();
            CreateMap<Zone, ZoneDto>().ReverseMap();
            CreateMap<Zone, ZoneApi>().ReverseMap();
            CreateMap<Alert, AlertDto>().ReverseMap();
            CreateMap<Alert, AlertApi>()
                .ForMember(dest => dest.ImageUrl, map =>
                    map.MapFrom(source => string.IsNullOrEmpty(source.Image)
                        ? "" : $"{publicUrl}/alert/{source.Image}"));
            CreateMap<AlertUser, AlertUserDto>().ReverseMap();

            #endregion Settings
        }
    }
}
