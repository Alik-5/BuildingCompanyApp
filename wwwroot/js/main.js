document.addEventListener("DOMContentLoaded", () => {
    initMenu();
    initReveal();
    initCounters();
    initHoverEffects();
    initSmoothScroll();
});

function initMenu() {
    const menuToggle = document.getElementById("menuToggle");
    const sidebar = document.getElementById("sidebar");

    if (!menuToggle || !sidebar) {
        return;
    }

    menuToggle.addEventListener("click", (e) => {
        e.stopPropagation();
        sidebar.classList.toggle("active");
        menuToggle.classList.toggle("active");
    });

    // Close menu when a link is clicked
    sidebar.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", () => {
            sidebar.classList.remove("active");
            menuToggle.classList.remove("active");
        });
    });

    document.addEventListener("click", (event) => {
        if (!event.target.closest("#sidebar") && !event.target.closest("#menuToggle")) {
            sidebar.classList.remove("active");
            menuToggle.classList.remove("active");
        }
    });

    // Prevent sidebar close on scroll for mobile
    sidebar.addEventListener("click", (e) => e.stopPropagation());
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
                entry.target.style.animation = `fadeInScale 0.8s cubic-bezier(0.34, 1.56, 0.64, 1) forwards`;
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.15 });

    elements.forEach((element, index) => {
        element.style.animationDelay = `${Math.min(index * 80, 300)}ms`;
        observer.observe(element);
    });
}

function initCounters() {
    const counters = document.querySelectorAll("[data-count]");
    
    if (!("IntersectionObserver" in window)) {
        counters.forEach(counter => animateCounter(counter));
        return;
    }

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                animateCounter(entry.target);
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    counters.forEach((counter) => observer.observe(counter));
}

function animateCounter(counter) {
    const target = Number(counter.getAttribute("data-count"));
    if (Number.isNaN(target)) {
        return;
    }

    const duration = 1500;
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
}

function easeOutCubic(value) {
    return 1 - Math.pow(1 - value, 3);
}

function initHoverEffects() {
    // Add hover animations to cards
    document.querySelectorAll(".panel-card, .insight-card").forEach(card => {
        card.addEventListener("mouseenter", function() {
            this.style.transform = "translateY(-4px)";
            this.style.transition = "all 0.4s cubic-bezier(0.34, 1.56, 0.64, 1)";
        });
        card.addEventListener("mouseleave", function() {
            this.style.transform = "translateY(0)";
        });
    });
}

function initSmoothScroll() {
    // Add scroll behavior to all anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href === '#') return;
            
            e.preventDefault();
            const target = document.querySelector(href);
            if (target) {
                target.scrollIntoView({ behavior: 'smooth', block: 'start' });
            }
        });
    });
}

function confirmDelete(message = "Are you sure you want to delete this item?") {
    return window.confirm(message);
}
