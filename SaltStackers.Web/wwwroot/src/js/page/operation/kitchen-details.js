import Toast, { useToast } from "vue-toastification";
import Chart from 'chart.js/auto';
import 'chartjs-adapter-moment';

const config = {
    data() {
        return {
            loading: {
                activeDays: true,
                cookingDays: true,
                currentRecipes: true,
                recipes: false
            },
            kitchenId: null,
            activeCookingDays: [],
            cookingDays: [],
            cookingDaysTemp: [],
            reports: [],
            currentRecipes: [],
            currentRecipesQuery: null,
            recipes: [],
            recipesQuery: null,
            selectedRecipe: null,
            selectedCookingDays: []
        }
    },
    async mounted() {
        this.$data.kitchenId = parseInt(this.$refs.kitchenId.value);
        this.addRecipeModal = new bootstrap.Modal(this.$refs.addRecipeModal, { backdrop: false });
        this.recipeCookingDaysModal = new bootstrap.Modal(this.$refs.recipeCookingDaysModal, { backdrop: false });

        this.getCookingDays();
        this.getActiveCookingDays();
        this.getCurrentRecipes();
    },
    computed: {
        filteredCurrentRecipes() {
            if (!this.$data.currentRecipesQuery) {
                return this.$data.currentRecipes;
            }
            
            return this.$data.currentRecipes.filter(kitchen => {
                return kitchen.recipe.food.title.toLowerCase().includes(this.$data.currentRecipesQuery.toLowerCase().trim()) ||
                    (kitchen.recipe.title && kitchen.recipe.title.toLowerCase().includes(this.$data.currentRecipesQuery.toLowerCase().trim()));
            });
        }
    },
    methods: {
        async getActiveCookingDays() {
            this.$data.loading.activeDays = true;
            let querystring = '?kitchenId=' + this.$data.kitchenId;
            await window.axios.get("/Api/Order/GetActiveCookingDays" + querystring)
                .then(response => {
                    this.$data.activeCookingDays = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            this.$data.loading.activeDays = false;
        },
        async getCookingDays() {
            this.$data.loading.cookingDays = true;
            let querystring = '?kitchenId=' + this.$data.kitchenId;
            await window.axios.get("/Api/Operation/GetPrepDays" + querystring)
                .then(response => {
                    this.$data.cookingDays = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            this.$data.loading.cookingDays = false;
        },
        async getCurrentRecipes() {
            this.$data.loading.currentRecipes = true;
            let querystring = '?kitchenId=' + this.$data.kitchenId;
            await window.axios.get("/Api/Operation/GetRecipesByKitchen" + querystring)
                .then(response => {
                    this.$data.currentRecipes = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            this.$data.loading.currentRecipes = false;
        },
        openAddRecipeForm() {
            this.addRecipeModal.show();
        },
        async searchRecipes() {
            if (this.$data.recipesQuery.length > 2) {
                this.$data.loading.recipes = true;
                let querystring = '?query=' + this.$data.recipesQuery;
                await window.axios.get("/Api/Nutrition/SearchRecipes" + querystring)
                    .then(response => {
                        if (response.status === 200) {
                            this.$data.recipes = response.data;
                        }
                        else {
                            useToast().success(response.statusText);
                        }
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
                this.$data.loading.recipes = false;
            }
            else {
                this.$data.recipes = [];
            }
        },
        isCurrentRecipe(item) {
            return this.$data.currentRecipes.some(p => p.recipe.id === item.id);
        },
        async addRecipe(item) {
            let querystring = '?kitchenId=' + this.$data.kitchenId +
                '&recipeId=' + item.id;
            await window.axios.get("/Api/Operation/AddRecipeToKitchen" + querystring)
                .then(async response => {
                    if (response.status === 200) {
                        await this.getCurrentRecipes();
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async removeRecipe(item) {
            if (confirm("Do you really want to delete?")) {
                let querystring = '?kitchenId=' + this.$data.kitchenId +
                    '&recipeId=' + item.id;
                await window.axios.get("/Api/Operation/RemoveRecipeFromKitchen" + querystring)
                    .then(async response => {
                        if (response.status === 200) {
                            await this.getCurrentRecipes();
                        }
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        openRecipeCookingDaysModal(recipe) {
            this.$data.cookingDaysTemp = [];
            this.$data.selectedRecipe = recipe;

            this.$data.cookingDaysTemp = { ...this.$data.cookingDays };
            this.$data.selectedCookingDays = [];

            for (const element of this.$data.cookingDays) {
                if (recipe.recipeCookingDays.some(p => p.kitchenCookingDay.id == element.id)) {
                    this.$data.selectedCookingDays.push(element.id);
                }
            }

            this.recipeCookingDaysModal.show();
        },
        async changeRecipeCookingDay(item) {
            let isRemove = false;
            if (this.$data.selectedCookingDays.some(p => p == item.id)) {
                this.$data.selectedCookingDays.pop(item.id);
                isRemove = true;
            }
            else {
                this.$data.selectedCookingDays.push(item.id);
            }

            let model = {
                recipeId: this.$data.selectedRecipe.id,
                KitchenCookingDayId: item.id,
                isRemove: isRemove
            }
            await window.axios.post("/Api/Operation/AddRemoveRecipeCookingDay", model)
                .then(async response => {
                    if (response.status === 200) {
                        await this.getCurrentRecipes();
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        isChecked(item) {
            return this.$data.selectedCookingDays.some(p => p == item.id);
        }
    }
};

const app = window.createApp(config);

const options = {
    position: "bottom-right"
};

app.use(Toast, options);

app.mount('#app');