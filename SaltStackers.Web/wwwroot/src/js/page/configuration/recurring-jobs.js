import Toast, { useToast } from "vue-toastification";

const config = {
    methods: {
        async updatePrices() {
            if (confirm("Do you really want to add or update job?")) {
                await window.axios.post("/Api/Configuration/UpdatePrices")
                    .then(response => {
                        useToast().success("Job created successfully");
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        async calculateRecipes() {
            if (confirm("Do you really want to add or update job?")) {
                await window.axios.post("/Api/Configuration/CalculateRecipes")
                    .then(response => {
                        useToast().success("Job created successfully");
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        async updateOrderWeek() {
            if (confirm("Do you really want to add or update job?")) {
                await window.axios.post("/Api/Configuration/UpdateOrderWeek")
                    .then(response => {
                        useToast().success("Job created successfully");
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        },
        async updateOrdersStatus() {
            if (confirm("Do you really want to add or update job?")) {
                await window.axios.post("/Api/Configuration/UpdateOrdersStatus")
                    .then(response => {
                        useToast().success("Job created successfully");
                    })
                    .catch(err => {
                        useToast().error(err.message);
                        console.log(err.message);
                    });
            }
        }
    }
};

const app = window.createApp(config);

const options = {
    position: "bottom-right"
};

app.use(Toast, options);

app.mount('#app');