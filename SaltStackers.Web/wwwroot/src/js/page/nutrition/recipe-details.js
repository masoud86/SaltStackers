import Toast, { useToast } from "vue-toastification";
import Chart from 'chart.js/auto';
import 'chartjs-adapter-moment';

let amountMethods = {
    openAmountsModal(ingredient) {
        this.$data.selectedIngredient = { ...ingredient };
        this.getOtherAmounts();
        this.manageAmountsModal.show();
    },
    selectOtherAmount(otherAmount) {
        this.$data.otherAmount = { ...otherAmount };
    },
    cancelOtherAmount() {
        this.$data.otherAmount = {
            id: 0,
            amount: null,
            processFee: null
        };
    },
    async getOtherAmounts() {
        let querystring = '?id=' + this.$data.selectedIngredient.id;
        await window.axios.get("/Api/Nutrition/GetIngredientOtherAmounts" + querystring)
            .then(response => {
                this.$data.otherAmounts = response.data;
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async createNewOtherAmount() {
        let model = { ...this.$data.otherAmount, ...{ 'recipeIngredientTypeUnitId': this.$data.selectedIngredient.id } };
        
        await window.axios.post("/Api/Nutrition/AddNewIngredientOtherAmount", model)
            .then(response => {
                this.cancelOtherAmount();
                this.getOtherAmounts();
                useToast().success('New amount added successfully.');
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
        await this.getRecipeIngredients()
    },
    async editOtherAmount() {
        await window.axios.post("/Api/Nutrition/EditIngredientOtherAmount", this.$data.otherAmount)
            .then(response => {
                this.cancelOtherAmount();
                this.getOtherAmounts();
                useToast().success('Amount updated successfully.');
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
        await this.getRecipeIngredients()
    },
    async deleteOtherAmount(otherAmount,) {
        if (confirm("Do you really want to delete?")) {
            let querystring = '?id=' + otherAmount.id;
            await window.axios.delete("/Api/Nutrition/DeleteIngredientOtherAmount" + querystring)
                .then(response => {
                    this.cancelOtherAmount();
                    this.getOtherAmounts();
                    useToast().success('Amount deleted successfully.');
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            await this.getRecipeIngredients()
        }
    }
};
let substituteMethods = {
    openSubstitutesModal(ingredient) {
        this.$data.selectedIngredient = ingredient;
        this.$data.selectedSubstitute = null;
        this.getSubstitutes();
        this.manageSubstitutesModal.show();
    },
    selectSubstitute(substitute) {
        this.$data.substitute = { ...substitute };
        this.$data.selectedSubstitute = substitute;
    },
    cancelSubstitute() {
        this.$data.substitute = {
            id: 0,
            substitute: null,
            processFee: null
        };
    },
    async getSubstitutes() {
        let querystring = '?id=' + this.$data.selectedIngredient.id;
        await window.axios.get("/Api/Nutrition/GetIngredientSubstitutes" + querystring)
            .then(response => {
                this.$data.substitutes = [];
                for (var i = 0; i < response.data.length; i++) {
                    let item = response.data[i];
                    this.$data.substitutes.push({
                        id: item.id,
                        title: item.ingredientTypeUnit.ingredientType.title + ' ' + item.ingredientTypeUnit.ingredientType.ingredient.title,
                        processFee: item.processFee
                    });
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async createNewSubstitute() {
        let model = model = {
            recipeIngredientTypeUnitId: this.$data.selectedIngredient.id,
            ingredientTypeUnitId: this.$data.selectedSubstitute.id,
            processFee: this.$data.substitute.processFee
        };
        
        await window.axios.post("/Api/Nutrition/AddNewIngredientSubstitute", model)
            .then(response => {
                this.cancelSubstitute();
                this.getSubstitutes();
                useToast().success('New substitute added successfully.');
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
        await this.getRecipeIngredients()
    },
    async editSubstitute() {
        let model = {
            id: this.$data.substitute.id,
            recipeIngredientTypeUnitId: this.$data.selectedIngredient.id,
            ingredientTypeUnitId: this.$data.selectedSubstitute.id,
            processFee: this.$data.substitute.processFee
        };
        await window.axios.post("/Api/Nutrition/EditIngredientSubstitute", model)
            .then(response => {
                this.cancelSubstitute();
                this.getSubstitutes();
                useToast().success('Substitute updated successfully.');
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
        await this.getRecipeIngredients()
    },
    async deleteSubstitute(substitute) {
        if (confirm("Do you really want to delete?")) {
            let querystring = '?id=' + substitute.id;
            await window.axios.delete("/Api/Nutrition/DeleteIngredientSubstitute" + querystring)
                .then(response => {
                    this.cancelSubstitute();
                    this.getSubstitutes();
                    useToast().success('Substitute deleted successfully.');
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            await this.getRecipeIngredients()
        }
    },
    openSelectSubstitute() {
        this.$data.substituteQuery = null;
        this.selectSubstituteModal.show();
    },
    async searchSubstitutes() {
        if (this.$data.substituteQuery.length > 2) {
            let querystring = '?query=' + this.$data.substituteQuery +
                '&unit=' + this.$data.selectedIngredient.unit;

            await window.axios.get("/Api/Nutrition/SearchSubstitutes" + querystring)
                .then(response => {
                    if (response.status === 200) {
                        this.$data.filteredSubstitutes = response.data;
                    }
                    else {
                        useToast().success(response.statusText);
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        }
        else {
            this.$data.filteredSubstitutes = [];
        }
    },
    selectNewSubstitute(substitute) {
        this.$data.selectedSubstitute = substitute;
        this.$data.substitute = { ...{ title: substitute.title } };
        this.selectSubstituteModal.hide();
    }
};
let dietMethods = {
    async getDiets() {
        let querystring = '?recipeId=' + this.$data.recipe.id;
        await window.axios.get("/Api/Nutrition/GetRecipeDiets" + querystring)
            .then(response => {
                this.$data.diets = response.data;
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async openDietsModal() {
        this.manageDietsModal.show();
        await this.getAllDiets();
    },
    async getAllDiets() {
        this.$data.allDiets = [];
        await window.axios.get("/Api/Nutrition/GetAllDiets")
            .then(async response => {
                this.$data.allDiets = response.data;
                await this.getDiets();
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    isDietExists(diet) {
        return this.$data.diets.filter(p => p.id == diet.id).length > 0;
    },
    async addDietToRecipe(diet) {
        let model = {
            recipeId: this.$data.recipeId,
            dietId: diet.id
        };
        await window.axios.post("/Api/Nutrition/AddDietToRecipe", model)
            .then(async response => {
                await this.getAllDiets();
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async removeDietFromRecipe(diet) {
        let model = {
            recipeId: this.$data.recipeId,
            dietId: diet.id
        };
        await window.axios.post("/Api/Nutrition/RemoveDietFromRecipe", model)
            .then(async response => {
                await this.getAllDiets();
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
};
let customerMethods = {
    openCustomerModal() {
        this.usersModal.hide();
        this.$data.customerQuery = null;
        this.$data.customers = [];
        this.selectCustomerModal.show();
    },
    openOwnersModal() {
        this.usersModal.show();
    },
    async searchUsers() {
        this.$data.loading.customer = true;
        this.$data.customers = [];
        if (this.$data.customerQuery) {
            let querystring = '?query=' + this.$data.customerQuery;

            await window.axios.get("/Api/Customer/Search" + querystring)
                .then(response => {
                    this.$data.customers = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        }
        this.$data.loading.customer = false;
    },
    async selectCustomer(customer) {
        this.$data.customer = customer;
        let querystring = '?recipeId=' + this.$data.recipeId +
            '&ownerId=' + (this.$data.customer ? this.$data.customer.id : null);
        await window.axios.get("/Api/Nutrition/AddRecipeOwner" + querystring)
            .then(async response => {
                if (response.data.succeeded) {
                    await this.getRecipeOwners();
                    this.selectCustomerModal.hide();
                    this.usersModal.show();
                    useToast().success('Owner updated successfully.');
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async getRecipeOwners() {
        let querystring = '?recipeId=' + this.$data.recipeId;
        await window.axios.get("/Api/Nutrition/GetRecipeOwners" + querystring)
            .then(response => {
                this.$data.users = response.data;
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async removeUser(user) {
        let querystring = '?recipeId=' + this.$data.recipeId +
            '&ownerId=' + user.id;
        await window.axios.get("/Api/Nutrition/RemoveRecipeOwner" + querystring)
            .then(async response => {
                if (response.data.succeeded) {
                    await this.getRecipeOwners();
                    useToast().success('Owner removed successfully.');
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
};
let tagMethods = {
    async getTags() {
        let querystring = '?recipeId=' + this.$data.recipe.id;
        await window.axios.get("/Api/Nutrition/GetRecipeTags" + querystring)
            .then(response => {
                this.$data.tags = response.data;
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async openTagsModal() {
        this.manageTagsModal.show();
        await this.getAllTags();
    },
    async getAllTags() {
        this.$data.allTags = [];
        await window.axios.get("/Api/Nutrition/GetAllTags")
            .then(async response => {
                this.$data.allTags = response.data;
                await this.getTags();
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    isTagExists(tag) {
        return this.$data.tags.filter(p => p.id == tag.id).length > 0;
    },
    async addTagToRecipe(tag) {
        let model = {
            recipeId: this.$data.recipeId,
            tagId: tag.id
        };
        await window.axios.post("/Api/Nutrition/AddTagToRecipe", model)
            .then(async response => {
                await this.getAllTags();
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    async removeTagFromRecipe(tag) {
        debugger;
        let model = {
            recipeId: this.$data.recipeId,
            tagId: tag.id
        };
        await window.axios.post("/Api/Nutrition/RemoveTagFromRecipe", model)
            .then(async response => {
                await this.getAllTags();
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
};
let stripeMethods = {
    openRecipeStripeModal() {
        let images = [];
        for (var i = 0; i < this.$data.recipe.food.attachments.length; i++) {
            images.push('https://app.saltstackers.com/Uploads/Food/' + this.$data.recipe.foodId + '/' + this.$data.recipe.food.attachments[i].fileName)
        }
        this.$data.recipeStripe = {
            id: this.$data.recipe.stripeId,
            name: this.$data.recipe.food.title + ' (' + this.$data.recipe.title + ')',
            description: this.$data.recipe.description,
            url: 'https://app.saltstackers.com/meals/' + this.$data.recipe.code,
            images: images
        };
        this.recipeStripeModal.show();
    },
    async sendRecipeToStripe() {
        let querystring = '?code=' + this.$data.recipe.code;
        await window.axios.get("/Api/Financial/SendRecipeToStripe" + querystring)
            .then(response => {
                if (response.data.succeeded) {
                    useToast().success('Stripe updated successfully.');
                    this.getRecipeDetails();
                    this.recipeStripeModal.hide();
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
}

const config = {
    data() {
        return {
            customerQuery: null,
            customers: [],
            loading: {
                customer: false,
                createOrder: false,
                priceChart: true
            },
            foodId: null,
            recipeId: null,
            recipe: null,
            nutritionFacts: null,
            ingredients: [],
            overheads: [],
            selectedIngredient: null,
            kitchens: [],
            otherAmounts: [],
            otherAmount: {
                id: 0,
                amount: null,
                processFee: null
            },
            substitutes: [],
            substitute: {
                id: 0,
                amount: null,
                processFee: null
            },
            allDiets: [],
            diets: [],
            allTags: [],
            tags: [],
            allIngredients: [],
            substituteQuery: null,
            filteredSubstitutes: [],
            selectedSubstitute: null,
            recipeStripe: null,
            remarks: [],
            users: [],
            priceChart: null,
            priceChartData: [],
            foodsQuery: null,
            foods: [],
            allergenAllerts: []
        }
    },
    computed: {
        subtotal() {
            let sumIngredients = this.$data.ingredients.filter(p => !p.isAddOn).reduce(function (a, b) { return a + b['partialPrice']; }, 0);
            return sumIngredients;
        },
        total() {
            return this.subtotal + this.overheads.reduce(function (a, b) { return a + b['amount']; }, 0);
        },
        profitMargin() {
            return this.total * this.$data.recipe.food.profitMargin / 100;
        },
        totalPrice() {
            return this.total + this.profitMargin;
        }
    },
    async mounted() {
        this.$data.foodId = parseInt(this.$refs.foodId.value);
        this.$data.recipeId = parseInt(this.$refs.recipeId.value);
        this.editRecipeModal = new bootstrap.Modal(this.$refs.editRecipeModal, { backdrop: false });
        this.manageAmountsModal = new bootstrap.Modal(this.$refs.manageAmountsModal, { backdrop: false });
        this.manageSubstitutesModal = new bootstrap.Modal(this.$refs.manageSubstitutesModal, { backdrop: false });
        this.selectSubstituteModal = new bootstrap.Modal(this.$refs.selectSubstituteModal, { backdrop: false });
        this.ingredientOrderModal = new bootstrap.Modal(this.$refs.ingredientOrderModal, { backdrop: false });
        this.manageDietsModal = new bootstrap.Modal(this.$refs.manageDietsModal, { backdrop: false });
        this.manageTagsModal = new bootstrap.Modal(this.$refs.manageTagsModal, { backdrop: false });
        this.selectCustomerModal = new bootstrap.Modal(this.$refs.selectCustomerModal, { backdrop: false });
        this.recipeStripeModal = new bootstrap.Modal(this.$refs.recipeStripeModal, { backdrop: false });
        this.usersModal = new bootstrap.Modal(this.$refs.usersModal, { backdrop: false });
        this.priceChartModal = new bootstrap.Modal(this.$refs.priceChartModal, { backdrop: false });
        this.transferFoodModal = new bootstrap.Modal(this.$refs.transferFoodModal, { backdrop: false });

        await this.syncAll();
    },
    methods: {
        async syncAll() {
            await this.getRecipeDetails();
            await this.getRecipeOwners();
            await this.getDiets();
            await this.getRecipeAllergenAllerts();
            await this.getTags();
            await this.getRecipeIngredients();
            await this.getRecipeOverheads();
            await this.getNutritionFacts();
        },
        manageIngredients(ingredients) {
            this.$data.allIngredients = [];
            for (var i = 0; i < ingredients.length; i++) {
                let ingredient = ingredients[i];
                let substitutes = [];
                for (var j = 0; j < ingredient.substitutes.length; j++) {
                    let substitute = ingredient.substitutes[j];
                    substitutes.push({
                        title: substitute.ingredientTypeUnit.ingredientType.displayTitle,
                        processFee: substitute.processFee,
                        ingredientId: substitute.ingredientTypeUnit.ingredientType.ingredientId
                    });
                }
                let amounts = [];
                amounts.push({
                    amount: ingredient.amount,
                    unit: ingredient.ingredientTypeUnit.unit.sign,
                    processFee: 0,
                    isDefault: true
                });
                for (var j = 0; j < ingredient.otherAmounts.length; j++) {
                    let otherAmount = ingredient.otherAmounts[j];
                    amounts.push({
                        amount: otherAmount.amount,
                        unit: ingredient.ingredientTypeUnit.unit.sign,
                        processFee: otherAmount.processFee,
                        isDefault: false
                    });
                }
                amounts.sort((a, b) => { return (a.amount > b.amount) ? 1 : -1 });
                this.$data.allIngredients.push({
                    id: ingredient.id,
                    ingredientId: ingredient.ingredientTypeUnit.ingredientType.ingredientId,
                    order: ingredient.order,
                    title: ingredient.ingredientTypeUnit.ingredientType.displayTitle,
                    isAddOn: ingredient.isAddOn,
                    isDressing: ingredient.isDressing,
                    substitutes: substitutes,
                    amounts: amounts,
                    unit: ingredient.ingredientTypeUnit.unit.sign,
                    price: ingredient.partialPriceFormatted
                });
            }

            this.$data.allIngredients.sort((a, b) => { return (a.order > b.order) ? 1 : -1 });
        },
        openRecipeModal() {
            this.editRecipeModal.show();
        },
        async getRecipeDetails() {
            let querystring = '?id=' + this.$data.recipeId;
            await window.axios.get("/Api/Nutrition/GetRecipeDetails" + querystring)
                .then(response => {
                    this.$data.recipe = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async getRecipeIngredients() {
            let querystring = '?id=' + this.$data.recipeId;
            await window.axios.get("/Api/Nutrition/GetRecipeIngredients" + querystring)
                .then(response => {
                    this.$data.ingredients = response.data;
                    this.manageIngredients(this.$data.ingredients);
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async getRecipeOverheads() {
            let querystring = '?id=' + this.$data.recipeId;
            await window.axios.get("/Api/Nutrition/GetRecipeOverheads" + querystring)
                .then(response => {
                    this.$data.overheads = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async getNutritionFacts() {
            let querystring = '?id=' + this.$data.recipeId;
            await window.axios.get("/Api/Nutrition/GetNutritionFacts" + querystring)
                .then(async response => {
                    this.$data.nutritionFacts = response.data;
                    await this.updatePriceChart();
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openIngredientOrder(ingredient) {
            this.$data.selectedIngredient = { ...ingredient };
            this.ingredientOrderModal.show();
        },
        async changeOrder() {
            let model = {
                id: this.$data.selectedIngredient.id,
                order: this.$data.selectedIngredient.order
            };
            await window.axios.post("/Api/Nutrition/UpdateOrder", model)
                .then(async response => {
                    await this.getRecipeIngredients();
                    this.ingredientOrderModal.hide();
                    this.$data.selectedIngredient = null;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async changeRemark() {
            let model = {
                recipeId: this.$data.recipe.id,
                allowNoSalt: this.$data.recipe.allowNoSalt,
                allowNoPepper: this.$data.recipe.allowNoPepper,
                allowNoAppleCider: this.$data.recipe.allowNoAppleCider,
                allowNoSalmonSkin: this.$data.recipe.allowNoSalmonSkin
            };

            await window.axios.post("/Api/Nutrition/UpdateRecipeRemarks", model)
                .then(async response => {
                    if (response.data.succeeded) {
                        useToast().success('Remark updated');
                    }
                    else {
                        useToast().error(response.data.errors[0].description);
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async updateRecipeVariables() {
            let querystring = '?recipeId=' + this.$data.recipe.id;
            await window.axios.get("/Api/Nutrition/UpdateRecipeVariables" + querystring)
                .then(async response => {
                    if (response.data.succeeded) {
                        await this.getNutritionFacts();
                    }
                    else {
                        useToast().error('Something went wrong!');
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openPriceChartModal() {
            this.priceChartModal.show();
            this.updatePriceChart();
        },
        updatePriceChart() {
            this.$data.loading.priceChart = true;

            if (this.$data.priceChart) {
                this.$data.priceChart.destroy();
            }

            let labels = [];
            let values = [];
            labels.push('Ingredients');
            values.push(this.subtotal.toFixed(2));

            for (var i = 0; i < this.$data.overheads.length; i++) {
                labels.push(this.$data.overheads[i].overheadCostTitle);
                values.push(this.$data.overheads[i].amount.toFixed(2));
            }

            labels.push('Gross Margin');
            values.push(this.profitMargin.toFixed(2));
            
            const data = {
                labels: labels,
                datasets: [
                    {
                        data: values,
                        backgroundColor: ['#F44336', '#FF9800', '#3F51B5', '#9C27B0', '#00BCD4', '#009688', '#CDDC39',
                            '#FF5722', '#E91E63', '#795548', '#FFC107', '#03A9F4', '#673AB7', '#607D8B', '#FFEB3B']
                    }
                ]
            };

            const ctx = document.getElementById('price-chart');

            this.$data.priceChart = new Chart(ctx, {
                type: 'pie',
                data: data,
                options: {
                    maintainAspectRatio: false,
                    responsive: true,
                    plugins: {
                        legend: {
                            display: true,
                            position: 'left'
                        },
                        title: {
                            display: false
                        }
                    }
                }
            });
            this.$data.priceChart.resize(null, 180);
        },
        openTransferModal() {
            this.transferFoodModal.show();
        },
        async searchFoods() {
            let querystring = '?query=' + this.$data.foodsQuery;
            await window.axios.get("/Api/Nutrition/SearchFoods" + querystring)
                .then(response => {
                    this.$data.foods = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async transferFood(food) {
            if (confirm("Are you sure to change the food? The page will be reloaded.")) {
                let model = {
                    recipeId: this.$data.recipeId,
                    foodId: food.id
                };
                await window.axios.post("/Api/Nutrition/TransferFood", model)
                    .then(async response => {
                        if (response.status === 200) {
                            window.location.reload(true);
                        }
                        else {
                            useToast().success(response.statusText);
                        }
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        async getRecipeAllergenAllerts() {
            let querystring = '?recipeId=' + this.$data.recipe.id;
            await window.axios.get("/Api/Nutrition/GetRecipeAllergenAllerts" + querystring)
                .then(response => {
                    this.$data.allergenAllerts = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        ...amountMethods,
        ...substituteMethods,
        ...dietMethods,
        ...customerMethods,
        ...tagMethods,
        ...stripeMethods
    }
};

const app = window.createApp(config);

const options = {
    position: "bottom-right"
};

app.use(Toast, options);

app.mount('#app');