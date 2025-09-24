export function closeMenuIfMobile(checkboxId) {
    const chk = document.getElementById(checkboxId);
    if (chk && window.matchMedia('(max-width: 768px)').matches) chk.checked = false;
}