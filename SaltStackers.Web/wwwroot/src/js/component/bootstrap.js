import { createPopper } from '@popperjs/core';
import * as bootstrap from 'bootstrap';
window.bootstrap = bootstrap;

const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl));