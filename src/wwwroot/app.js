function setDarkMode(darkMode) {
    if (darkMode)
        document.getElementsByTagName('body')[0].classList.add('bgDarkMode');
    else
        document.getElementsByTagName('body')[0].classList.remove('bgDarkMode');
}
