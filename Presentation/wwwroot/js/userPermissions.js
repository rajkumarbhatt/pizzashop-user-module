var parentCheckbox = document.getElementById('flexCheckIndeterminate');
parentCheckbox.addEventListener('change', e => {
    document.querySelectorAll('.permission-checkbox').forEach(checkbox => {
        checkbox.checked = e.target.checked;
    });
});

document.querySelectorAll('.permission-checkbox').forEach(checkbox => {
    checkbox.addEventListener('change', () => {
        var tbodyCheckbox = document.querySelectorAll('.permission-checkbox').length;
        var tbodyCheckedbox = document.querySelectorAll('.permission-checkbox:checked').length;
        if (tbodyCheckbox === tbodyCheckedbox) {
            // All selected
            parentCheckbox.indeterminate = false;
            parentCheckbox.checked = true;
        } else if (tbodyCheckedbox > 0) {
            // Some selected
            parentCheckbox.indeterminate = true;
            parentCheckbox.checked = false;
        } else {
            // None selected
            parentCheckbox.indeterminate = false;
            parentCheckbox.checked = false;
        }
    });
});