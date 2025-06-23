let mix = require('laravel-mix');
const webpack = require('webpack');

mix
    //.copyDirectory("node_modules/@fortawesome/fontawesome-free/webfonts", "src/assets/fonts")
    .js('src/js/main.js', 'dist/js/main.js')
    .js('src/js/public.js', 'dist/js/public.js')
    .js('src/js/page/dashboard.js', 'dist/js/page/dashboard.js')
    .js('src/js/page/nutrition/recipe-details.js', 'dist/js/page/nutrition/recipe-details.js')
    .js('src/js/page/nutrition/recipe-form.js', 'dist/js/page/nutrition/recipe-form.js')
    .js('src/js/page/nutrition/ingredient-manage.js', 'dist/js/page/nutrition/ingredient-manage.js')
    .js('src/js/page/nutrition/package-edit.js', 'dist/js/page/nutrition/package-edit.js')
    .js('src/js/page/operation/kitchen-cooking-days.js', 'dist/js/page/operation/kitchen-cooking-days.js')
    .js('src/js/page/operation/kitchen-details.js', 'dist/js/page/operation/kitchen-details.js')
    .js('src/js/page/configuration/recurring-jobs.js', 'dist/js/page/configuration/recurring-jobs.js')
    .js('src/js/page/membership/user-profile.js', 'dist/js/page/membership/user-profile.js')
    .autoload({
        jquery: ['$', 'window.jQuery']
    })
    .sass('src/css/main.scss', 'dist/css/main.css')
    .sass('src/css/public.scss', 'dist/css/public.css')
    .sass('src/css/bootstrap.scss', 'dist/css/bootstrap.css')
    .webpackConfig({
        stats: {
            children: false
        },
        plugins: [
            new webpack.DefinePlugin({
                __VUE_PROD_DEVTOOLS__: JSON.stringify(false)
            })
        ]
    })
    .options({
        processCssUrls: false
    })
    .copyDirectory("src/assets/fonts", "dist/assets/fonts")
    .copyDirectory("src/assets/images", "dist/assets/images");