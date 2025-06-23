import Toast, { useToast } from "vue-toastification";

const config = {
    data() {
        return {
            foodId: null,
            dayOfWeek: null
        }
    },
    mounted() {
        this.$data.kitchenId = parseInt(this.$refs.kitchenId.value);
        this.$data.dayOfWeek = parseInt(this.$refs.dayOfWeek.value);
    },
    methods: {
        async assignDay(foodId, event) {
            let action = event.target.checked ? 'AddKitchenCookingDay' : 'RemoveKitchenCookingDay';
            let querystring = '?kitchenId=' + this.$data.kitchenId +
                '&dayOfWeek=' + this.$data.dayOfWeek +
                '&foodId=' + foodId;
            await window.axios.get("/Api/Nutrition/" + action + querystring)
                .then(response => {
                    useToast().success("Updated successfully");
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