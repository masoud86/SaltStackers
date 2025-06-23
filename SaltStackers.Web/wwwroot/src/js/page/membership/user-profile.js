import CKEditor from "@ckeditor/ckeditor5-vue";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import Toast, { useToast } from "vue-toastification";
import Uppy from '@uppy/core';
import DragDrop from '@uppy/drag-drop';
import ThumbnailGenerator from '@uppy/thumbnail-generator';
import StatusBar from '@uppy/status-bar';
import ProgressBar from '@uppy/progress-bar';
import XHR from '@uppy/xhr-upload';

import '@uppy/core/dist/style.min.css';
import '@uppy/drag-drop/dist/style.min.css';
import '@uppy/status-bar/dist/style.min.css';
import '@uppy/progress-bar/dist/style.min.css';



let debtMethods = {
    async getDebts() {
        this.$data.loading.debt = true;
        let querystring = '?id=' + this.$data.userId;
        await window.axios.get("/Api/Membership/GetUserDebts" + querystring)
            .then(response => {
                if (response.status === 200) {
                    this.$data.debts = response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });

        this.$data.loading.debt = false;
    },
    async deleteDebt(id) {
        if (confirm("Do you really want to delete?")) {
            let querystring = '?id=' + id;
            await window.axios.delete("/Api/Membership/DeleteUserDebt" + querystring)
                .then(async response => {
                    if (response.status === 200) {
                        await this.getDebts();
                    }
                    else {
                        useToast().error(response.statusText);
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        }
    },
    openAddUserDebtModal() {
        this.$data.debt = {
            title: null,
            amount: null
        };
        this.addUserDebtModal.show();
    },
    async addNewUserDebt() {
        if (this.$data.debt.title && this.$data.debt.amount) {
            this.$data.debt.userId = this.$data.userId;
            await window.axios.post("/Api/Membership/AddNewUserDebt", this.$data.debt)
                .then(async response => {
                    if (response.status === 200) {
                        this.addUserDebtModal.hide();
                        await this.getDebts();
                    }
                    else {
                        useToast().error(response.statusText);
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });
        }
        else {
            useToast().error('title or amount is not valid');
        }
    }
};
let userProfile = {
    async getUserProfile() {
        this.$data.loading.userDetails = true;
        let querystring = '?id=' + this.$data.userId;
        await window.axios.get("/Api/Membership/GetUserProfile" + querystring)
            .then(response => {
                if (response.status === 200) {
                    this.$data.user = response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });

        this.$data.loading.userDetails = false;
    },
    openEditUsernameModal() {
        this.$data.username = this.$data.user.username;
        this.editUsernameModal.show();
    },
    async updateUsername() {
        let model = {
            id: this.$data.userId,
            username: this.$data.username
        };
        await window.axios.post("/Api/Membership/EditUsername", model)
            .then(response => {
                if (response.status === 200) {
                    this.editUsernameModal.hide();
                    useToast().success('Username updated successfully');
                    this.getUserProfile();
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    },
    openAboutModal() {
        this.$data.aboutTemp = this.$data.user.about;
        this.aboutModal.show();
    },
    async editAbout() {
        let model = {
            id: this.$data.userId,
            about: this.$data.aboutTemp
        };
        await window.axios.post("/Api/Membership/EditAbout", model)
            .then(response => {
                if (response.status === 200) {
                    this.aboutModal.hide();
                    useToast().success('About updated successfully');
                    this.getUserProfile();
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
}
let invoices = {
    async getInvoices() {
        this.$data.loading.invoices = true;
        let querystring = '?ownerId=' + this.$data.userId;
        await window.axios.get("/Api/Financial/GetInvoices" + querystring)
            .then(response => {
                if (response.status === 200) {
                    this.$data.invoices = response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });

        this.$data.loading.invoices = false;
    },
    async openCreateInvoiceModal() {
        this.createInvoiceModal.show();
        await this.getKitchens();
    },
    async getKitchens() {
        this.$data.loading.kitchens = true;
        await window.axios.get("/Api/Operation/GetKitchens")
            .then(response => {
                if (response.status === 200) {
                    this.$data.kitchens = response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
        this.$data.loading.kitchens = false;
    },
    async createInvoice(kitchenId) {
        let model = {
            userId: this.$data.userId,
            kitchenId: kitchenId
        };
        await window.axios.post("/Api/Financial/CreateInvoice", model)
            .then(response => {
                if (response.status === 200) {
                    useToast().success('Invoice created, wait to redirect!');
                    window.location = '/financial/invoice/details/' + response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
};
let wallet = {
    async getWalletBalance() {
        this.$data.loading.wallet = true;
        let querystring = '?userId=' + this.$data.userId;
        await window.axios.get("/Api/Financial/GetWalletBalance" + querystring)
            .then(response => {
                if (response.status === 200) {
                    this.$data.walletBalance = response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });

        this.$data.loading.wallet = false;
    }
};
let files = {
    openUploader() {
        if (this.$data.uploader) {
            this.$data.uploader.close();
        }
        this.$data.uploader = new Uppy()
            .use(ThumbnailGenerator)
            .use(DragDrop, { target: '#drag-drop' })
            .use(StatusBar, { target: '#status-bar' })
            .use(ProgressBar, { target: '#progress-bar' })
            .use(XHR, { endpoint: '/Api/User/UploadFile', headers: { userId: this.$data.userId } })
            .on('thumbnail:generated', (file, preview) => doSomething(file, preview));
        this.$data.uploader.on("complete", (result) => {
            if (result.failed.length === 0) {
                this.getUserFiles();
                useToast().success('Uploaded successfully');
            } else {
                useToast().error('Upload error');
            }
        });
        this.uploadFileModal.show();
    },
    async getUserFiles() {
        this.$data.loading.files = true;
        let querystring = '?userId=' + this.$data.userId;
        await window.axios.get("/Api/User/GetUserFiles" + querystring)
            .then(response => {
                if (response.status === 200) {
                    this.$data.userFiles = response.data;
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });

        this.$data.loading.files = false;
    }
};
let roles = {
    async openSwitchRoleModal() {
        if (this.$data.roles.length === 0) {
            this.$data.loading.roles = true;
            await window.axios.get("/Api/Membership/GetRoles")
                .then(response => {
                    if (response.status === 200) {
                        this.$data.roles = response.data.items;
                    }
                    else {
                        useToast().error(response.statusText);
                    }
                })
                .catch(err => {
                    useToast().error(err.message);
                    console.log(err.message);
                });

            this.$data.loading.roles = false;
        }
        this.switchRoleModal.show();
    },
    async switchRole(role) {
        let model = {
            userId: this.$data.userId,
            roleName: role.name
        };
        await window.axios.post("/Api/Membership/SwitchRole", model)
            .then(response => {
                if (response.status === 200) {
                    this.switchRoleModal.hide();
                    useToast().success('The role updated successfully');
                    this.getUserProfile();
                }
                else {
                    useToast().error(response.statusText);
                }
            })
            .catch(err => {
                useToast().error(err.message);
                console.log(err.message);
            });
    }
};

const config = {
    data() {
        return {
            loading: {
                userDetails: true,
                debt: true,
                invoices: true,
                wallet: true,
                files: true,
                roles: false,
                kitchens: true
            },
            editor: ClassicEditor,
            editorConfig: {},
            userId: null,
            user: null,
            debts: [],
            debt: {
                title: null,
                amount: null
            },
            username: null,
            invoices: [],
            walletBalance: null,
            uploader: null,
            userFiles: [],
            roles: [],
            kitchens: null,
            aboutTemp: null
        }
    },
    computed: {
        totalDebt() { return this.$data.debts.filter(p => !p.isPaid).reduce(function (a, b) { return a + b['amount']; }, 0) },
    },
    async mounted() {
        this.$data.userId = this.$refs.userId.value;
        this.addUserDebtModal = new bootstrap.Modal(this.$refs.addUserDebtModal, { backdrop: false });
        this.editUsernameModal = new bootstrap.Modal(this.$refs.editUsernameModal, { backdrop: false });
        this.uploadFileModal = new bootstrap.Modal(this.$refs.uploadFileModal, { backdrop: false });
        this.switchRoleModal = new bootstrap.Modal(this.$refs.switchRoleModal, { backdrop: false });
        this.createInvoiceModal = new bootstrap.Modal(this.$refs.createInvoiceModal, { backdrop: false });
        this.aboutModal = new bootstrap.Modal(this.$refs.aboutModal, { backdrop: false });
        this.getUserProfile();
        this.getDebts();
        this.getInvoices();
        this.getWalletBalance();
        this.getUserFiles();
    },
    methods: {
        ...userProfile,
        ...debtMethods,
        ...invoices,
        ...wallet,
        ...files,
        ...roles
    }
};

const app = window.createApp(config);

app.use(CKEditor);

const options = {
    position: "bottom-right"
};

app.use(Toast, options);

app.mount('#app');