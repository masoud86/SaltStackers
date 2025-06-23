$('textarea').each(function () {
    this.setAttribute('style', 'height:' + this.scrollHeight + 'px;overflow-y:hidden;');
}).on('input', function () {
    this.style.height = 'auto';
    this.style.height = this.scrollHeight + 'px';
});

$('textarea[data-val-length-max]').each(function () {
    var $this = $(this);
    $this.after('<span class="text-muted small float-end counter"><span>' + $this.val().length + '</span>/<span>' + $this.data('val-length-max') + '</span></span>');
});

function counterStyleReset($counter, $current, $maximum) {
    $counter.removeClass('font-weight-bold');
    $current.removeClass('text-warning text-danger');
    $maximum.removeClass('text-warning text-danger');
}

function counterStyle(currentValue, $counter) {
    var $current = $counter.find('span:first'),
        $maximum = $counter.find('span:last'),
        max = $maximum.text(),
        criteria = (max / 3) * 2;

    if (currentValue < criteria) {
        counterStyleReset($counter, $current, $maximum);
    }
    else if (currentValue >= criteria && currentValue <= max) {
        counterStyleReset($counter, $current, $maximum);
        $current.addClass('text-warning');
    }
    else {
        counterStyleReset($counter, $current, $maximum);
        $counter.addClass('font-weight-bold');
        $current.addClass('text-danger');
        $maximum.addClass('text-danger');
    }
}

$('.page-content').on('input', 'textarea[data-val-length-max]', function () {
    debugger;
    var $this = $(this);
    var characterCount = $this.val().length,
        $counter = $this.next('.counter'),
        $current = $counter.find('span:first');

    $current.text(characterCount);
    counterStyle(characterCount, $counter);
});