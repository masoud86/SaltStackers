import { useLoading } from 'vue3-loading-overlay';
import 'vue3-loading-overlay/dist/vue3-loading-overlay.css';
//window.useLoading = useLoading;

window.showLoading = function (container) {
    debugger;
    let loader = useLoading();
    loader.show({
        container: container,
        color: '#34558b',
        loader: 'bars',
        opacity: 0.6,
        zIndex: 99999
    });
    return loader;
};