// Student Dashboard JavaScript with API Integration
document.addEventListener('DOMContentLoaded', function () {
    // Initialize the dashboard
    initDashboard();

    // Set up event listeners
    setupEventListeners();

    // Load course data from API
    loadCourseData();
    document.addEventListener("click", (event) => {
        const button = event.target.closest(".btn-primary");

        if (button && button.dataset.courseId) {
            const courseId = button.dataset.courseId;
            // Redirect to the course page with query parameter
            window.location.href = `https://localhost:44380/Students/html/course.html?course=${courseId}`;
        }
    });
});

// API Configuration
const API_BASE_URL = 'https://localhost:44380/api';
const STUDENT_ID = localStorage.getItem("studentId");

// Initialize dashboard components
function initDashboard() {
    console.log('Cinco Academy Dashboard initialized');

    // Update current date if needed
    updateCurrentDate();

    // Set active menu item based on current page
    setActiveMenuItem();
}

// Set up event listeners for interactive elements
function setupEventListeners() {
    // Notification icon click
    const notificationIcon = document.querySelector('.notification-icon');
    if (notificationIcon) {
        notificationIcon.addEventListener('click', function () {
            alert('You have 3 new notifications');
        });
    }

    // Profile click
    const profile = document.querySelector('.profile');
    if (profile) {
        profile.addEventListener('click', function () {
            alert('Profile menu would open here');
        });
    }

    // Menu item clicks
    const menuItems = document.querySelectorAll('.menu-item a');
    menuItems.forEach(item => {
        item.addEventListener('click', function (e) {
            //e.preventDefault();

            // Remove active class from all menu items
            menuItems.forEach(i => {
                i.parentElement.classList.remove('active');
            });

            // Add active class to clicked menu item
            this.parentElement.classList.add('active');

            // In a real application, this would navigate to the selected page
            const pageName = this.querySelector('span').textContent;
            console.log(`Navigating to ${pageName} page`);
        });
    });

    // Retry button for error state
    const retryBtn = document.getElementById('retry-btn');
    if (retryBtn) {
        retryBtn.addEventListener('click', function () {
            loadCourseData();
        });
    }

    // Mobile menu toggle (for smaller screens)
    setupMobileMenu();
}

// Load course data from API
async function loadCourseData() {
    showLoadingState();
    hideErrorState();
    hideCourseSections();

    try {
        const response = await fetch(`${API_BASE_URL}/Course/ongoing/${STUDENT_ID}`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const courses = await response.json();

        // Hide loading state
        hideLoadingState();

        if (courses && courses.length > 0) {
            // Process and display course data
            processCourseData(courses);
            showCourseSections();
        } else {
            // Show empty state if no courses
            showEmptyState();
        }

    } catch (error) {
        console.error('Error loading course data:', error);
        hideLoadingState();
        showErrorState();
    }
}

// Process course data and update the UI
function processCourseData(courses) {
    // Update progress summary
    updateProgressSummary(courses);

    // Update continue learning section with the first course
    updateContinueLearning(courses[0]);

    // Update my courses section
    updateMyCourses(courses);

    // Animate progress bars
    animateProgressBars();
}

// Update progress summary in the welcome section
function updateProgressSummary(courses) {
    const overallProgress = document.getElementById('overall-progress');
    const activeCourses = document.getElementById('active-courses');
    const completedLessons = document.getElementById('completed-lessons');

    //if (overallProgress) {
    //    // Calculate average progress across all courses
    //    const totalProgress = courses.reduce((sum, course) => sum + course.progress, 0);
    //    const averageProgress = courses.length > 0 ? Math.round(totalProgress / courses.length) : 0;
    //    overallProgress.textContent = `${averageProgress}%`;
    //}

    //if (activeCourses) {
    //    activeCourses.textContent = courses.length;
    //}

    //if (completedLessons) {
    //    // Calculate completed lessons (this would need more detailed API data)
    //    // For now, we'll estimate based on progress
    //    const estimatedLessons = courses.reduce((sum, course) => {
    //        return sum + Math.round((course.progress / 100) * 10); // Assuming 10 lessons per course
    //    }, 0);
    //    completedLessons.textContent = estimatedLessons;
    //}
}

// Update continue learning section
function updateContinueLearning(course) {
    const continueCard = document.getElementById('continue-card');

    if (!continueCard) return;

    // Determine thumbnail based on course title
    const thumbnailUrl = getCourseThumbnail(course.title);

    continueCard.innerHTML = `
        <div class="course-thumbnail">
            <img src="${thumbnailUrl}" alt="${course.title}">
        </div>
        <div class="course-info">
            <h3>${course.title}</h3>
            <p class="instructor-name">Instructor: ${course.instructorName}</p>
            <p>${course.description}</p>
            <div class="progress-bar">
                <div class="progress-fill" style="width: ${course.progress}%"></div>
            </div>
            <div class="progress-text">${course.progress}% Complete</div>
            <button class="btn-primary" data-course-id="${course.id}">Continue Course</button>
        </div>
    `;

    // Add event listener to the continue button
    const continueButton = continueCard.querySelector('.btn-primary');
    if (continueButton) {
        continueButton.addEventListener('click', function () {
            const courseId = this.getAttribute('data-course-id');
            navigateToCourse(courseId);
        });
    }
}

// Update my courses section
function updateMyCourses(courses) {
    const coursesGrid = document.getElementById('courses-grid');

    if (!coursesGrid) return;

    coursesGrid.innerHTML = '';

    courses.forEach(course => {
        const thumbnailUrl = getCourseThumbnail(course.title);

        const courseCard = document.createElement('div');
        courseCard.className = 'course-card';
        courseCard.innerHTML = `
            <div class="course-image">
                <img src="${thumbnailUrl}" alt="${course.title}">
            </div>
            <div class="course-content">
                <h3>${course.title}</h3>
                <p class="instructor-name">Instructor: ${course.instructorName}</p>
                <p>${course.description}</p>
                <div class="progress-bar">
                    <div class="progress-fill" style="width: ${course.progress}%"></div>
                </div>
                <div class="course-footer">
                    <span class="progress-text">${course.progress}% Complete</span>
                    <button class="btn-primary" data-course-id="${course.id}">Go to Course</button>
                </div>
            </div>
        `;

        coursesGrid.appendChild(courseCard);

        // Add event listener to the course button
        const courseButton = courseCard.querySelector('.btn-primary');
        if (courseButton) {
            courseButton.addEventListener('click', function () {
                const courseId = this.getAttribute('data-course-id');
                navigateToCourse(courseId);
            });
        }
    });
}

// Get appropriate thumbnail based on course title
function getCourseThumbnail(courseTitle) {
    const thumbnails = {
        'web development': 'https://images.unsplash.com/photo-1555066931-4365d14bab8c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1170&q=80',
        'data science': 'https://images.unsplash.com/photo-1555949963-aa79dcee981c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1170&q=80',
        'ui/ux': 'https://images.unsplash.com/photo-1551288049-bebda4e38f71?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1170&q=80',
        'default': 'https://images.unsplash.com/photo-1547658719-da2b51169166?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1064&q=80'
    };

    const titleLower = courseTitle.toLowerCase();

    if (titleLower.includes('web')) {
        return thumbnails['web development'];
    } else if (titleLower.includes('data')) {
        return thumbnails['data science'];
    } else if (titleLower.includes('ui') || titleLower.includes('ux')) {
        return thumbnails['ui/ux'];
    } else {
        return thumbnails['default'];
    }
}

// Navigate to course (placeholder function)
//function navigateToCourse(courseId) {
//    console.log(`Navigating to course ${courseId}`);
//    // In a real application, this would redirect to the course page
//    alert(`Redirecting to course ${courseId}`);
//}

// UI State Management Functions
function showLoadingState() {
    const loadingState = document.getElementById('loading-state');
    if (loadingState) {
        loadingState.style.display = 'flex';
    }
}

function hideLoadingState() {
    const loadingState = document.getElementById('loading-state');
    if (loadingState) {
        loadingState.style.display = 'none';
    }
}

function showErrorState() {
    const errorState = document.getElementById('error-state');
    if (errorState) {
        errorState.style.display = 'flex';
    }
}

function hideErrorState() {
    const errorState = document.getElementById('error-state');
    if (errorState) {
        errorState.style.display = 'none';
    }
}

function showCourseSections() {
    const continueLearning = document.getElementById('continue-learning');
    const myCourses = document.getElementById('my-courses');

    if (continueLearning) continueLearning.style.display = 'block';
    if (myCourses) myCourses.style.display = 'block';
}

function hideCourseSections() {
    const continueLearning = document.getElementById('continue-learning');
    const myCourses = document.getElementById('my-courses');

    if (continueLearning) continueLearning.style.display = 'none';
    if (myCourses) myCourses.style.display = 'none';
}

function showEmptyState() {
    // Create empty state if it doesn't exist
    let emptyState = document.getElementById('empty-state');

    if (!emptyState) {
        emptyState = document.createElement('div');
        emptyState.id = 'empty-state';
        emptyState.className = 'empty-state';
        emptyState.innerHTML = `
            <i class="fas fa-book-open"></i>
            <h3>No Courses Enrolled</h3>
            <p>You haven't enrolled in any courses yet. Browse our catalog to get started with your learning journey.</p>
            <button class="btn-primary">Browse Courses</button>
        `;

        const myCoursesSection = document.getElementById('my-courses');
        if (myCoursesSection) {
            myCoursesSection.appendChild(emptyState);
        }
    }

    emptyState.style.display = 'flex';
}

// Update current date in the dashboard
function updateCurrentDate() {
    const dateElements = document.querySelectorAll('.current-date');
    if (dateElements.length > 0) {
        const now = new Date();
        const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        const formattedDate = now.toLocaleDateString('en-US', options);

        dateElements.forEach(element => {
            element.textContent = formattedDate;
        });
    }
}

function setActiveMenuItem() {
    const currentPath = window.location.pathname;
    const menuItems = document.querySelectorAll(".menu-item a");

    menuItems.forEach(link => {
        const linkPath = link.getAttribute("href");
        if (linkPath && currentPath.includes(linkPath)) {
            link.parentElement.classList.add("active");
        } else {
            link.parentElement.classList.remove("active");
        }
    });
}

document.addEventListener("DOMContentLoaded", setActiveMenuItem);


// Animate progress bars when they come into view
function animateProgressBars() {
    const progressBars = document.querySelectorAll('.progress-fill');

    // Create an intersection observer to animate progress bars when they become visible
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const progressFill = entry.target;
                const width = progressFill.style.width;

                // Reset width to 0 for animation
                progressFill.style.width = '0%';

                // Animate to the target width
                setTimeout(() => {
                    progressFill.style.width = width;
                }, 300);

                // Stop observing after animation
                observer.unobserve(progressFill);
            }
        });
    }, { threshold: 0.5 });

    // Observe each progress bar
    progressBars.forEach(bar => {
        observer.observe(bar);
    });
}

// Setup mobile menu functionality
function setupMobileMenu() {
    // In a real application, this would handle mobile menu toggle
    window.addEventListener('resize', function () {
        if (window.innerWidth <= 768) {
            console.log('Mobile view activated');
        } else {
            console.log('Desktop view activated');
        }
    });
}

// Utility function to format percentage values
function formatPercentage(value) {
    return `${Math.round(value)}%`;
}

// Export functions for use in other modules (if needed)
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        initDashboard,
        setupEventListeners,
        loadCourseData,
        formatPercentage
    };
}

