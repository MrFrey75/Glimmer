// Glimmer Creator - Ribbon Menu Interactions

document.addEventListener('DOMContentLoaded', function() {
    // Close dropdown menus when clicking outside
    document.addEventListener('click', function(event) {
        if (!event.target.closest('.ribbon-menu')) {
            const activeItems = document.querySelectorAll('.ribbon-item.active');
            activeItems.forEach(item => item.classList.remove('active'));
        }
    });

    // Toggle active state on click for touch devices
    const ribbonItems = document.querySelectorAll('.ribbon-item > a');
    ribbonItems.forEach(item => {
        item.addEventListener('click', function(e) {
            const parent = this.parentElement;
            const hasDropdown = parent.querySelector('.dropdown-menu');
            
            if (hasDropdown) {
                e.preventDefault();
                
                // Close other open menus
                const otherItems = document.querySelectorAll('.ribbon-item.active');
                otherItems.forEach(other => {
                    if (other !== parent) {
                        other.classList.remove('active');
                    }
                });
                
                // Toggle this menu
                parent.classList.toggle('active');
            }
        });
    });

    // Keyboard navigation for accessibility
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            const activeItems = document.querySelectorAll('.ribbon-item.active');
            activeItems.forEach(item => item.classList.remove('active'));
        }
    });

    // Prevent submenu from closing when clicking inside
    const dropdownMenus = document.querySelectorAll('.dropdown-menu');
    dropdownMenus.forEach(menu => {
        menu.addEventListener('click', function(e) {
            if (e.target.tagName !== 'A') {
                e.stopPropagation();
            }
        });
    });
});
