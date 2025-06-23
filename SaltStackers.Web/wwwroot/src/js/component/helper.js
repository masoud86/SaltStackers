function getValue(obj, item) {
    return obj[item];
};

helper = {
    groupBy: function (array, key) {
        const result = {}
        array.forEach(item => {
            if (!result[item[key]]) {
                result[item[key]] = []
            }
            result[item[key]].push(item)
        })
        return result
    },
    selectProps: function (...props) {
        return function (obj) {
            const newObj = {};
            props.forEach(name => {
                let names = name.split('.');
                let value = obj;

                for (const item in names) {
                    value = getValue(value, names[item]);
                }
                newObj[names[names.length - 1]] = value;
            });

            return newObj;
        }
    },
    capitalizeFirstLetter: function (string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
}