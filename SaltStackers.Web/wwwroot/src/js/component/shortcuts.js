import { Shortcuts } from 'shortcuts';

const shortcuts = new Shortcuts();

shortcuts.add([
    {
        shortcut: 'F11', handler: event => {
            $('.full-screen').trigger('click');
            return true;
        }
    },
    {
        shortcut: 'Alt+X', handler: event => {
            $('.toggle-menu').trigger('click');
            return true;
        }
    },
    {
        shortcut: 'Alt+L', handler: event => {
            $('#logoff').trigger('click');
            return true;
        }
    }
]);