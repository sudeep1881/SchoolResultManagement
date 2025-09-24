document.addEventListener("DOMContentLoaded", function () {
    let fileInput = document.getElementById("fileProfileImage");
    let errorSpan = document.getElementById("fileError");
    let form = fileInput.closest("form"); // get the parent form

    if (fileInput) {
        // Validate when file changes
        fileInput.addEventListener("change", function () {
            validateFile();
        });

        // Validate when form is submitted
        form.addEventListener("submit", function (e) {
            if (!validateFile()) {
                e.preventDefault(); // stop form submit
            }
        });
    }

    function validateFile() {
        errorSpan.textContent = "";

        let file = fileInput.files[0];
        if (!file) {
            errorSpan.textContent = "Profile image is required.";
            return false;
        }

        // ✅ Check file size (2MB limit)
        let maxSize = 2 * 1024 * 1024;
        if (file.size > maxSize) {
            errorSpan.textContent = "File size must be less than 2MB.";
            fileInput.value = "";
            return false;
        }

        // ✅ Check file extension
        let allowedExtensions = [".jpg", ".jpeg", ".png"];
        let fileName = file.name.toLowerCase();
        let isValidExt = allowedExtensions.some(ext => fileName.endsWith(ext));

        if (!isValidExt) {
            errorSpan.textContent = "Only JPG, JPEG, PNG files are allowed.";
            fileInput.value = "";
            return false;
        }

        return true; // ✅ valid
    }
});