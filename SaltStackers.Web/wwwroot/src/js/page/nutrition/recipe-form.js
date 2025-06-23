import CKEditor from '@ckeditor/ckeditor5-vue';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';

const config = {
    data() {
        return {
            editor: ClassicEditor,
            editorConfig: {},
            recipeDetails: null
        }
    },
    mounted() {
        this.$data.recipeDetails = this.$refs.recipeDetailsTemp.value;
    },
    methods: {
        updateRecipeDetails() {
            document.getElementById('RecipeDetails').value = this.$data.recipeDetails;
            this.$refs.mainForm.submit();
        }
    }
};

const app = window.createApp(config);

app.use(CKEditor);

app.mount('#app');