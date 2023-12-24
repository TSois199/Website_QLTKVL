$(document).ready(function () {
    $('#eye').click(function () {
        $(this).toggleClass('fa-eye fa-eye-slash');
        var input = $(this).prev();
        if (input.attr('type') === 'password') {
            input.attr('type', 'text');
        } else {
            input.attr('type', 'password');
        }
    });
});
