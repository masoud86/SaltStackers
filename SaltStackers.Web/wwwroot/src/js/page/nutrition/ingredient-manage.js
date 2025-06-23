import Toast, { useToast } from "vue-toastification";

function groupBy(array, key) {
    const result = {}
    array.forEach(item => {
        if (!result[item[key]]) {
            result[item[key]] = []
        }
        result[item[key]].push(item)
    })
    return result
}

const config = {
    data() {
        return {
            typeTitle: null,
            basePrice: null,
            ingredientId: null,
            ingredientType: null,
            ingredientTitle: null,
            ingredientTypeUnit: null,
            loading: {
                ingredientTypeUnit: false
            },
            units: [],
            ingredientTypes: [],
            ingredientCategories: [],
            ingredientSubCategories: [],
            ingredientCategory: null,
            ingredientSubCategory: null,
        }
    },
    computed: {
        unitsGroup() {
            return groupBy(this.$data.units, 'category');
        },
        ingredientTypeUnitTotal() {
            if (this.$data.ingredientTypeUnit) {
                let basePrice = parseFloat(this.$data.ingredientType.basePrice);
                let priceFactor = parseFloat(this.$data.ingredientTypeUnit.priceFactor);
                if (this.$data.ingredientTypeUnit.isPercent) {
                    priceFactor = (basePrice * priceFactor) / 100;
                }
                switch (this.$data.ingredientTypeUnit.priceOperator) {
                    case 'x':
                        return parseFloat(basePrice * priceFactor).toFixed(4);
                    case '/':
                        return parseFloat(basePrice / priceFactor).toFixed(4);
                    case '+':
                        return parseFloat(basePrice + priceFactor).toFixed(4);
                    case '-':
                        return parseFloat(basePrice - priceFactor).toFixed(4);
                    default:
                        return 0;
                }
            }

            return 0;
        },
        basePriceFormat: {
            get() {
                if (this.$data.ingredientType && this.$data.ingredientType.basePrice) {
                    //return this.$data.ingredientType.basePrice.toFixed(4);
                    return this.$data.ingredientType.basePrice;
                }
                return 0;
            },
            set(newValue) {
                this.$data.ingredientType.basePrice = Number(newValue);
            }
        }
    },
    mounted() {
        const interval = setInterval(() => {
            if (this.$refs.ingredientId) {
                this.$data.ingredientId = parseInt(this.$refs.ingredientId.value);
                this.$data.ingredientTitle = this.$refs.ingredientTitle.value;

                this.getIngredientTypes();
                this.getAllUnits();

                this.ingredientTypeModal = new bootstrap.Modal(this.$refs.ingredientTypeModal, { backdrop: false });
                this.ingredientTypeUnitModal = new bootstrap.Modal(this.$refs.ingredientTypeUnitModal, { backdrop: false });
                this.addIngredientTypeCategoryModal = new bootstrap.Modal(this.$refs.addIngredientTypeCategoryModal, { backdrop: false });

                clearInterval(interval)
            }
        }, 50);
    },
    methods: {
        async deleteIngredientTypeUnit(ingredientTypeUnitId) {
            if (confirm("Do you really want to delete?")) {
                this.$data.loading.ingredientTypeUnit = true;
                let querystring = '?ingredientTypeUnitId=' + ingredientTypeUnitId;

                await window.axios.delete("/Api/Nutrition/DeleteIngredientTypeUnit" + querystring)
                    .then(response => {
                        if (response.data.succeeded) {
                            this.refreshModel();
                            useToast().success("Unit deleted successfully");
                        }
                        else {
                            response.data.errors.forEach(function (error) {
                                useToast().error(error.description);
                            });
                        }
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
                this.$data.loading.ingredientTypeUnit = false;
            }
        },
        async deleteIngredientType(ingredientTypeId) {
            if (confirm("Do you really want to delete?")) {
                let querystring = '?ingredientTypeId=' + ingredientTypeId;

                await window.axios.delete("/Api/Nutrition/DeleteIngredientType" + querystring)
                    .then(response => {
                        if (response.data.succeeded) {
                            this.refreshModel();
                            useToast().success("Type deleted successfully");
                        }
                        else {
                            response.data.errors.forEach(function (error) {
                                useToast().error(error.description);
                            });
                        }
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        async getIngredientTypes() {
            let querystring = '?ingredientId=' + this.$data.ingredientId;

            await window.axios.get("/Api/Nutrition/GetIngredientTypes" + querystring)
                .then(response => {
                    this.$data.ingredientTypes = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async addNewTypeUnit() {
            this.$data.loading.ingredientTypeUnit = true;
            this.$data.ingredientTypeUnit.unitId = this.$data.ingredientTypeUnit.unitId;
            this.$data.ingredientTypeUnit.unit = null;
            this.$data.ingredientTypeUnit.units = [];
            this.$data.ingredientTypeUnit.ingredientTypeId = this.$data.ingredientType.id;
            await window.axios.post("/Api/Nutrition/CreateIngredientTypeUnit", this.$data.ingredientTypeUnit)
                .then(response => {
                    if (response.data.succeeded) {
                        this.refreshModel();
                        useToast().success("Type added successfully");
                    }
                    else {
                        response.data.errors.forEach(function (error) {
                            useToast().error(error.description);
                        });
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            this.$data.loading.ingredientTypeUnit = false;
        },
        async editTypeUnit() {
            this.$data.loading.ingredientTypeUnit = true;
            await window.axios.post("/Api/Nutrition/EditIngredientTypeUnit", this.$data.ingredientTypeUnit)
                .then(response => {
                    if (response.data.succeeded) {
                        this.refreshModel();
                        useToast().success("Unit edited successfully");
                    }
                    else {
                        response.data.errors.forEach(function (error) {
                            useToast().error(error.description);
                        });
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            this.$data.loading.ingredientTypeUnit = false;
        },
        async refreshModel() {
            await this.getIngredientTypes();
            this.ingredientTypeModal.hide();
            this.addIngredientTypeCategoryModal.hide();
            this.ingredientTypeUnitModal.hide();
            this.$data.ingredientType = null;
            this.$data.ingredientTypeUnit = null;
        },
        async getAllUnits() {
            await window.axios.get("/Api/Nutrition/GetAllUnits")
                .then(response => {
                    this.$data.units = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async addNewType() {
            let model = {
                ingredientId: this.$data.ingredientId,
                title: this.$data.ingredientType.title,
                displayTitle: this.$data.ingredientType.displayTitle,
                basePrice: this.$data.ingredientType.basePrice,
                mixDescription: this.$data.ingredientType.mixDescription,
                needsPrep: this.$data.ingredientType.needsPrep,
                pchef: this.$data.ingredientType.pchef,
                allergens: this.$data.ingredientType.allergens
            };

            await window.axios.post("/Api/Nutrition/CreateIngredientType", model)
                .then(response => {
                    if (response.data.succeeded) {
                        this.refreshModel();
                        useToast().success("Type added successfully");
                    }
                    else {
                        response.data.errors.forEach(function (error) {
                            useToast().error(error.description);
                        });
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async editType() {
            await window.axios.post("/Api/Nutrition/EditIngredientType", this.$data.ingredientType)
                .then(response => {
                    if (response.data.succeeded) {
                        this.refreshModel();
                        useToast().success("Type edited successfully");
                    }
                    else {
                        response.data.errors.forEach(function (error) {
                            useToast().error(error.description);
                        });
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openNewIngredientType() {
            this.$data.ingredientType = {
                title: null,
                allergens: []
            };
            this.ingredientTypeModal.show();
        },
        formulaFormat(ingredientTypeUnit, ingredientType) {
            return ingredientType.basePrice.toFixed(2) + ' ' +
                ingredientTypeUnit.priceOperator + ' ' +
                ingredientTypeUnit.priceFactor + (ingredientTypeUnit.isPercent ? '%' : '') + ' = ' +
                this.calculatePrice(ingredientTypeUnit, ingredientType);
        },
        calculatePrice(ingredientTypeUnit, ingredientType) {
            let priceFactor = parseFloat(ingredientTypeUnit.priceFactor);
            let basePrice = ingredientType.basePrice;
            if (ingredientTypeUnit.isPercent) {
                priceFactor = (basePrice * priceFactor) / 100;
            }
            switch (ingredientTypeUnit.priceOperator) {
                case 'x':
                    return parseFloat(basePrice * priceFactor).toFixed(4);
                case '/':
                    return parseFloat(basePrice / priceFactor).toFixed(4);
                case '+':
                    return parseFloat(basePrice + priceFactor).toFixed(4);
                case '-':
                    return parseFloat(basePrice - priceFactor).toFixed(4);
                default:
                    return 0.0000;
            }
        },
        openEditIngredientType(ingredientType) {
            this.$data.ingredientType = { ...ingredientType };
            this.ingredientTypeModal.show();
        },
        openNewIngredientTypeUnit(ingredientType) {
            this.$data.ingredientType = ingredientType;
            this.$data.ingredientTypeUnit = {
                amountOperator: '+',
                priceOperator: '+',
                amountFactor: 0,
                priceFactor: 0,
                total: 0
            };
            this.ingredientTypeUnitModal.show();
        },
        openEditIngredientTypeUnit(ingredientTypeUnit, ingredientType) {
            this.$data.ingredientType = ingredientType;
            this.$data.ingredientTypeUnit = { ...ingredientTypeUnit };
            this.ingredientTypeUnitModal.show();
        },
        updateDisplayTitle() {
            let display = '';
            if (this.$data.ingredientType && this.$data.ingredientType.title) {
                display = this.$data.ingredientType.title.trim() + ' ' + this.$data.ingredientTitle;
            }
            this.$data.ingredientType.displayTitle = display;
        },
        changeUnit() {
            if (this.$data.ingredientTypeUnit && this.$data.ingredientTypeUnit.unitId) {
                this.$data.ingredientTypeUnit.unit = this.$data.units
                    .filter(p => p.id == this.$data.ingredientTypeUnit.unitId)[0];
            }
        },
        async removeIngredientTypeSubCategory(id) {
            if (confirm("Do you really want to delete?")) {
                let querystring = '?id=' + id;

                await window.axios.delete("/Api/Nutrition/DeleteIngredientTypeSubCategory" + querystring)
                    .then(response => {
                        if (response.data.succeeded) {
                            this.refreshModel();
                            useToast().success("Category deleted successfully");
                        }
                        else {
                            response.data.errors.forEach(function (error) {
                                useToast().error(error.description);
                            });
                        }
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        async openIngredientTypeSubCategory(ingredientType) {
            this.$data.ingredientType = ingredientType;
            await window.axios.get("/Api/Nutrition/GetIngredientCategories")
                .then(response => {
                    this.$data.ingredientCategories = response.data;
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
            this.addIngredientTypeCategoryModal.show();
        },
        changeCategory() {
            this.$data.ingredientSubCategories = this.$data.ingredientCategory.ingredientSubCategories;
        },
        async addNewCategory() {
            let model = {
                ingredientTypeId: this.$data.ingredientType.id,
                ingredientSubCategoryId: this.$data.ingredientSubCategory.id
            };

            await window.axios.post("/Api/Nutrition/CreateIngredientTypeSubCategory", model)
                .then(response => {
                    if (response.data.succeeded) {
                        this.refreshModel();
                        useToast().success("Category added successfully");
                    }
                    else {
                        response.data.errors.forEach(function (error) {
                            useToast().error(error.description);
                        });
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        } 
    }
};

const app = window.createApp(config);

const options = {
    position: "bottom-right"
};

app.use(Toast, options);

app.mount('#app');