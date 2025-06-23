import Masonry from 'masonry-layout';
import Toast, { useToast } from "vue-toastification";
import Chart from 'chart.js/auto';
import 'chartjs-adapter-moment';

const config = {
    data() {
        return {
            loading: {
                incomeChart: true
            },
            transactions: [],
            transactionPeriod: 7,
            totalIncome: 0,
            incomeChart: null
        }
    },
    computed: {
    },
    async mounted() {
    },
    methods: {
    }
};

const app = window.createApp(config);

//app.component('loading', {
//    props: ['show'],
//    template: '#loading-template'
//});

app.use(Toast, {
    position: "bottom-right"
});

app.mount('#app');