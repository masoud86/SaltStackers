import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import Toast, { useToast } from "vue-toastification";

const config = {
    data() {
        return {
            loading: {
                searchRecipes: false,
            },
            packageId: null,
            editor: ClassicEditor,
            editorConfig: {},
            package: null,
            packageTemp: null,
            groupId: null,
            groupTitle: null,
            groupEdit: {
                mode: false,
                id: null,
            },
            recipes: [],
            recipeQuery: null,
            selectedRecipe: null,
            itemLabel: null,
            itemId: null,
            itemEditMode: false,
        };
    },
    async mounted() {
        this.$data.packageId = parseInt(this.$refs.packageId.value);
        this.packageCreateModal = new bootstrap.Modal(
            this.$refs.packageCreateModal,
            { backdrop: false }
        );
        this.groupModal = new bootstrap.Modal(this.$refs.groupModal, {
            backdrop: false,
        });
        this.selectRecipeModal = new bootstrap.Modal(this.$refs.selectRecipeModal, {
            backdrop: false,
        });
        this.groupItemModal = new bootstrap.Modal(this.$refs.groupItemModal, {
            backdrop: false,
        });

        await this.getPackage();
    },
    methods: {
        async getPackage() {
            let querystring = "?id=" + this.$data.packageId;
            await window.axios
                .get("/Api/Nutrition/GetPackage" + querystring)
                .then((response) => {
                    this.$data.package = response.data;
                })
                .catch((err) => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openCreatePackageForm() {
            //document.getElementById("Description").value = this.$data.package.description;
            this.$data.packageTemp = { ...this.$data.package };
            this.packageCreateModal.show();
        },
        async editPackage() {
            let model = {
                id: this.$data.packageTemp.id,
                title: this.$data.packageTemp.title,
                subtitle: this.$data.packageTemp.subtitle,
                description: this.$data.packageTemp.description,
                price: this.$data.packageTemp.price,
                isActive: this.$data.packageTemp.isActive
            };
            await window.axios
                .post("/Api/Nutrition/EditPackage", model)
                .then((response) => {
                    if (response.data.succeeded) {
                        useToast().success("Package updated successfully.");
                        this.getPackage();
                        this.packageCreateModal.hide();
                    } else {
                        useToast().error("Update Error!");
                    }
                })
                .catch((err) => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openAddGroupForm() {
            this.$data.groupTitle = null;
            this.$data.groupEdit = {
                mode: false,
                id: null,
            };
            this.groupModal.show();
        },
        async addGroup() {
            let model = {
                packageId: this.$data.packageId,
                title: this.$data.groupTitle,
            };
            await window.axios
                .post("/Api/Nutrition/AddPackageGroup", model)
                .then((response) => {
                    if (response.data.succeeded) {
                        useToast().success("Group added successfully.");
                        this.getPackage();
                        this.groupModal.hide();
                    } else {
                        useToast().error("Update Error!");
                    }
                })
                .catch((err) => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openEditGroup(groupId) {
            this.$data.groupEditMode = true;
            let group = this.$data.package.groups.find((p) => p.id == groupId);
            this.$data.groupTitle = group.title;
            this.$data.groupEdit = {
                mode: true,
                id: group.id,
            };
            this.groupModal.show();
        },
        async editGroup() {
            let model = {
                id: this.$data.groupEdit.id,
                title: this.$data.groupTitle,
            };
            await window.axios
                .post("/Api/Nutrition/EditPackageGroup", model)
                .then((response) => {
                    if (response.data.succeeded) {
                        useToast().success("Group edited successfully.");
                        this.getPackage();
                        this.groupModal.hide();
                        this.$data.groupEdit = {
                            mode: false,
                            id: null,
                        };
                    } else {
                        useToast().error("Update Error!");
                    }
                })
                .catch((err) => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async searchRecipes() {
            if (this.$data.recipeQuery.length > 2) {
                this.$data.loading.searchRecipes = true;
                let querystring = "?query=" + this.$data.recipeQuery;

                await window.axios
                    .get("/Api/Nutrition/SearchRecipes" + querystring)
                    .then((response) => {
                        if (response.status === 200) {
                            this.$data.recipes = response.data;
                        } else {
                            useToast().success(response.statusText);
                        }
                    })
                    .catch((err) => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
                this.$data.loading.searchRecipes = false;
            } else {
                this.$data.recipes = [];
            }
        },
        openSelectRecipeModal(groupId) {
            this.$data.groupId = groupId;
            this.selectRecipeModal.show();
        },
        selectRecipe(recipe) {
            this.$data.selectedRecipe = recipe;
            this.selectRecipeModal.hide();
            this.groupItemModal.show();
        },
        async addItem() {
            let model = {
                groupId: this.$data.groupId,
                recipeId: this.$data.selectedRecipe.id,
                label: this.$data.itemLabel,
            };
            await window.axios
                .post("/Api/Nutrition/AddPackageGroupItem", model)
                .then((response) => {
                    if (response.data.succeeded) {
                        useToast().success("Item added successfully.");
                        this.getPackage();
                        this.groupItemModal.hide();
                    } else {
                        useToast().error("Update Error!");
                    }
                })
                .catch((err) => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        openEditItem(itemId, groupId) {
            this.$data.itemId = itemId;
            this.$data.itemEditMode = true;
            let group = this.$data.package.groups.find((p) => p.id == groupId);
            let item = group.items.find((p) => p.id == itemId);
            this.$data.selectedRecipe = item.recipe;
            this.$data.itemLabel = item.label;
            this.groupItemModal.show();
        },
        async editItem() {
            let model = {
                id: this.$data.itemId,
                label: this.$data.itemLabel,
            };
            await window.axios
                .post("/Api/Nutrition/EditPackageGroupItem", model)
                .then((response) => {
                    if (response.data.succeeded) {
                        useToast().success("Item edited successfully.");
                        this.getPackage();
                        this.groupItemModal.hide();
                    } else {
                        useToast().error("Update Error!");
                    }
                })
                .catch((err) => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        },
        async deleteItem(itemId) {
            if (confirm("Do you really want to delete?")) {
                await window.axios
                    .delete("/Api/Nutrition/DeletePackageGroupItem?id=" + itemId)
                    .then((response) => {
                        if (response.data.succeeded) {
                            useToast().success("Item deleted successfully.");
                            this.getPackage();
                        } else {
                            useToast().error("Update Error!");
                        }
                    })
                    .catch((err) => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
    },
};

const app = window.createApp(config);

app.use(CKEditor);

const options = {
    position: "bottom-right",
};

app.use(Toast, options);

app.mount("#app");
