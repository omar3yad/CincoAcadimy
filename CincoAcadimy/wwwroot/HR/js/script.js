// Sidebar toggle for mobile
const sidebarToggle = document.querySelector('.sidebar-toggle');
const sidebar = document.querySelector('.sidebar');
if (sidebarToggle) {
    sidebarToggle.addEventListener('click', () => {
        sidebar.classList.toggle('collapsed');
    });
}

// Profile dropdown
const profileBtn = document.querySelector('.profile-dropdown-btn');
const profileDropdown = document.getElementById('profileDropdown');
if (profileBtn) {
    profileBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        profileDropdown.hidden = !profileDropdown.hidden;
    });
    document.addEventListener('click', () => {
        profileDropdown.hidden = true;
    });
}

// Notification bell
const notifBtn = document.querySelector('.notification-btn');
const notifPanel = document.getElementById('notificationPanel');
const notifDot = document.getElementById('notifDot');
let hasNotifications = true; // Simulate new notifications

if (notifBtn) {
    notifBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        notifPanel.hidden = !notifPanel.hidden;
        if (!notifPanel.hidden) notifDot.style.display = 'none';
    });
    document.addEventListener('click', () => {
        notifPanel.hidden = true;
    });
    if (hasNotifications) notifDot.style.display = 'inline-block';
    else notifDot.style.display = 'none';
}

// Accessibility: close dropdowns/panels with Esc
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
        if (profileDropdown) profileDropdown.hidden = true;
        if (notifPanel) notifPanel.hidden = true;
    }
});