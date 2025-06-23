var $btnFilter = $('.btn-collapse-filters'),
    $filterForm = $('.filter-form');

$btnFilter.on('click', function () {
    $(this).toggleClass('btn-success btn-outline-secondary');
});

$filterForm.on('change', 'select', function () {
    $filterForm.submit();
});