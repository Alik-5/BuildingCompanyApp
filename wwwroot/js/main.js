document.addEventListener("DOMContentLoaded", () => {
    initMenu();
    initReveal();
    initCounters();
});

function initMenu() {
    const menuToggle = document.getElementById("menuToggle");
    const sidebar = document.getElementById("sidebar");

    if (!menuToggle || !sidebar) {
        return;
    }

    menuToggle.addEventListener("click", () => {
        sidebar.classList.toggle("active");
    });

    document.addEventListener("click", (event) => {
        if (!event.target.closest("#sidebar") && !event.target.closest("#menuToggle")) {
            sidebar.classList.remove("active");
        }
    });
}

function initReveal() {
    const elements = document.querySelectorAll("[data-reveal]");
    if (!("IntersectionObserver" in window)) {
        elements.forEach((element) => element.classList.add("revealed"));
        return;
    }

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.classList.add("revealed");
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.15 });

    elements.forEach((element, index) => {
        element.style.transitionDelay = `${Math.min(index * 70, 280)}ms`;
        observer.observe(element);
    });
}

function initCounters() {
    const counters = document.querySelectorAll("[data-count]");
    counters.forEach((counter) => {
        const target = Number(counter.getAttribute("data-count"));
        if (Number.isNaN(target)) {
            return;
        }

        const duration = 1100;
        const start = performance.now();

        function tick(timestamp) {
            const progress = Math.min((timestamp - start) / duration, 1);
            const value = Math.round(target * easeOutCubic(progress));
            counter.textContent = Intl.NumberFormat("en-US").format(value);

            if (progress < 1) {
                requestAnimationFrame(tick);
            }
        }

        requestAnimationFrame(tick);
    });
}

function easeOutCubic(value) {
    return 1 - Math.pow(1 - value, 3);
}

function confirmDelete(message = "Are you sure you want to delete this item?") {
    return window.confirm(message);
}
